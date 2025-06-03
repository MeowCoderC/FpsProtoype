using UnityEngine;

namespace FPSPrototype
{
    public class PlayerLookController : MonoBehaviour
    {
        [Header("Look Settings")]
        [SerializeField] private float sensitivity = 0.1f;
        [SerializeField] private float minPitch = -80f;
        [SerializeField] private float maxPitch = 80f;

        [Header("References")]
        [SerializeField] private Transform cameraHolder;
        [SerializeField] private Transform playerBody;

        private float pitch;

        public void Rotate(Vector2 lookDelta)
        {
            if (playerBody == null || cameraHolder == null) return;

            // Xoay ngang
            playerBody.Rotate(Vector3.up * lookDelta.x * sensitivity);

            // Xoay dọc
            pitch                      -= lookDelta.y * sensitivity;
            pitch                      =  Mathf.Clamp(pitch, minPitch, maxPitch);
            cameraHolder.localRotation =  Quaternion.Euler(pitch, 0f, 0f);
        }
    }
}