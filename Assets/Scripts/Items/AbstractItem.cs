using UnityEngine;
using KitchenChaos.Physics;
using ActionCode.VisualEffects;

namespace KitchenChaos.Items
{
    /// <summary>
    /// Component representing an abstract Item able to be highlighted and collected.
    /// </summary>
    [RequireComponent(typeof(CollectableBody))]
    [RequireComponent(typeof(HighlighterContainer))]
    public abstract class AbstractItem : MonoBehaviour, IItemCollectable
    {
        [SerializeField] private CollectableBody collectibleBody;
        [SerializeField] private HighlighterContainer highlighterContainer;

        public bool IsEnabled
        {
            get => enabled;
            set => enabled = value;
        }

        protected virtual void Reset()
        {
            collectibleBody = GetComponent<CollectableBody>();
            highlighterContainer = GetComponent<HighlighterContainer>();
        }

        public ICollectable GetCollectible() => collectibleBody;
        public IHighlightable GetHighlightable() => highlighterContainer;

        public void SetPosition(Vector3 position) => transform.position = position;
        public void Destroy() => Destroy(gameObject);
    }
}