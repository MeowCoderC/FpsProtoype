using UnityEngine;

namespace FPSPrototype
{
    [RequireComponent(typeof(PlayerInputHandler), typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PlayerDataSO playerData;

        private CharacterController characterController;
        private PlayerInputHandler inputHandler;
        private Vector3 velocity;
        public Vector3 Velocity
        {
            get => velocity;
            set => velocity = value;
        }
        private bool isGrounded;

        public bool IsGrounded => isGrounded;

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
            inputHandler = GetComponent<PlayerInputHandler>();
        }

        private void Update()
        {
            CheckGrounded();

            Vector2 moveInput = inputHandler.MoveInput;
            bool jumpPressed = inputHandler.JumpPressed;

            HandleMovement(moveInput);
            HandleJumpAndGravity(jumpPressed);
        }

        private void HandleMovement(Vector2 moveInput)
        {
            float speed = playerData.moveSpeed;
            if (inputHandler.SprintHeld)
                speed *= playerData.sprintMultiplier;

            Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
            characterController.Move(move * speed * Time.deltaTime);
        }

        private void HandleJumpAndGravity(bool jumpPressed)
        {
            if (isGrounded && velocity.y < 0)
                velocity.y = -2f;

            if (isGrounded && jumpPressed)
                velocity.y = Mathf.Sqrt(playerData.jumpForce * -2f * playerData.gravity);

            if (velocity.y < 0)
            {
                // Falling
                velocity.y += playerData.gravity * playerData.fallMultiplier * Time.deltaTime;
            }
            else if (velocity.y > 0)
            {
                // Rising
                velocity.y += playerData.gravity * playerData.riseMultiplier * Time.deltaTime;
            }
            else
            {
                velocity.y += playerData.gravity * Time.deltaTime;
            }

            characterController.Move(Vector3.up * velocity.y * Time.deltaTime);
        }

        private void CheckGrounded()
        {
            Vector3 spherePosition = transform.position + Vector3.up * playerData.groundedOffset;
            isGrounded = Physics.CheckSphere(spherePosition, playerData.groundedRadius, playerData.groundLayers, QueryTriggerInteraction.Ignore);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = IsGrounded ? new Color(0f, 1f, 0f, 0.35f) : new Color(1f, 0f, 0f, 0.35f);
            Vector3 spherePosition = transform.position + Vector3.up * playerData.groundedOffset;
            Gizmos.DrawSphere(spherePosition, playerData.groundedRadius);
        }
    }
}
