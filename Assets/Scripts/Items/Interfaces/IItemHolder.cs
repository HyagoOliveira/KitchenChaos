namespace KitchenChaos.Items
{
    /// <summary>
    /// Interface used on objects able to hold Items. 
    /// </summary>
    public interface IItemHolder : IItemTransfer
    {
        IItemCollectable CurrentItem { get; }

        void ReleaseItem();
    }
}