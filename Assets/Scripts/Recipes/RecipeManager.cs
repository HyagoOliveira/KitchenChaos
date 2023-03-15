using UnityEngine;

namespace KitchenChaos.Recipes
{
    [DisallowMultipleComponent]
    public sealed class RecipeManager : MonoBehaviour
    {
        [SerializeField] private RecipeSettings settings;

        private void Awake() => settings.Initialize();
    }
}