using UnityEngine;
using ActionCode.Audio;
using KitchenChaos.Matches;

namespace KitchenChaos.Musics
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(AudioSource))]
    public sealed class BackgroundMusic : MonoBehaviour
    {
        [SerializeField] private MatchSettings matchSettings;
        [SerializeField] private AudioSource audioSource;
        [SerializeField, Min(1F)] private float accelerateSpeed = 1.25F;

        private static bool created;

        private void Reset()
        {
            audioSource = GetComponent<AudioSource>();

            audioSource.loop = true;
            audioSource.playOnAwake = true;
            audioSource.SetSpatialBlendTo2D();
        }

        private void Awake()
        {
            if (created)
            {
                DestroyImmediate(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);
            created = true;
        }

        private void OnEnable()
        {
            matchSettings.TimeLimit.OnFinished += HandleTimeLimitFinished;
            matchSettings.OnFinalSecondsStarted += HandleFinalSecondsStarted;
        }

        private void OnDisable()
        {
            matchSettings.TimeLimit.OnFinished -= HandleTimeLimitFinished;
            matchSettings.OnFinalSecondsStarted -= HandleFinalSecondsStarted;
        }

        private void HandleTimeLimitFinished() => NormalizeMusic();

        private void HandleFinalSecondsStarted() => AccelerateMusic();

        private void AccelerateMusic() => audioSource.pitch = accelerateSpeed;
        private void NormalizeMusic() => audioSource.pitch = 1f;
    }
}