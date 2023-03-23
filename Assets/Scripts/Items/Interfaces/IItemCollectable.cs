using UnityEngine;

namespace KitchenChaos.Items
{
    /// <summary>
    /// Interface used on objects able to be a Collectable Item.
    /// </summary>
    public interface IItemCollectable : IEnable
    {
        ICollectable GetCollectible();
        IHighlightable GetHighlightable();

        void SetPosition(Vector3 position);
        void Destroy();
    }
}