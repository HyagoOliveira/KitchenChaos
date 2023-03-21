using UnityEngine;
using ActionCode.Audio;

namespace KitchenChaos.Players
{
    /// <summary>
    /// Animation Events will call functions in this component.
    /// This component must be attached to the same GameObject where the Animator component is.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(AudioSourceDictionary))]
    public sealed class PlayerAnimationEventHandler : MonoBehaviour
    {
        [SerializeField] private AudioSourceDictionary footsteps;

        private void Reset() => footsteps = GetComponent<AudioSourceDictionary>();

        public void PlayFootstepSound() => footsteps.PlayRandom();
    }
}