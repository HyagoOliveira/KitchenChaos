using TMPro;
using UnityEngine;
using KitchenChaos.Levels;

namespace KitchenChaos.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Animation))]
    [RequireComponent(typeof(AudioSource))]
    public sealed class InitialCountDown : MonoBehaviour
    {
        [SerializeField] private LevelSettings levelSettings;
        [SerializeField] private Animation animation;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private TMP_Text countDown;
        [SerializeField] private string countDownFormat = "D2";
        [SerializeField] private AudioClip finishedSound;

        private void Reset()
        {
            animation = GetComponent<Animation>();
            audioSource = GetComponent<AudioSource>();
            countDown = GetComponentInChildren<TMP_Text>();
        }

        private void OnEnable()
        {
            levelSettings.InitialCountDown.OnStarted += HandleInitialCoundDownStarted;
            levelSettings.InitialCountDown.OnUpdated += HandleInitialCoundDownUpdated;
            levelSettings.InitialCountDown.OnFinished += HandleInitialCoundDownFinished;
        }

        private void OnDisable()
        {
            levelSettings.InitialCountDown.OnStarted -= HandleInitialCoundDownStarted;
            levelSettings.InitialCountDown.OnUpdated -= HandleInitialCoundDownUpdated;
            levelSettings.InitialCountDown.OnFinished -= HandleInitialCoundDownFinished;
        }

        private void HandleInitialCoundDownStarted() => countDown.gameObject.SetActive(true);

        private void HandleInitialCoundDownUpdated(uint time)
        {
            animation.Play();
            audioSource.Play();
            countDown.text = time.ToString(countDownFormat);
        }

        private void HandleInitialCoundDownFinished()
        {
            audioSource.PlayOneShot(finishedSound);
            countDown.gameObject.SetActive(false);
        }
    }
}