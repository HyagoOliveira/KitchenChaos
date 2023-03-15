namespace KitchenChaos.Items
{
    /// <summary>
    /// Interface used on objects able to transfer Items.
    /// </summary>
    public interface IItemTransfer
    {
        bool TryTransferItem(IItemHolder fromHolder);
    }
}