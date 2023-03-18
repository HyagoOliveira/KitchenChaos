using System;
using UnityEngine;
using KitchenChaos.Levels;
using System.Collections;

namespace KitchenChaos.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Animation))]
    [RequireComponent(typeof(AudioSource))]
    public sealed class TimesUp : MonoBehaviour
    {
        [SerializeField] private MatchSettings matchSettings;
        [SerializeField] private Animation animation;
        [SerializeField] private AudioSource audioSource;

        public event Action OnAnimationFinished;

        private void Reset()
        {
            animation = GetComponent<Animation>();
            audioSource = GetComponent<AudioSource>();
        }

        private void OnEnable() => matchSettings.OnFinished += HandleMatchFinished;
        private void OnDisable() => matchSettings.OnFinished -= HandleMatchFinished;

        private void HandleMatchFinished() => StartCoroutine(PlayAnimationAndInvokeActionRoutine());

        private IEnumerator PlayAnimationAndInvokeActionRoutine()
        {
            audioSource.Play();
            yield return animation.PlayAndWait();
            OnAnimationFinished?.Invoke();
        }
    }
}