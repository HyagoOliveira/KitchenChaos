using UnityEngine;
using KitchenChaos.Matches;
using System.Collections;
using KitchenChaos.Scenes;

namespace KitchenChaos.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Animation))]
    [RequireComponent(typeof(AudioSource))]
    public sealed class TimesUp : MonoBehaviour
    {
        [SerializeField] private MatchSettings matchSettings;
        [SerializeField] private SceneSettings sceneSettings;
        [SerializeField] private Animation animation;
        [SerializeField] private AudioSource audioSource;

        private void Reset()
        {
            animation = GetComponent<Animation>();
            audioSource = GetComponent<AudioSource>();
        }

        private void OnEnable() => matchSettings.TimeLimit.OnFinished += HandleMatchFinished;
        private void OnDisable() => matchSettings.TimeLimit.OnFinished -= HandleMatchFinished;

        private void HandleMatchFinished() => StartCoroutine(PlayAnimationAndInvokeActionRoutine());

        private IEnumerator PlayAnimationAndInvokeActionRoutine()
        {
            audioSource.Play();
            yield return animation.PlayAndWait();
            sceneSettings.GoToResults();
        }
    }
}