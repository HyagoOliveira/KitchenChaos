using UnityEngine;

namespace KitchenChaos.Players
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Player))]
    public sealed class PlayerInputHandler : MonoBehaviour
    {
        [SerializeField] private Player player;
        [SerializeField] private PlayerInputSettings settings;

        private void Reset() => player = GetComponent<Player>();

        private void OnEnable()
        {
            UnBind();
            Bind();
        }

        private void OnDisable() => UnBind();

        private void Bind()
        {
            settings.OnMove += player.Motor.Move;
            //settings.OnDash += player.Motor.Dash;
            settings.OnCollectItem += player.Interactor.TryInteractWithItem;
            settings.OnInteractWithEnvironment += player.Interactor.TryInteractWithEnvironment;
        }

        private void UnBind()
        {
            settings.OnMove -= player.Motor.Move;
            //settings.OnDash -= player.Motor.Dash;
            settings.OnCollectItem -= player.Interactor.TryInteractWithItem;
            settings.OnInteractWithEnvironment -= player.Interactor.TryInteractWithEnvironment;
        }
    }
}