using UnityEngine;
using ActionCode.Audio;

namespace KitchenChaos.Counters
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(AudioSourceDictionary))]
    public sealed class CounterKnife : MonoBehaviour
    {
        [SerializeField] private AudioSourceDictionary audioDictionary;

        private void Reset()
        {
            audioDictionary = GetComponent<AudioSourceDictionary>();
        }

        public void Show() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);

        public void PlayCutSound() => audioDictionary.PlayRandom();
    }
}