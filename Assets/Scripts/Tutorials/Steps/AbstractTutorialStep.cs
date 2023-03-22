using KitchenChaos.Players;
using UnityEngine;

namespace KitchenChaos.Tutorials
{
    public abstract class AbstractTutorialStep : MonoBehaviour
    {
        protected Player CurrentPlayer => manager.PlayerSettings.Current;
        protected PlayerInputSettings PlayerInput => manager.PlayerInputSettings;

        protected TutorialManager manager;

        internal void Initialize(TutorialManager manager) => this.manager = manager;

        private void Start()
        {
        }

        internal abstract void Begin();

        protected void Complete() => manager.CompleteStep();

        internal abstract string GetDescription();
    }
}