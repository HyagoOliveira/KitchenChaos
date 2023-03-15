using UnityEngine;
using ActionCode.Audio;
using KitchenChaos.Items;
using System.Collections;
using KitchenChaos.VisualEffects;

namespace KitchenChaos.Counters
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Animation))]
    [RequireComponent(typeof(HighlighterContainer))]
    [RequireComponent(typeof(AudioSourceDictionary))]
    public sealed class Trash : MonoBehaviour, IItemDisposer, IItemTransfer
    {
        [SerializeField] private ItemHolder holder;
        [SerializeField] private Animation animation;
        [SerializeField] private AudioSourceDictionary sourceDictionary;

        private void Reset()
        {
            animation = GetComponent<Animation>();
            holder = GetComponentInChildren<ItemHolder>();
            sourceDictionary = GetComponent<AudioSourceDictionary>();
        }

        public bool TryTransferItem(IItemHolder fromHolder)
        {
            if (fromHolder.CurrentItem is Ingredient ingredient)
            {
                fromHolder.ReleaseItem();
                Dispose(ingredient);
                return true;
            }

            return false;
        }

        public void Dispose(IItemCollectable item)
        {
            holder.PlaceItem(item);
            StartCoroutine(PlayAnimationAndDestroyItem());
        }

        private IEnumerator PlayAnimationAndDestroyItem()
        {
            sourceDictionary.PlayRandom();

            animation.Play();
            yield return new WaitWhile(() => animation.isPlaying);
            animation.Stop();

            holder.DestroyItem();
        }
    }
}