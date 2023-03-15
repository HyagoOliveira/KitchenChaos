using UnityEngine;
using UnityEngine.UI;

namespace KitchenChaos.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Slider))]
    [RequireComponent(typeof(Canvas))]
    public sealed class PreparationTimer : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private Canvas canvas;

        public float Progress
        {
            get => slider.value;
            set => slider.value = value;
        }

        private void Reset()
        {
            slider = GetComponent<Slider>();
            canvas = GetComponent<Canvas>();
        }

        public void Show() => canvas.enabled = true;
        public void Hide() => canvas.enabled = false;
    }
}