namespace KitchenChaos.Items
{
    /// <summary>
    /// Interface used on objects able to transfer Items.
    /// </summary>
    public interface IItemTransfer : IEnable
    {
        bool TryTransferItem(IItemHolder fromHolder);
    }
}