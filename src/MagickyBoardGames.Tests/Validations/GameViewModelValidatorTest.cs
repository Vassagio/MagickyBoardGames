using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MagickyBoardGames.Validations;
using MagickyBoardGames.ViewModels;
using Xunit;

namespace MagickyBoardGames.Tests.Validations
{
    public class GameViewModelValidatorTest
    {
        [Theory]
        [InlineData(1)]
        [InlineData(50)]
        [InlineData(100)]
        public void Valid(int length) {
            var viewModel = BuildGameViewModel(new string('x', length));                
            var validator = new GameSaveViewModelValidator();

            var results = validator.Validate(viewModel);

            results.Errors.Should().BeEmpty();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void Must_Have_A_Name(string name) {
            var viewModel = BuildGameViewModel();
            viewModel.Game.Name = name;
            var validator = new GameSaveViewModelValidator();

            var results = validator.Validate(viewModel);

            results.Errors.First().ErrorMessage.Should().Be("Must have a name.");
        }

        [Theory]
        [InlineData(101)]
        [InlineData(1000)]
        public void Must_Be_Less_Than_Or_Equal_To_30_Characters(int length) {
            var viewModel = BuildGameViewModel(new string('x', length));
            var validator = new GameSaveViewModelValidator();

            var results = validator.Validate(viewModel);

            results.Errors.First().ErrorMessage.Should().Be("Must be less than or equal to 100 characters.");
        }

        [Fact]
        public void Min_Players_Must_Be_Greater_Than_0() {
            var viewModel = new GameSaveViewModel {
                Game = new GameViewModel {
                    Name = "Name",
                    Description = "Description",
                    MinPlayers = 0,
                    MaxPlayers = 10
                }
            };

            var validator = new GameSaveViewModelValidator();

            var results = validator.Validate(viewModel);

            results.Errors.First().ErrorMessage.Should().Be("Min Players must be greater than 0.");
        }

        [Fact]
        public void Max_Players_Must_Be_Greater_Than_0() {
            var viewModel = new GameSaveViewModel {
                Game = new GameViewModel {
                    Name = "Name",
                    Description = "Description",
                    MinPlayers = 1,
                    MaxPlayers = 0
                }
            };

            var validator = new GameSaveViewModelValidator();

            var results = validator.Validate(viewModel);

            results.Errors.First().ErrorMessage.Should().Be("Max Players must be greater than 0.");
        }

        [Theory]
        [InlineData(100, 12)]
        public void Must_Be_Greater_Than_MinPlayers(int minPlayers, int maxPlayers) {
            var viewModel = BuildGameViewModel(minPlayers: minPlayers, maxPlayers: maxPlayers);
            var validator = new GameSaveViewModelValidator();

            var results = validator.Validate(viewModel);

            results.Errors.First().ErrorMessage.Should().Be("Must be greater than or equal to min players.");
        }

        private static GameSaveViewModel BuildGameViewModel(string name = null, string description = null, int? minPlayers = null, int? maxPlayers = null) {
            name = name ?? "Name";
            description = description?? "Description";
            minPlayers = minPlayers ?? 1;
            maxPlayers = maxPlayers ?? 10;
            return new GameSaveViewModel {
                Game = new GameViewModel {
                    Name = name,
                    Description = description,
                    MinPlayers = minPlayers.Value,
                    MaxPlayers = maxPlayers.Value
                }
            };            
        }
    }
}
