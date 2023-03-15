using UnityEngine;

namespace KitchenChaos.Items
{
    [DisallowMultipleComponent]
    public sealed class IngredientManager : MonoBehaviour
    {
        [SerializeField] private IngredientSettings settings;

        private void Awake() => settings.InitializeDictionary();
    }
}