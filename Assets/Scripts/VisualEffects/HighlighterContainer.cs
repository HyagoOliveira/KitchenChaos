using UnityEngine;
using System.Collections.Generic;

namespace KitchenChaos.VisualEffects
{
    /// <summary>
    /// Container for implementations of <see cref="IHighlightable"/>.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class HighlighterContainer : MonoBehaviour, IHighlightable
    {
        [SerializeField] private MaterialHighlighter[] materials;

        private bool isHighlighted;
        private List<IHighlightable> highlightables;

        private void Reset() =>
            materials = GetComponentsInChildren<MaterialHighlighter>(includeInactive: true);

        private void Awake() => highlightables = new(materials);

        public void Highlight()
        {
            foreach (var highlightable in highlightables)
            {
                highlightable.Highlight();
            }
            isHighlighted = true;
        }

        public void UnHighlight()
        {
            foreach (var highlightable in highlightables)
            {
                highlightable.UnHighlight();
            }
            isHighlighted = false;
        }

        public void Add(IHighlightable highlightable)
        {
            highlightables.Add(highlightable);
            if (isHighlighted) highlightable.Highlight();
        }

        public void Remove(IHighlightable highlightable)
        {
            highlightables.Remove(highlightable);
            highlightable.UnHighlight();
        }
    }
}