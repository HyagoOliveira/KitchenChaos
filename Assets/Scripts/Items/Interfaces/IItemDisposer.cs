namespace KitchenChaos.Items
{
    /// <summary>
    /// Interface used on objects able to dispose Items.
    /// </summary>
    public interface IItemDisposer : IEnable
    {
        void Dispose(IItemCollectable item);
    }
}