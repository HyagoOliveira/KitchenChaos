using UnityEngine;
using KitchenChaos.VisualEffects;

namespace KitchenChaos.Counters
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(MaterialHighlighter))]
    public sealed class FryingPan : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private ParticleSystem sizzlingParticles;

        private void Reset()
        {
            audioSource = GetComponent<AudioSource>();
            sizzlingParticles = GetComponentInChildren<ParticleSystem>();
        }

        public void StartFry()
        {
            audioSource.Play();
            sizzlingParticles.Play();
        }

        public void StopFry()
        {
            audioSource.Stop();
            sizzlingParticles.Stop();
        }
    }
}