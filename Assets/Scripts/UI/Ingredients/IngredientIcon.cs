using UnityEngine;
using UnityEngine.UI;

namespace KitchenChaos.UI
{
    [DisallowMultipleComponent]
    public sealed class IngredientIcon : MonoBehaviour
    {
        [SerializeField] private Image icon;

        public Sprite Icon
        {
            get => icon.sprite;
            set => icon.sprite = value;
        }

        private void Reset() => icon = GetComponentInChildren<Image>();
    }
}