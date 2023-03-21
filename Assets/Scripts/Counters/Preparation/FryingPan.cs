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
        [SerializeField] private ParticleSystem burnedParticles;

        private void Reset() => audioSource = GetComponent<AudioSource>();

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

        public void StartBurn() => burnedParticles.Play();
        public void StopBurn() => burnedParticles.Stop();
    }
}