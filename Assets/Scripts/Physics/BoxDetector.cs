using System;
using UnityEngine;
using ActionCode.Shapes;
using UnityPhysics = UnityEngine.Physics;

namespace KitchenChaos.Physics
{
    /// <summary>
    /// Casts a Box, filtering by the <see cref="layers"/>.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class BoxDetector : MonoBehaviour
    {
        [SerializeField] private float distance = 0.8F;
        [SerializeField] private LayerMask layers;
        [SerializeField] private Vector3 size = Vector3.one * 0.5F;

        public event Action<RaycastHit> OnHitChanged;

        private bool hasHit;
        private RaycastHit lastHit;
        private RaycastHit currentHit;

        private void Update() => UpdateCast();
        private void OnDrawGizmosSelected() => DrawCast();

        public bool HasHit() => hasHit;

        public bool TryGetHittingComponent<T>(out T component) where T : IEnable
        {
            if (HasHit() && currentHit.transform.TryGetComponent(out component))
                return component.IsEnabled;

            component = default;
            return false;
        }

        private void UpdateCast()
        {
            lastHit = currentHit;
            hasHit = UnityPhysics.BoxCast(
                GetCastOrigin(),
                halfExtents: size * 0.5F,
                GetCastDirection(),
                out currentHit,
                GetCastOrientation(),
                distance,
                layers
            );

            var hasHitChanged = !currentHit.Equals(lastHit);
            if (hasHitChanged) OnHitChanged?.Invoke(currentHit);
        }

        private void DrawCast()
        {
            var origin = GetCastOrigin();
            var end = origin + GetCastDirection() * distance;
            var color = HasHit() ? Color.red : Color.green;

            Debug.DrawLine(origin, end, color);
            ShapeDebug.DrawCuboid(end, size, GetCastOrientation(), color);
        }

        private Vector3 GetCastOrigin() => transform.position;
        private Vector3 GetCastDirection() => transform.forward;
        private Quaternion GetCastOrientation() => transform.rotation;
    }
}