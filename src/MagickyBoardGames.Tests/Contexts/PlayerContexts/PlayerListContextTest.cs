using System.Collections.Generic;
using FluentAssertions;
using MagickyBoardGames.Builders;
using MagickyBoardGames.Contexts.PlayerContexts;
using MagickyBoardGames.Models;
using MagickyBoardGames.Repositories;
using MagickyBoardGames.Tests.Mocks;
using MagickyBoardGames.ViewModels;
using Xunit;

namespace MagickyBoardGames.Tests.Contexts.PlayerContexts
{
    public class PlayerListContextTest
    {
        [Fact]
        public void Initializes() {
            var context = BuildPlayerListContext();
            context.Should().NotBeNull();
        }

        [Fact]
        public async void Returns_Empty_View_Model_When_Not_Found() {
            var userRepository = new MockUserRepository().GetByStubbedToReturn(null);
            var playerBuilder = new MockBuilder<ApplicationUser, PlayerViewModel>();
            var context = BuildPlayerListContext(userRepository, playerBuilder);

            var playerListViewModel = await context.BuildViewModel();

            playerListViewModel.Should().BeOfType<PlayerListViewModel>();
            userRepository.VerifyGetAllCalled();
            playerBuilder.VerifyBuildNotCalled();
        }

        [Fact]
        public async void Build_View_Model() {
            var entity = new ApplicationUser();
            var entities = new List<ApplicationUser> {entity};
            var viewModel = new PlayerViewModel();
            var repository = new MockUserRepository().GetAllStubbedToReturn(entities);
            var builder = new MockBuilder<ApplicationUser, PlayerViewModel>().BuildStubbedToReturn(viewModel);
            var context = BuildPlayerListContext(repository, builder);

            var playerListViewModel = await context.BuildViewModel();

            playerListViewModel.Should().BeOfType<PlayerListViewModel>();
            repository.VerifyGetAllCalled();
            builder.VerifyBuildCalled(entity);
        }

        private static PlayerListContext BuildPlayerListContext(IUserRepository repository = null, IBuilder<ApplicationUser, PlayerViewModel> builder = null) {
            repository = repository ?? new MockUserRepository();
            builder = builder ?? new MockBuilder<ApplicationUser, PlayerViewModel>();
            return new PlayerListContext(repository, builder);
        }
    }
}
