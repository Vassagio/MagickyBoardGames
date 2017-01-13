using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using MagickyBoardGames.Builders;
using MagickyBoardGames.Models;
using MagickyBoardGames.Repositories;
using MagickyBoardGames.ViewModels;

namespace MagickyBoardGames.Contexts.GameContexts {
    public class GameSaveContext : IGameSaveContext {
        private readonly IGameRepository _gameRepository;
        private readonly IBuilder<Game, GameViewModel> _gameBuilder;
        private readonly IValidator<GameSaveViewModel> _validator;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBuilder<Category, CategoryViewModel> _categoryBuilder;
        private readonly IUserRepository _userRepository;
        private readonly IBuilder<ApplicationUser, OwnerViewModel> _ownerBuilder;
        private readonly IRatingRepository _ratingRepository;
        private readonly IBuilder<Rating, RatingViewModel> _ratingBuilder;

        public GameSaveContext(IGameRepository gameRepository, IBuilder<Game, GameViewModel> gameBuilder, IValidator<GameSaveViewModel> validator, 
                               ICategoryRepository categoryRepository, IBuilder<Category, CategoryViewModel> categoryBuilder, 
                               IUserRepository userRepository, IBuilder<ApplicationUser, OwnerViewModel> ownerBuilder,
                               IRatingRepository ratingRepository, IBuilder<Rating, RatingViewModel> ratingBuilder) {
            _gameRepository = gameRepository;
            _gameBuilder = gameBuilder;
            _validator = validator;
            _categoryRepository = categoryRepository;
            _categoryBuilder = categoryBuilder;
            _userRepository = userRepository;
            _ownerBuilder = ownerBuilder;
            _ratingRepository = ratingRepository;
            _ratingBuilder = ratingBuilder;
        }

        public ValidationResult Validate(GameSaveViewModel viewModel) {
            return _validator.Validate(viewModel);
        }

        public async Task Save(GameSaveViewModel viewModel) {
            var game = _gameBuilder.Build(viewModel.Game);
            var categories = await GetCategories(viewModel.CategoryIds);
            var owners = await GetOwners(viewModel.OwnerIds);
            if (viewModel.Game.Id.HasValue)
                await Save(await _gameRepository.GetBy(viewModel.Game.Id.Value), game, categories, owners);
            else
                await Save(await _gameRepository.GetBy(viewModel.Game.Name), game, categories, owners);
        }

        private async Task<IEnumerable<Category>> GetCategories(IEnumerable<int> categoryIds) {
            var categories = new List<Category>();
            foreach (var categoryId in categoryIds) {
                var category = await _categoryRepository.GetBy(categoryId);
                categories.Add(category);
            }
            return categories;
        }

        private async Task<IEnumerable<ApplicationUser>> GetOwners(IEnumerable<string> ownerIds) {
            var owners = new List<ApplicationUser>();
            foreach (var ownerId in ownerIds) {
                var owner = await _userRepository.GetById(ownerId);
                owners.Add(owner);
            }
            return owners;
        }

        private async Task Save(Game found, Game game, IEnumerable<Category> categories, IEnumerable<ApplicationUser> owners) {
            if (found != null)
                await Update(found, game, categories, owners);
            else
                await Insert(game, categories, owners);
        }

        private async Task Update(Game found, Game game, IEnumerable<Category> categories, IEnumerable<ApplicationUser> owners) {
            found.Name = game.Name;
            found.Description = game.Description;
            found.MinPlayers = game.MinPlayers;
            found.MaxPlayers = game.MaxPlayers;
            await _gameRepository.Update(found, categories, owners);
        }

        private async Task Insert(Game game, IEnumerable<Category> categories, IEnumerable<ApplicationUser> owners) {
            await _gameRepository.Add(game, categories, owners);
        }

        public async Task<GameSaveViewModel> BuildViewModel() {
            return new GameSaveViewModel {
                AvailableCategories = await BuildCategoryViewModels(),
                AvailableOwners = await BuildOwnerViewModels(),
                AvailableRatings = await BuildRatingViewModels()
            };
        }
        private async Task<IEnumerable<CategoryViewModel>> BuildCategoryViewModels() {
            var categories = await _categoryRepository.GetAll();
            return categories.Select(c => _categoryBuilder.Build(c)).ToList();
        }

        private async Task<IEnumerable<OwnerViewModel>> BuildOwnerViewModels() {
            var owners = await _userRepository.GetAll();
            return owners.Select(o => _ownerBuilder.Build(o)).ToList();
        }

        private async Task<IEnumerable<RatingViewModel>> BuildRatingViewModels() {
            var ratings = await _ratingRepository.GetAll();
            return ratings.Select(r => _ratingBuilder.Build(r)).ToList();
        }


        public async Task<GameSaveViewModel> BuildViewModel(int id) {
            var viewModel = await BuildViewModel();
            var game = await _gameRepository.GetBy(id);
            if (game == null)
                return viewModel;

            viewModel.Game = _gameBuilder.Build(game);
            viewModel.CategoryIds = BuildCategoryIds(game.GameCategories);
            viewModel.OwnerIds = BuildOwnerIds(game.GameOwners);
            viewModel.RatingIds = BuildRatingIds(game.GamePlayerRatings);
            return viewModel;
        }

        private static int[] BuildCategoryIds(IEnumerable<GameCategory> gameCategories) {
            return gameCategories.Select(gc => gc.CategoryId).ToArray();
        }

        private static string[] BuildOwnerIds(IEnumerable<GameOwner> gameOwners) {
            return gameOwners.Select(go => go.OwnerId).ToArray();
        }

        private static int[] BuildRatingIds(IEnumerable<GamePlayerRating> gamePlayerRatings) {
            return gamePlayerRatings.Select(gc => gc.RatingId).ToArray();
        }
    }
}