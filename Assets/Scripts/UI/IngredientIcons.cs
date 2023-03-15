using UnityEngine;
using System.Collections.Generic;

namespace KitchenChaos.UI
{
    [DisallowMultipleComponent]
    public sealed class IngredientIcons : MonoBehaviour
    {
        [SerializeField] private IngredientIcon iconPrefab;

        private readonly List<IngredientIcon> icons = new(4);

        public void Add(Sprite icon)
        {
            var instance = Instantiate(iconPrefab);

            instance.Icon = icon;
            instance.transform.SetParent(transform);
            instance.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

            icons.Add(instance);
        }

        public void Remove(Sprite sprite)
        {
            var icon = icons.FindLast(i => i.Icon == sprite);
            Destroy(icon.gameObject);
            icons.Remove(icon);
        }

        public void Clear()
        {
            foreach (var icon in icons)
            {
                Destroy(icon.gameObject);
            }
            icons.Clear();
        }
    }
}