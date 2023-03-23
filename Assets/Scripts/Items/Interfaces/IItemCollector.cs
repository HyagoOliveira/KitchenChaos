namespace KitchenChaos.Items
{
    /// <summary>
    /// Interface used on objects able to collect Items.
    /// </summary>
    public interface IItemCollector : IEnable
    {
        bool TryCollectItem(out IItemCollectable item);
    }
}