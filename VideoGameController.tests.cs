using Moq;
using Microsoft.AspNetCore.Mvc;
using GameCatalog.Controllers;
using GameCatalog.Models;

public class VideoGameController_Should
{
    [Fact]
    public async Task ReturnListOfGames()
    {
        var mockRepo = new Mock<IGameRepo>();
        var testGames = new List<Game>
        {
            new Game { Id = 1, Title = "some game" },
            new Game { Id = 2, Title = "another game" }
        };

        mockRepo.Setup(repo => repo.GetGames()).ReturnsAsync(testGames);

        var controller = new VideoGameController(mockRepo.Object);

        var games = await controller.GetGames();

        var response = Assert.IsType<ActionResult<IEnumerable<Game>>>(games);
        var okResult = Assert.IsType<OkObjectResult>(response.Result);
        var returnedGames = Assert.IsType<List<Game>>(okResult.Value);

        Assert.Equal(2, returnedGames.Count());
    }

    [Fact]
    public async Task ReturnGameById()
    {
        var mockRepo = new Mock<IGameRepo>();
        var testGame = new Game { Id = 1, Title = "some game" };
        mockRepo.Setup(repo => repo.GetGame(1)).ReturnsAsync(testGame);

        var controller = new VideoGameController(mockRepo.Object);

        var game = await controller.GetGame(1);

        Assert.NotNull(game);
        Assert.Equal(testGame, game.Value);
    }

    [Fact]
    public async Task ReturnNotFoundWhenGameNotFound()
    {
        var mockRepo = new Mock<IGameRepo>();
        mockRepo.Setup(repo => repo.GetGame(1)).ReturnsAsync(null as Game);

        var controller = new VideoGameController(mockRepo.Object);

        var game = await controller.GetGame(1);

        var response = Assert.IsType<ActionResult<Game>>(game);
        Assert.IsType<NotFoundResult>(response.Result);
    }

    [Fact]
    public async Task ReturnNewGameId()
    {
        var mockRepo = new Mock<IGameRepo>();
        var testGame = new Game { Id = 1, Title = "some game" };
        mockRepo.Setup(repo => repo.NewGame(testGame)).ReturnsAsync(1);

        var controller = new VideoGameController(mockRepo.Object);

        var result = await controller.NewGame(testGame);

        Assert.NotNull(result);
        Assert.Equal(1, result.Value);
    }

    [Fact]
    public async Task ReturnUpdatedGameId()
    {
        var mockRepo = new Mock<IGameRepo>();
        var testGame = new Game { Id = 1, Title = "some game" };
        mockRepo.Setup(repo => repo.UpdateGame(1, testGame)).ReturnsAsync(1);

        var controller = new VideoGameController(mockRepo.Object);

        var result = await controller.UpdateGame(1, testGame);

        Assert.NotNull(result);
        Assert.Equal(1, result.Value);
    }

    [Fact]
    public async Task ReturnBadRequestIfIdsDontMatch()
    {
        var mockRepo = new Mock<IGameRepo>();
        var testGame = new Game { Id = 10, Title = "some game" };
        mockRepo.Setup(repo => repo.UpdateGame(1, testGame)).ThrowsAsync(new InvalidOperationException());

        var controller = new VideoGameController(mockRepo.Object);

        var result = await controller.UpdateGame(1, testGame);

        Assert.NotNull(result);
        Assert.IsType<BadRequestResult>(result.Result);
    }

    // TODO: Add tests for image ops. Running out of time.
}