using UnityEngine;

namespace KitchenChaos
{
    /// <summary>
    /// Interface used on objects able to be picked up.
    /// </summary>
    public interface IPickable
    {
        void PickUp(Transform picker);
    }
}