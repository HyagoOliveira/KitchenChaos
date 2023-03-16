using UnityEngine;

namespace KitchenChaos.Levels
{
    [DisallowMultipleComponent]
    [DefaultExecutionOrder(Managers.EXECUTION_ORDER)]
    public sealed class LevelManager : MonoBehaviour
    {
        [SerializeField] private LevelSettings settings;

        private void Awake() => settings.Initialize();
        private void Start() => settings.StartInitialCountDown();

        private void OnEnable()
        {
            settings.InitialCountDown.OnFinished += HandleInitialCountDownFinished;
        }

        private void OnDisable()
        {
            settings.InitialCountDown.OnFinished -= HandleInitialCountDownFinished;
        }

        private void HandleInitialCountDownFinished() => settings.StartTimeLimit();
    }
}