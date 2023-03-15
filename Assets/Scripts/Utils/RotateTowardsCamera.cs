using UnityEngine;

namespace KitchenChaos
{
    [DisallowMultipleComponent]
    public sealed class RotateTowardsCamera : MonoBehaviour
    {
        private Transform cameraTransform;

        private void Start() => cameraTransform = Camera.main.transform;

        private void Update()
        {
            var invertedPosition = transform.position - cameraTransform.position;
            transform.LookAt(invertedPosition);
        }
    }
}