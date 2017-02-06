using FluentAssertions;
using MagickyBoardGames.Builders;
using MagickyBoardGames.Services.BoardGameGeek;
using MagickyBoardGames.Tests.Mocks;
using MagickyBoardGames.ViewModels;
using System.Linq;
using Xunit;

namespace MagickyBoardGames.Tests.Services.BoardGameGeek
{
    public class BoardGameGeekServiceTest
    {
        [Fact]
        public async void Search_For_Game() {
            var service = BuildBoardGameGeekService();
            var result = await service.Search("Settlers Of Catan");
            result.Should().NotBeNull();
        }

        [Fact]
        public async void Load_A_Game() {
            var gameViewModel = new GameViewModel();
            var xmlElementBuilder = new MockXmlElementBuilder<GameViewModel>().BuildStubbedToReturn(gameViewModel);
            var service = BuildBoardGameGeekService(xmlElementBuilder);
            var result = await service.LoadGames(37111);
            result.Should().NotBeNull();
            result.Count().Should().Be(1);
            result.First().Should().Be(gameViewModel);
            xmlElementBuilder.VerifyBuildCalled();  
        }

        [Fact]
        public async void Load_Multiple_Games() {
            var gameViewModel1 = new GameViewModel();
            var gameViewModel2 = new GameViewModel();
            var xmlElementBuilder = new MockXmlElementBuilder<GameViewModel>().BuildStubbedToReturn(gameViewModel1, gameViewModel2);
            var service = BuildBoardGameGeekService(xmlElementBuilder);
            var result = await service.LoadGames(37111, 35424);
            result.Should().NotBeNull();
            result.Count().Should().Be(2);
            result.First().Should().Be(gameViewModel1);
            result.Last().Should().Be(gameViewModel2);
            xmlElementBuilder.VerifyBuildCalled(2);
        }

        private static BoardGameGeekService BuildBoardGameGeekService(IXmlElementBuilder<GameViewModel> xmlElementBuilder = null) {
            xmlElementBuilder = xmlElementBuilder ?? new MockXmlElementBuilder<GameViewModel>();
            return new BoardGameGeekService(xmlElementBuilder);
        }
    }
}
