using TMPro;
using UnityEngine;

namespace KitchenChaos.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Animation))]
    [RequireComponent(typeof(AudioSource))]
    public sealed class DeliveryCounterCanvas : MonoBehaviour
    {
        [SerializeField] private Animation animation;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private TMP_Text tip;
        [SerializeField] private string tipFormat = "+{0:D2} Tip!";

        private void Reset()
        {
            animation = GetComponent<Animation>();
            audioSource = GetComponent<AudioSource>();
        }

        public void ShowTip(int value)
        {
            tip.text = string.Format(tipFormat, value);
            PlayOnlyShowTipAnimation();
        }

        public void ShowNeedPlate()
        {
            audioSource.Play();
            PlayOnlyShowNeedPlateAnimation();
        }

        private void PlayOnlyShowNeedPlateAnimation()
        {
            animation.Stop();
            animation.Play("DeliveryCounterCanvas@ShowNeedPlateText");
        }

        private void PlayOnlyShowTipAnimation()
        {
            animation.Stop();
            animation.Play("DeliveryCounterCanvas@ShowTipText");
        }
    }
}