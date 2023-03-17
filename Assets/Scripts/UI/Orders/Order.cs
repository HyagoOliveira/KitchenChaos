using System;
using System.Collections;
using KitchenChaos.Items;
using KitchenChaos.Recipes;
using UnityEngine;
using UnityEngine.UI;

namespace KitchenChaos.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Animation))]
    [RequireComponent(typeof(RectTransform))]
    public sealed class Order : MonoBehaviour
    {
        [SerializeField] private Slider time;
        [SerializeField] private Animation animation;
        [SerializeField] private IngredientIcon iconPrefab;
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private HorizontalLayoutGroup iconsGroup;

        public event Action<Order> OnFailed;
        public event Action<Order> OnDestroyed;
        public event Action<Order> OnDelivered;

        public float CurrentTime
        {
            get => time.value;
            set => time.value = value;
        }

        private RecipeData recipe;
        private Coroutine countDownRoutine;

        private void Reset()
        {
            animation = GetComponent<Animation>();
            time = GetComponentInChildren<Slider>();
            rectTransform = GetComponent<RectTransform>();
            iconsGroup = GetComponentInChildren<HorizontalLayoutGroup>();
        }

        public void SetRecipe(RecipeData recipe)
        {
            this.recipe = recipe;
            SetIcons(recipe.PlatedIngredients);
            SetWidth(recipe.PlatedIngredients.Length);
        }

        public void SetInitialTime(float initialTime)
        {
            time.minValue = 0f;
            time.maxValue = initialTime;
            CurrentTime = initialTime;
        }

        public void StartCountDown() =>
            countDownRoutine = StartCoroutine(CountDownRotine());

        public bool TryDelivery(Plate plate)
        {
            var hasAllIngredients = recipe.ContainsAll(plate.Ingredients);
            if (hasAllIngredients) Delivery();
            return hasAllIngredients;
        }

        private void Delivery()
        {
            StopCoroutine(countDownRoutine);
            StartCoroutine(DeliveryRotine());
        }

        private void SetWidth(int ingredientsCount)
        {
            const float widthPerIngredient = 80f;
            const float spacePerIngredient = 40f;

            var size = rectTransform.sizeDelta;

            size.x = ingredientsCount * (widthPerIngredient + spacePerIngredient);

            rectTransform.sizeDelta = size;
        }

        private void SetIcons(PlatedIngredient[] ingredients)
        {
            foreach (var ingredient in ingredients)
            {
                AddIcon(ingredient.Data.Icon);
            }
        }

        private void AddIcon(Sprite icon)
        {
            var instance = Instantiate(iconPrefab, parent: iconsGroup.transform);
            instance.Icon = icon;
        }

        public IEnumerator CountDownRotine()
        {
            while (CurrentTime > 0F)
            {
                yield return null;
                CurrentTime -= Time.deltaTime;
            }

            CurrentTime = 0F;
            yield return FailedRotine();
        }

        public IEnumerator FailedRotine()
        {
            OnFailed?.Invoke(this);

            yield return PlayFailAnimationAndWait();

            OnDestroyed?.Invoke(this);
            Destroy(gameObject);
        }

        public IEnumerator DeliveryRotine()
        {
            OnDelivered?.Invoke(this);

            yield return PlayDeliveryAnimationAndWait();
            Destroy(gameObject);
        }

        private IEnumerator PlayFailAnimationAndWait() => WaitAnimation(animation, "Order@Fail");
        private IEnumerator PlayDeliveryAnimationAndWait() => WaitAnimation(animation, "Order@Delivered");

        private void HandleTimeUpdated(float time) => CurrentTime = time;

        private static IEnumerator WaitAnimation(Animation animation, string animationName)
        {
            animation.Play(animationName);
            while (animation.isPlaying) yield return null;
        }
    }
}