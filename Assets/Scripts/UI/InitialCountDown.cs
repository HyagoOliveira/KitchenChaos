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
        [SerializeField] private MatchSettings levelSettings;
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
            levelSettings.CountDown.OnStarted += HandlelCountDownStarted;
            levelSettings.CountDown.OnUpdated += HandleCountDownUpdated;
            levelSettings.CountDown.OnFinished += HandleCountDownFinished;
        }

        private void OnDisable()
        {
            levelSettings.CountDown.OnStarted -= HandlelCountDownStarted;
            levelSettings.CountDown.OnUpdated -= HandleCountDownUpdated;
            levelSettings.CountDown.OnFinished -= HandleCountDownFinished;
        }

        private void HandlelCountDownStarted() => countDown.gameObject.SetActive(true);

        private void HandleCountDownUpdated(uint time)
        {
            animation.Play();
            audioSource.Play();
            countDown.text = time.ToString(countDownFormat);
        }

        private void HandleCountDownFinished()
        {
            audioSource.PlayOneShot(finishedSound);
            countDown.gameObject.SetActive(false);
        }
    }
}