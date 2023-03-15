using System;

namespace KitchenChaos
{
    /// <summary>
    /// Interface used on objects able to be interacted.
    /// </summary>
    public interface IInteractable
    {
        event Action OnInteracted;

        void Interact();
    }
}