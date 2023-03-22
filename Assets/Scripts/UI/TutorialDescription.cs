using TMPro;
using UnityEngine;
using System.Collections;

namespace KitchenChaos.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(CanvasGroup))]
    public sealed class TutorialDescription : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;
        [SerializeField] private CanvasGroup group;
        [SerializeField] private AudioSource audioSource;
        [SerializeField, Min(0f)] private float fadeSpeed = 1.2f;

        [Header("Colors")]
        [SerializeField] private Color uncompletedColor;
        [SerializeField] private Color completedColor = Color.green;

        private void Reset()
        {
            group = GetComponent<CanvasGroup>();
            text = GetComponentInChildren<TMP_Text>();
            audioSource = GetComponent<AudioSource>();

            uncompletedColor = text.color;
        }

        public void SetUncompletedText(string value)
        {
            text.text = value;
            text.color = uncompletedColor;
            Enable();
        }

        public void Complete()
        {
            text.color = completedColor;
            audioSource.Play();
        }

        public IEnumerator FadeOutRoutine()
        {
            Enable();

            do
            {
                yield return null;
                group.alpha -= fadeSpeed * Time.deltaTime;

            } while (group.alpha > 0);

            Disable();
        }

        private void Enable() => group.alpha = 1F;
        private void Disable() => group.alpha = 0F;
    }
}