using UnityEngine;
using System.Collections;

namespace KitchenChaos
{
    public static class AnimationExtension
    {
        public static IEnumerator PlayAndWait(this Animation animation)
        {
            animation.Play();
            yield return WaitAnimation(animation);
        }

        public static IEnumerator PlayAndWait(this Animation animation, string name)
        {
            animation.Play(name);
            yield return WaitAnimation(animation);
        }

        private static IEnumerator WaitAnimation(Animation animation)
        {
            while (animation.isPlaying) yield return null;
        }
    }
}