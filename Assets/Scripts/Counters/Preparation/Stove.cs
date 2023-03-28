using UnityEngine;
using KitchenChaos.Items;
using System.Collections;

namespace KitchenChaos.Counters
{
    [DisallowMultipleComponent]
    public sealed class Stove : AbstractIngredientPreparator
    {
        [SerializeField] private FryingPan fryingPan;
        [SerializeField] private PreparationWarning warning;
        [SerializeField] private float timeBeforeBurn = 2f;

        public bool IsBurning { get; private set; }

        public bool IsAbleToBurn { get; set; } = true;

        private Coroutine burningRoutine;

        private const string burningAnimationName = "Stove@Burning";
        private const string waitBurningAnimationName = "Stove@WaitBurning";

        protected override void Reset()
        {
            base.Reset();
            fryingPan = GetComponentInChildren<FryingPan>();
            warning = GetComponentInChildren<PreparationWarning>();
        }

        protected override void Awake()
        {
            base.Awake();
            warning.Hide();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            holder.OnItemReleased += HandleOnItemReleased;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            holder.OnItemReleased -= HandleOnItemReleased;
        }

        protected override void SetupIngredientStatus()
        {
            allowedStatus = IngredientStatus.Raw;
            preparatedStatus = IngredientStatus.Ready;
        }

        public override void Interact()
        {
            if (IsBurning) StopBurning();
            else base.Interact();
        }

        protected override void PlayPreparationAnimation()
        {
            base.PlayPreparationAnimation();
            fryingPan.StartFry();
        }

        protected override void PlayIdleAnimation()
        {
            base.PlayIdleAnimation();
            fryingPan.StopFry();
        }

        protected override void CompletePreparation()
        {
            base.CompletePreparation();
            burningRoutine = StartCoroutine(BurningRoutine());
        }

        protected override string GetIdleAnimationName() => "Stove@Idle";
        protected override string GetPreparationAnimationName() => "Stove@Heatting";

        private IEnumerator BurningRoutine()
        {
            if (!IsAbleToBurn) yield break;

            IsBurning = true;
            PlayWaitBuningAnimation();

            yield return new WaitForSeconds(timeBeforeBurn);

            PlayBuningAnimation();

            yield return ToggleWarningRotine(0.8F, 0.2F, 3);
            SetBuningAnimationSpeed(2f);
            yield return ToggleWarningRotine(0.2F, 0.2F, 5);

            BurnIngredient();
            CompleteBurning();
            fryingPan.StartBurn();
        }

        private IEnumerator ToggleWarningRotine(float showTime, float hideTime, int times)
        {
            var waitShowTime = new WaitForSeconds(showTime);
            var hideShowTime = new WaitForSeconds(hideTime);

            for (int i = 0; i < times; i++)
            {
                warning.Show();
                yield return waitShowTime;
                warning.Hide();
                yield return hideShowTime;
            }
        }

        private void PlayBuningAnimation() => animation.Play(burningAnimationName);
        private void PlayWaitBuningAnimation() => animation.Play(waitBurningAnimationName);
        private void SetBuningAnimationSpeed(float speed) => animation[burningAnimationName].speed = speed;

        private void BurnIngredient()
        {
            var ingredient = holder.CurrentItem as Ingredient;
            var burnedIngredient = ingredientSettings.SpawnIngredient(ingredient.Name, IngredientStatus.Burned);

            holder.ReplaceItem(burnedIngredient);
        }

        private void StopBurning()
        {
            StopCoroutine(burningRoutine);
            CompleteBurning();
        }

        private void CompleteBurning()
        {
            IsBurning = false;
            warning.Hide();
            PlayIdleAnimation();
            SetBuningAnimationSpeed(1F);
        }

        private void HandleOnItemReleased(IItemCollectable _)
        {
            if (IsBurning) StopBurning();
            fryingPan.StopBurn();
        }
    }
}