using MagickyBoardGames.Builders;
using MagickyBoardGames.Contexts.GameContexts;
using MagickyBoardGames.Models;
using MagickyBoardGames.Repositories;
using MagickyBoardGames.Tests.Mocks;
using MagickyBoardGames.ViewModels;
using Xunit;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;

namespace MagickyBoardGames.Tests.Contexts.GameContexts {
    public class GameSaveContextTest {
        [Fact]
        public void Initialize_Game_Save_Context() {
            var context = BuildGameSaveContext();
            context.Should().NotBeNull();
        }

        [Fact]
        public void Validates() {
            var viewModel = new GameViewModel();
            var validator = new MockValidator<GameViewModel>().ValidateStubbedToReturnValid();
            var context = BuildGameSaveContext(validator: validator);

            var results = context.Validate(viewModel);

            results.Should().BeOfType<ValidationResult>();
        }

        [Fact]
        public async void Saves_A_New_Record() {
            var game = new Game {
                Description = "Game"
            };
            var viewModel = new GameViewModel {
                Description = "Game"
            };
            var repository = new MockRepository<Game>().GetByStubbedToReturn(null);
            var builder = new MockBuilder<Game, GameViewModel>().BuildStubbedToReturn(game);
            var context = BuildGameSaveContext(repository, builder);

            await context.Save(viewModel);

            repository.VerifyGetByCalled(game);
            repository.VerifyGetByIdNotCalled();
            repository.VerifyAddCalled(game);
            builder.VerifyBuildCalled(viewModel);
        }

        [Fact]
        public async void Does_Not_Save_When_A_New_Record_Already_Exists() {
            var game = new Game {
                Description = "Game"
            };
            var viewModel = new GameViewModel {
                Description = "Game"
            };
            var repository = new MockRepository<Game>().GetByStubbedToReturn(game);
            var builder = new MockBuilder<Game, GameViewModel>().BuildStubbedToReturn(game);
            var context = BuildGameSaveContext(repository, builder);

            await context.Save(viewModel);

            repository.VerifyGetByCalled(game);
            repository.VerifyGetByIdNotCalled();
            repository.VerifyAddNotCalled();
            repository.VerifyUpdateCalled(game);
            builder.VerifyBuildCalled(viewModel);
        }

        [Fact]
        public async void Saves_A_New_Record_When_Record_Not_Found() {
            var game = new Game {
                Id = 50,
                Description = "Game"
            };
            var viewModel = new GameViewModel {
                Id = 50,
                Description = "Game"
            };
            var repository = new MockRepository<Game>().GetByStubbedToReturn(null);
            var builder = new MockBuilder<Game, GameViewModel>().BuildStubbedToReturn(game);
            var context = BuildGameSaveContext(repository, builder);

            await context.Save(viewModel);

            repository.VerifyGetByCalled(50);
            repository.VerifyGetByNotCalled();
            repository.VerifyAddCalled(game);
            builder.VerifyBuildCalled(viewModel);
        }

        [Fact]
        public async void Saves_A_Record_When_Record_Found() {
            var game = new Game {
                Id = 60,
                Description = "Game"
            };
            var viewModel = new GameViewModel {
                Id = 60,
                Description = "Game"
            };
            var repository = new MockRepository<Game>().GetByStubbedToReturn(game);
            var builder = new MockBuilder<Game, GameViewModel>().BuildStubbedToReturn(game);
            var context = BuildGameSaveContext(repository, builder);

            await context.Save(viewModel);

            repository.VerifyGetByCalled(60);
            repository.VerifyGetByNotCalled();
            repository.VerifyUpdateCalled(game);
            builder.VerifyBuildCalled(viewModel);
        }

        private static GameSaveContext BuildGameSaveContext(IRepository<Game> repository = null, IBuilder<Game, GameViewModel> builder = null, IValidator<GameViewModel> validator = null) {
            repository = repository ?? new MockRepository<Game>();
            builder = builder ?? new MockBuilder<Game, GameViewModel>();
            validator = validator ?? new MockValidator<GameViewModel>();
            return new GameSaveContext(repository, builder, validator);
        }
    }
}
