using UnityEngine;
using KitchenChaos.Items;

namespace KitchenChaos.Counters
{
    [DisallowMultipleComponent]
    public sealed class CuttingBoard : AbstractIngredientPreparator
    {
        [SerializeField] private CounterKnife knife;

        protected override void Reset()
        {
            base.Reset();
            knife = GetComponentInChildren<CounterKnife>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            holder.OnItemPlaced += HandleItemPlaced;
            holder.OnItemReleased += HandleItemReleased;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            holder.OnItemPlaced -= HandleItemPlaced;
            holder.OnItemReleased -= HandleItemReleased;
        }

        public void PlayCutSound() => knife.PlayCutSound();

        protected override void SetupIngredientStatus()
        {
            allowedStatus = IngredientStatus.Cuttable;
            preparatedStatus = IngredientStatus.Ready;
        }

        protected override string GetIdleAnimationName() => "CuttingBoard@Idle";
        protected override string GetPreparationAnimationName() => "CuttingBoard@Cutting";

        protected override void PlayIdleAnimation()
        {
            base.PlayIdleAnimation();
            if (IsPaused) knife.Hide();
        }

        protected override void PlayPreparationAnimation()
        {
            knife.Show();
            base.PlayPreparationAnimation();
        }

        private void HandleItemPlaced(IItemCollectable _) => knife.Hide();
        private void HandleItemReleased(IItemCollectable _) => knife.Show();
    }
}