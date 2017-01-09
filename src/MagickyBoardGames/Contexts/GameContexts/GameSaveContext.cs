using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using MagickyBoardGames.Builders;
using MagickyBoardGames.Models;
using MagickyBoardGames.Repositories;
using MagickyBoardGames.ViewModels;

namespace MagickyBoardGames.Contexts.GameContexts {
    public class GameSaveContext : IGameSaveContext {
        private readonly IGameRepository _repository;
        private readonly IBuilder<Game, GameViewModel> _builder;
        private readonly IValidator<GameViewModel> _validator;

        public GameSaveContext(IGameRepository repository, IBuilder<Game, GameViewModel> builder, IValidator<GameViewModel> validator) {
            _repository = repository;
            _builder = builder;
            _validator = validator;
        }

        public ValidationResult Validate(GameViewModel viewModel) {
            return _validator.Validate(viewModel);
        }

        public async Task Save(GameViewModel viewModel) {
            var entity = _builder.Build(viewModel);
            if (viewModel.Id.HasValue)
                await Save(await _repository.GetBy(viewModel.Id.Value), entity);
            else
                await Save(await _repository.GetBy(viewModel.Name), entity);
        }

        private async Task Save(Game found, Game entity) {
            if (found != null)
                await Update(found, entity);
            else
                await Insert(entity);
        }

        private async Task Update(Game found, Game entity) {
            found.Description = entity.Description;
            await _repository.Update(found);
        }

        private async Task Insert(Game entity) {
            await _repository.Add(entity);
        }
    }
}