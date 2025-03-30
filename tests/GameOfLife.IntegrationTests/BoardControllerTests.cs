using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using GameOfLife.Domain.Entities;
using GameOfLife.Domain.Utilities;
using MongoDB.Bson;
using GameOfLife.API.Dto;

namespace GameOfLife.IntegrationTests
{
    // Inherit from IClassFixture so that the factory is shared between tests.
    public class BoardControllerIntegrationTests : IClassFixture<CustomFactory<Program>>
    {
        private readonly HttpClient _client;

        public BoardControllerIntegrationTests(CustomFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task UploadBoard_ReturnsValidId_And_GetNextState_Works()
        {
            // Setup the options for serialization/deserialization of bool[,]
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            options.Converters.Add(new BoolMultiDimensionalArrayConverter());

            // Arrange: create a board payload.
            var boardPayload = new BoardDTO()
            {
                Rows = 5,
                Cols = 5,
                // Create a 5x5 matrix with a horizontal "blinker" on row 2.
                // We'll initialize with all false values.
                State = new bool[5, 5]
            };

            // Set the blinker cells to true.
            boardPayload.State[2, 1] = true;
            boardPayload.State[2, 2] = true;
            boardPayload.State[2, 3] = true;

            // Serialize the payload.
            var json = JsonSerializer.Serialize(boardPayload, options);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act: Create the board.
            var createResponse = await _client.PostAsync("/api/game/", content);
            createResponse.EnsureSuccessStatusCode();
            var responseString = await createResponse.Content.ReadAsStringAsync();

            // Deserialize the response to extract the board ID.
            var createdResponse = JsonSerializer.Deserialize<Guid>(responseString, options);
            Assert.NotEqual(createdResponse, Guid.Empty);

            // Act: Request the next state.
            var nextResponse = await _client.GetAsync($"/api/game/{createdResponse}/next");
            nextResponse.EnsureSuccessStatusCode();
            var nextResponseString = await nextResponse.Content.ReadAsStringAsync();

            // Deserialize the board.
            var updatedBoard = JsonSerializer.Deserialize<bool[,]>(nextResponseString, options);
            Assert.NotNull(updatedBoard);
            Assert.Equal(5, updatedBoard.GetLength(0));
            Assert.Equal(5, updatedBoard.GetLength(1));
        }
    }
}