using UnityEngine;

namespace KitchenChaos.Counters
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(SpriteRenderer))]
    public sealed class PreparationWarning : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private SpriteRenderer spriteRenderer;

        public bool IsVisible => spriteRenderer.enabled;

        private void Reset()
        {
            audioSource = GetComponent<AudioSource>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void Toggle()
        {
            if (IsVisible) Hide();
            else Show();
        }

        public void Show()
        {
            audioSource.Play();
            spriteRenderer.enabled = true;
        }

        public void Hide() => spriteRenderer.enabled = false;
    }
}