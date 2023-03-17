using UnityEngine;

namespace KitchenChaos
{
    [DisallowMultipleComponent]
    public sealed class RotateTowardsBack : MonoBehaviour
    {
        private void Start() => Rotate();

        [ContextMenu("Rotate")]
        private void Rotate()
        {
            var forward = transform.forward;
            var forwardDot = Vector3.Dot(forward, Vector3.forward);
            var isFacingBackward = Mathf.Approximately(forwardDot, -1F);
            if (isFacingBackward) return;

            var isFacingForward = Mathf.Approximately(forwardDot, 1F);
            var angle = isFacingForward ?
                180F :
                Mathf.Sign(forward.x) * 90F;

            transform.Rotate(Vector3.up * angle);
        }
    }
}