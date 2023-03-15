using UnityEngine;
using System.Collections.Generic;

namespace KitchenChaos.Items
{
    [CreateAssetMenu(fileName = "IngredientSettings", menuName = EditorPaths.INGREDIENTS + "Settings", order = 110)]
    public sealed class IngredientSettings : ScriptableObject
    {
        [SerializeField] private IngredientDataCollection[] collections;

        private Dictionary<IngredientName, IngredientDataCollection> collectionDictionary;

        internal void InitializeDictionary()
        {
            collectionDictionary = new(collections.Length);

            foreach (var container in collections)
            {
                container.InitializeDictionary();
                collectionDictionary.Add(container.Name, container);
            }
        }

        public Ingredient SpawnIngredient(IngredientName name, IngredientStatus status)
        {
            var hasCollection = TryGetIngredientCollection(name, out IngredientDataCollection collection);
            if (!hasCollection)
            {
                Debug.LogError($"Ingredient {name} is not present in any collection.");
                return null;
            }

            var hasData = collection.TryGetIngredientData(status, out IngredientData data);
            if (!hasData)
            {
                Debug.LogError($"Ingredient Status {status} is not present into {collection}");
                return null;
            }

            return data.Spawn();
        }

        public bool TryGetIngredientCollection(IngredientName name, out IngredientDataCollection container) =>
            collectionDictionary.TryGetValue(name, out container);

        public float GetPreparationTime(IngredientData data)
        {
            if (data.Status == IngredientStatus.Ready)
            {
                var previousData = collectionDictionary[data.Name].GetPreviousIngredientData(data);
                return previousData.PreparationTime;
            }

            return data.PreparationTime;
        }

        internal IngredientData GetData(Ingredient ingredient)
        {
            foreach (var collection in collections)
            {
                if (collection.Name != ingredient.Name) continue;

                foreach (var data in collection.Ingredients)
                {
                    if (data.Status == ingredient.Status)
                        return data;
                }
            }

            return null;
        }
    }
}