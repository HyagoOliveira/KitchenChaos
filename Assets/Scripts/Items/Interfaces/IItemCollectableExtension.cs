namespace KitchenChaos.Items
{
    public static class IItemCollectableExtension
    {
        public static bool IsIngredient(this IItemCollectable item, IngredientName name) =>
            item is Ingredient ingredient && ingredient.Name == name;

        public static bool IsIngredient(this IItemCollectable item, IngredientName name, IngredientStatus status) =>
            item is Ingredient ingredient && ingredient.Name == name && ingredient.Status == status;
    }
}