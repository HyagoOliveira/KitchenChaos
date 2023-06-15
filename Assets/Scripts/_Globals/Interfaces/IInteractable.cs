using System;
using ActionCode.Physics;

namespace KitchenChaos
{
    /// <summary>
    /// Interface used on objects able to be interacted.
    /// </summary>
    public interface IInteractable : IEnable
    {
        event Action OnInteracted;

        void Interact();
    }
}