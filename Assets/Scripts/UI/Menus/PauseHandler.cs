using UnityEngine;
using ActionCode.PauseSystem;

namespace KitchenChaos.UI
{
    [DisallowMultipleComponent]
    public sealed class PauseHandler : MonoBehaviour
    {
        [SerializeField] private PauseSettings pauseSettings;
        [SerializeField] private PauseCanvas pauseCanvas;

        private void OnEnable()
        {
            pauseSettings.OnPaused += HandlePaused;
            pauseSettings.OnResumed += HandleResumed;
        }

        private void OnDisable()
        {
            pauseSettings.OnPaused -= HandlePaused;
            pauseSettings.OnResumed -= HandleResumed;
        }

        private void HandlePaused() => pauseCanvas.Show();
        private void HandleResumed() => pauseCanvas.Hide();
    }
}