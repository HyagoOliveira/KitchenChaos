using UnityEngine;

namespace KitchenChaos.Physics
{
    /// <summary>
    /// Component representing a Rigidbody able to be collected.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody))]
    public sealed class CollectableBody : MonoBehaviour, ICollectable
    {
        [SerializeField] private Rigidbody body;

        private void Reset() => body = GetComponent<Rigidbody>();

        public void PickUp(Transform holder)
        {
            EnableRigidbody(false);

            transform.SetParent(holder);
            transform.SetPositionAndRotation(
                holder.position,
                holder.rotation
            );
        }

        public void Drop()
        {
            transform.SetParent(null);
            EnableRigidbody(true);
        }

        private void EnableRigidbody(bool enabled)
        {
            body.isKinematic = !enabled;
            body.detectCollisions = enabled;
        }
    }
}