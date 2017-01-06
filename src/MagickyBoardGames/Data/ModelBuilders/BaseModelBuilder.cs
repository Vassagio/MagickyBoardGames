using Microsoft.EntityFrameworkCore;

namespace MagickyBoardGames.Data.ModelBuilders
{
    public abstract class BaseModelBuilder
    {
        protected readonly ModelBuilder Builder;

        protected BaseModelBuilder(ModelBuilder builder) {
            Builder = builder;
        }
        public abstract void Build();
    }
}
