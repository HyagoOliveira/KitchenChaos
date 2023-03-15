using UnityEngine;
using KitchenChaos.Physics;

namespace KitchenChaos.VisualEffects
{
    /// <summary>
    /// Detector for <see cref="IHighlightable"/> implementations.
    /// <para>
    /// Uses <see cref="BoxDetector"/> to find any component implementing the <see cref="IHighlightable"/> interface.<br/>
    /// UnHighlights the components when exiting the Cast.
    /// </para>
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(BoxDetector))]
    public sealed class HighlightableDetector : MonoBehaviour
    {
        [SerializeField] private BoxDetector detector;

        private IHighlightable lastHighlightable;

        private void Reset() => detector = GetComponent<BoxDetector>();
        private void OnEnable() => detector.OnHitChanged += HandleHitChanged;
        private void OnDisable() => detector.OnHitChanged -= HandleHitChanged;

        private void HandleHitChanged(RaycastHit hit)
        {
            lastHighlightable?.UnHighlight();

            if (hit.transform == null) return;

            var hasHighlightable = hit.transform.TryGetComponent(out IHighlightable highlightable);
            if (!hasHighlightable) return;

            highlightable.Highlight();
            lastHighlightable = highlightable;
        }
    }
}