using UnityEngine;

namespace KitchenChaos.Items
{
    [DisallowMultipleComponent]
    [DefaultExecutionOrder(Managers.EXECUTION_ORDER)]
    public sealed class IngredientManager : MonoBehaviour
    {
        [SerializeField] private IngredientSettings settings;

        private void Awake() => settings.InitializeDictionary();
    }
}