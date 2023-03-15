using System;
using UnityEngine;
using ActionCode.UI;

namespace KitchenChaos.UI
{
    [DisallowMultipleComponent]
    public sealed class PauseOptions : CanvasViewer
    {
        [Header("Buttons")]
        [SerializeField] private DelayedButton resumeButton;
        [SerializeField] private DelayedButton restartButton;
        [SerializeField] private DelayedButton soundsButton;
        [SerializeField] private DelayedButton exitButton;

        public event Action OnResumeClicked;
        public event Action OnRestartClicked;
        public event Action OnSoundClicked;
        public event Action OnExitClicked;

        private void OnEnable()
        {
            resumeButton.onClick.AddListener(Resume);
            restartButton.onClick.AddListener(Restart);
            soundsButton.onClick.AddListener(Sound);
            exitButton.onClick.AddListener(Exit);
        }

        private void OnDisable()
        {
            resumeButton.onClick.RemoveListener(Resume);
            restartButton.onClick.RemoveListener(Restart);
            soundsButton.onClick.RemoveListener(Sound);
            exitButton.onClick.RemoveListener(Exit);
        }

        private void Resume() => OnResumeClicked?.Invoke();
        private void Restart() => OnRestartClicked?.Invoke();
        private void Sound() => OnSoundClicked?.Invoke();
        private void Exit() => OnExitClicked?.Invoke();
    }
}