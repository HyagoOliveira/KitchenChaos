using UnityEngine;

namespace KitchenChaos.Serialization
{
    [DisallowMultipleComponent]
    [DefaultExecutionOrder(Managers.EXECUTION_ORDER)]
    public sealed class GameDataManager : MonoBehaviour
    {
        [SerializeField] private GameDataSettings settings;

        private static bool wasInitialized;

        private void Start()
        {
            if (wasInitialized)
            {
                DestroyImmediate(gameObject);
                return;
            }

            settings.LoadOrCreate();
            wasInitialized = true;

            DontDestroyOnLoad(gameObject);
        }
    }
}