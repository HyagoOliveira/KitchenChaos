using UnityEngine;
using KitchenChaos.Items;

namespace KitchenChaos.Players
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(PlayerMotor))]
    [RequireComponent(typeof(PlayerInputHandler))]
    [RequireComponent(typeof(ItemHandler))]
    public sealed class Player : MonoBehaviour
    {
        [field: SerializeField] public PlayerMotor Motor { get; private set; }
        [field: SerializeField] public PlayerInputHandler Input { get; private set; }
        [field: SerializeField] public ItemHandler Interactor { get; private set; }

        [field: SerializeField] public PlayerType Type { get; private set; }

        [SerializeField] private GameObject interactableCircle;

        private void Reset()
        {
            Motor = GetComponent<PlayerMotor>();
            Input = GetComponent<PlayerInputHandler>();
            Interactor = GetComponent<ItemHandler>();
        }

        public int Index { get; internal set; }

        public void SetActive(bool active)
        {
            interactableCircle.SetActive(active);

            Input.enabled = active;
            Motor.Stop();
        }
    }
}