using UnityEngine;

namespace FPSPrototype
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovementController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float jumpForce = 5f;
        [SerializeField] private float gravity   = -9.81f;

        [Header("Ground Check")]
        [SerializeField] private Transform groundCheck;
        [SerializeField] private float     groundRadius = 0.2f;
        [SerializeField] private LayerMask groundMask;

        private CharacterController controller;
        private Vector3             velocity;
        private bool                isGrounded;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
        }

        public void Move(Vector2 moveInput, bool jumpPressed)
        {
            // Kiểm tra đang đứng trên mặt đất
            isGrounded = Physics.CheckSphere(groundCheck.position, groundRadius, groundMask);

            if (isGrounded && velocity.y < 0)
                velocity.y = -2f;

            // Tính hướng di chuyển
            Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
            controller.Move(move * moveSpeed * Time.deltaTime);

            // Nhảy
            if (isGrounded && jumpPressed)
                velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);

            // Trọng lực
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
    }
}