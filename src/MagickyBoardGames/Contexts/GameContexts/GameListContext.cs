using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MagickyBoardGames.Builders;
using MagickyBoardGames.Models;
using MagickyBoardGames.Repositories;
using MagickyBoardGames.ViewModels;

namespace MagickyBoardGames.Contexts.GameContexts {
    public class GameListContext : IGameListContext {
        private readonly IGameRepository _gameRepository;
        private readonly IBuilder<Game, GameViewModel> _gameBuilder;

        public GameListContext(IGameRepository gameRepository, IBuilder<Game, GameViewModel> gameBuilder) {
            _gameRepository = gameRepository;
            _gameBuilder = gameBuilder;
        }

        //TODO: Needs Testing
        public async Task<GameListViewModel> BuildViewModel(GameListViewModel viewModel = null) {
            viewModel = viewModel ?? new GameListViewModel();
            var games = (await _gameRepository.GetAll())
                         .ToSearchableQuery()
                         .FilterByName(viewModel.Filter.Name)
                         .FilterByDescription(viewModel.Filter.Description)
                         .FilterByNumberOfPlayers(viewModel.Filter.NumberOfPlayers)
                         .FilterByAverageRating(viewModel.Filter.Rating)
                         .Execute();
                            
            viewModel.Games = games.Select(game => _gameBuilder.Build(game)).ToList();
            return viewModel;
        }
    }
}
