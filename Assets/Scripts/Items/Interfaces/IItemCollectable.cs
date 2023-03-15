namespace KitchenChaos.Items
{
    /// <summary>
    /// Interface used on objects able to be a Collectable Item.
    /// </summary>
    public interface IItemCollectable
    {
        ICollectable GetCollectible();
        IHighlightable GetHighlightable();

        void Destroy();
    }
}