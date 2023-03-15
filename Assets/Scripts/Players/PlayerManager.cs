using UnityEngine;
using ActionCode.PauseSystem;

namespace KitchenChaos.Players
{
    [DisallowMultipleComponent]
    public sealed class PlayerManager : MonoBehaviour
    {
        [SerializeField] private PlayerSettings settings;
        [SerializeField] private PauseSettings pauseSettings;
        [SerializeField] private PlayerInputSettings inputSettings;

        private void Awake()
        {
            settings.Initialize();
            inputSettings.Initialize();
        }

        private void Start()
        {
            inputSettings.Enable();
            settings.EnableFirstPlayer();
        }

        private void OnEnable()
        {
            inputSettings.BindActions();

            inputSettings.OnPause += HandlePlayerPause;
            inputSettings.OnSwitch += HandlePlayerSwitch;
            pauseSettings.OnPaused += HandlePaused;
            pauseSettings.OnResumed += HandleResumed;
        }

        private void OnDisable()
        {
            inputSettings.OnPause -= HandlePlayerPause;
            inputSettings.OnSwitch -= HandlePlayerSwitch;
            pauseSettings.OnPaused -= HandlePaused;
            pauseSettings.OnResumed -= HandleResumed;

            inputSettings.UnBindActions();
        }

        private void HandlePlayerSwitch()
        {
            inputSettings.ResetAxis();
            settings.Switch();
        }

        private void HandlePlayerPause() => pauseSettings.Pause();

        private void HandlePaused() => inputSettings.Disable();
        private void HandleResumed() => inputSettings.Enable();
    }
}