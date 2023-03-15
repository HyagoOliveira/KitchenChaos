using UnityEngine;

namespace KitchenChaos.Players
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody))]
    public sealed class PlayerMotor : MonoBehaviour
    {
        [SerializeField] private Rigidbody body;
        [SerializeField] private PlayerAnimator animator;
        [SerializeField, Min(0F)] private float moveSpeed = 6f;
        [SerializeField, Min(0F)] private float dashSpeed = 12f;
        [SerializeField, Min(0F)] private float dashTime = 0.8f;

        public bool IsMoveInputting { get; private set; }

        public Vector2 MoveInput { get; private set; }
        public Vector3 Speed { get; private set; }
        public Vector3 Velocity { get; private set; }

        private bool isDashing;
        private float currentSpeed;
        private Vector3 moveDirection;
        private Transform currentCamera;

        private void Reset()
        {
            body = GetComponent<Rigidbody>();
            animator = GetComponentInChildren<PlayerAnimator>();
        }

        private void Start()
        {
            StopDash();
            currentCamera = Camera.main.transform;
        }

        private void Update()
        {
            UpdateMovement();
            UpdateRotation();
            UpdateAnimations();
        }

        public void Move(Vector2 input)
        {
            MoveInput = input;
            IsMoveInputting = Mathf.Abs(MoveInput.sqrMagnitude) > 0F;
        }

        public void Dash()
        {
            if (isDashing) return;

            currentSpeed = dashSpeed;
            Invoke(nameof(StopDash), dashTime);
        }

        public void Stop()
        {
            StopDash();
            Move(Vector2.zero);
        }

        private void UpdateMovement()
        {
            UpdateMovingDirection();

            Speed = currentSpeed * moveDirection;
            Velocity = Speed * Time.deltaTime;


            var newPosition = body.position + Velocity;

            body.MovePosition(newPosition);
        }

        private void UpdateRotation()
        {
            var direction = transform.position + moveDirection;
            transform.LookAt(direction);
        }

        private void UpdateMovingDirection()
        {
            moveDirection = IsMoveInputting ?
                GetMoveInputDirectionRelativeToCamera() :
                Vector3.zero;
        }

        private void UpdateAnimations()
        {
            animator.SetIsWalking(IsMoveInputting);
        }

        private void StopDash()
        {
            isDashing = false;
            currentSpeed = moveSpeed;
        }

        private Vector3 GetMoveInputDirectionRelativeToCamera()
        {
            var right = currentCamera.right;
            right.y = 0f;
            var forward = Vector3.Cross(right, Vector3.up);
            // do not normalize if player should walk according with move input.
            return (right * MoveInput.x + forward * MoveInput.y).normalized;
        }
    }
}