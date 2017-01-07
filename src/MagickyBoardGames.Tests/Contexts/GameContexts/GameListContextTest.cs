using System.Collections.Generic;
using MagickyBoardGames.Builders;
using MagickyBoardGames.Contexts.GameContexts;
using MagickyBoardGames.Models;
using MagickyBoardGames.Repositories;
using MagickyBoardGames.Tests.Mocks;
using MagickyBoardGames.ViewModels;
using Xunit;
using FluentAssertions;

namespace MagickyBoardGames.Tests.Contexts.GameContexts {
    public class GameListContextTest {
        [Fact]
        public void Initializes() {
            var context = BuildGameListContext();
            context.Should().NotBeNull();
        }

        [Fact]
        public async void Returns_Empty_View_Model_When_Not_Found() {
            var gameRepository = new MockRepository<Game>().GetByStubbedToReturn(null);
            var gameBuilder = new MockBuilder<Game, GameViewModel>();
            var context = BuildGameListContext(gameRepository, gameBuilder);

            var gameListViewModel = await context.BuildViewModel();

            gameListViewModel.Should().BeOfType<GameListViewModel>();
            gameRepository.VerifyGetAllCalled();
            gameBuilder.VerifyBuildNotCalled();
        }

        [Fact]
        public async void Build_View_Model() {
            var entity = new Game();
            var entities = new List<Game> { entity };
            var viewModel = new GameViewModel();
            var repository = new MockRepository<Game>().GetAllStubbedToReturn(entities);
            var builder = new MockBuilder<Game, GameViewModel>().BuildStubbedToReturn(viewModel);
            var context = BuildGameListContext(repository, builder);

            var gameListViewModel = await context.BuildViewModel();

            gameListViewModel.Should().BeOfType<GameListViewModel>();
            repository.VerifyGetAllCalled();
            builder.VerifyBuildCalled(entity);
        }

        private static GameListContext BuildGameListContext(IRepository<Game> repository = null, IBuilder<Game, GameViewModel> builder = null) {
            repository = repository ?? new MockRepository<Game>();
            builder = builder ?? new MockBuilder<Game, GameViewModel>();
            return new GameListContext(repository, builder);
        }
    }
}
