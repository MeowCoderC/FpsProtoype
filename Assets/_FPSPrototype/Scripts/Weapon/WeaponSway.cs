using UnityEngine;

namespace FPSPrototype.WeaponSystem
{
    public class WeaponSway : MonoBehaviour
    {
        [Header("Sway Settings")]
        [SerializeField] private float swayAmount = 0.05f;
        [SerializeField] private float maxSwayAmount = 0.06f;
        [SerializeField] private float smoothAmount  = 6f;

        [Header("References")]
        [SerializeField] private PlayerInputHandler playerInputHandler;

        private Vector3 initialPosition;

        private void Start()
        {
            initialPosition = transform.localPosition;

            // Nếu chưa gán thủ công
            if (playerInputHandler == null)
            {
                playerInputHandler = FindObjectOfType<PlayerInputHandler>();
            }
        }

        private void LateUpdate()
        {
            Vector2 lookInput = playerInputHandler?.LookInput ?? Vector2.zero;

            float moveX = -lookInput.x * swayAmount;
            float moveY = -lookInput.y * swayAmount;

            moveX = Mathf.Clamp(moveX, -maxSwayAmount, maxSwayAmount);
            moveY = Mathf.Clamp(moveY, -maxSwayAmount, maxSwayAmount);

            Vector3 finalPosition = initialPosition + new Vector3(moveX, moveY, 0f);
            transform.localPosition = Vector3.Lerp(transform.localPosition, finalPosition, Time.deltaTime * smoothAmount);
        }
    }
}