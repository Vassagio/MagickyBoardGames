namespace MagickyBoardGames.Contexts.CategoryContexts
{
    public interface ICategoryContextLoader
    {
        ICategoryListContext LoadCategoryListContext();
        ICategoryViewContext LoadCategoryViewContext();
        ICategorySaveContext LoadCategorySaveContext();
    }
}
