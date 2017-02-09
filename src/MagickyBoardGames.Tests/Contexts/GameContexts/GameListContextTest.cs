using System.Collections.Generic;
using System.Linq;
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
            var gameRepository = new MockGameRepository().GetByStubbedToReturn(null);
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
            var repository = new MockGameRepository().GetAllStubbedToReturn(entities);
            var builder = new MockBuilder<Game, GameViewModel>().BuildStubbedToReturn(viewModel);
            var context = BuildGameListContext(repository, builder);

            var gameListViewModel = await context.BuildViewModel();

            gameListViewModel.Should().BeOfType<GameListViewModel>();
            repository.VerifyGetAllCalled();
            builder.VerifyBuildCalled(entity);
        }

        [Fact]
        public async void Build_View_Model_Filter_By_Name() {
            var entity1 = new Game { Name = "Something"};
            var entity2 = new Game { Name = "Game Name" };
            var entity3 = new Game { Name = "Name" };
            var entities = new List<Game> { entity1, entity2, entity3 };
            var viewModel2 = new GameViewModel { Name = entity2.Name };
            var viewModel3 = new GameViewModel { Name = entity3.Name };
            var listViewModel = new GameListViewModel {
                Filter = new GameFilterViewModel {
                    Name = "Name"
                }
            };
            var repository = new MockGameRepository().GetAllStubbedToReturn(entities);
            var builder = new MockBuilder<Game, GameViewModel>().BuildStubbedToReturn(viewModel2, viewModel3);
            var context = BuildGameListContext(repository, builder);

            var gameListViewModel = await context.BuildViewModel(listViewModel);

            gameListViewModel.Should().BeOfType<GameListViewModel>();
            gameListViewModel.Games.Count().Should().Be(2);
            repository.VerifyGetAllCalled();
            builder.VerifyBuildNotCalled(entity1);
            builder.VerifyBuildCalled(entity2);
            builder.VerifyBuildCalled(entity3);
        }

        private static GameListContext BuildGameListContext(IGameRepository repository = null, IBuilder<Game, GameViewModel> builder = null) {
            repository = repository ?? new MockGameRepository();
            builder = builder ?? new MockBuilder<Game, GameViewModel>();
            return new GameListContext(repository, builder);
        }
    }
}
