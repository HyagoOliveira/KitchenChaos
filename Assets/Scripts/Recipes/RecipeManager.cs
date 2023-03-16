using UnityEngine;

namespace KitchenChaos.Recipes
{
    [DisallowMultipleComponent]
    [DefaultExecutionOrder(Managers.EXECUTION_ORDER)]
    public sealed class RecipeManager : MonoBehaviour
    {
        [SerializeField] private RecipeSettings settings;

        private void Awake() => settings.Initialize();
    }
}