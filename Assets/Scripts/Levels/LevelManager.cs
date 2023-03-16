using UnityEngine;

namespace KitchenChaos.Levels
{
    [DisallowMultipleComponent]
    [DefaultExecutionOrder(Managers.EXECUTION_ORDER)]
    public sealed class LevelManager : MonoBehaviour
    {
        [SerializeField] private LevelSettings settings;

        private void Awake() => settings.Initialize();
        private void Start() => Invoke(nameof(StartInitialCountDown), 0.2F);

        private void OnEnable()
        {
            settings.InitialCountDown.OnFinished += HandleInitialCountDownFinished;
        }

        private void OnDisable()
        {
            settings.InitialCountDown.OnFinished -= HandleInitialCountDownFinished;
        }

        private void StartInitialCountDown() => settings.StartInitialCountDown();

        private void HandleInitialCountDownFinished() => settings.StartTimeLimit();
    }
}