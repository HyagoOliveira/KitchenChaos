using UnityEngine;

namespace KitchenChaos.Players
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Animator))]
    public sealed class PlayerAnimator : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        private const string isWalking = "IsWalking";

        private void Reset() => animator = GetComponent<Animator>();

        public void SetIsWalking(bool value) => animator.SetBool(isWalking, value);
    }
}