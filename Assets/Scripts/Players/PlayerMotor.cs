using UnityEngine;

namespace KitchenChaos.Players
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    public sealed class PlayerMotor : MonoBehaviour
    {
        [SerializeField] private Rigidbody body;
        [SerializeField] private CapsuleCollider collider;
        [SerializeField] private PlayerAnimator animator;
        [SerializeField, Min(0F)] internal float moveSpeed = 6f;

        public bool IsMoveInputting { get; private set; }

        public Vector2 MoveInput { get; private set; }
        public Vector3 Speed { get; private set; }
        public Vector3 Velocity { get; private set; }

        private Vector3 moveDirection;
        private Transform currentCamera;

        private void Reset()
        {
            body = GetComponent<Rigidbody>();
            collider = GetComponent<CapsuleCollider>();
            animator = GetComponentInChildren<PlayerAnimator>();
        }

        private void Start() => currentCamera = Camera.main.transform;

        private void FixedUpdate()
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

        public void Stop() => Move(Vector2.zero);

        public bool IsMoving() => Mathf.Abs(Speed.sqrMagnitude) > 0F;

        private void UpdateMovement()
        {
            UpdateMovingDirection();

            var isMovingIntoCollision = IsMoveInputting && IsForwardCollision();

            Speed = isMovingIntoCollision ? Vector3.zero : moveSpeed * moveDirection;
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

        private bool IsForwardCollision()
        {
            var origin = body.position + Vector3.up;
            return UnityEngine.Physics.Raycast(origin, transform.forward, collider.radius);
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