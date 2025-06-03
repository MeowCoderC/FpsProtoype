using UnityEngine;

namespace FPSPrototype
{
    using System;
    
    [RequireComponent(typeof(PlayerInputHandler))]
    public class PlayerLookController : MonoBehaviour
    {
        [Header("Look Settings")]
        [SerializeField] private float sensitivity = 0.1f;
        [SerializeField] private float minPitch = -80f;
        [SerializeField] private float maxPitch = 80f;

        [Header("References")]
        [SerializeField] private Transform cameraHolder;
        [SerializeField] private Transform playerBody;

        private float              pitch;
        private PlayerInputHandler inputHandler;

        private void Awake()
        {
            inputHandler = GetComponent<PlayerInputHandler>();
        }

        public void Rotate(Vector2 lookDelta)
        {
            if (playerBody == null || cameraHolder == null) return;
            
            //Rotate vertically
            playerBody.Rotate(Vector3.up * lookDelta.x * sensitivity);
            
            //rotate horizontally
            pitch                      -= lookDelta.y * sensitivity;
            pitch                      =  Mathf.Clamp(pitch, minPitch, maxPitch);
            cameraHolder.localRotation =  Quaternion.Euler(pitch, 0f, 0f);
        }

        private void Update()
        {
            Rotate(inputHandler.LookInput);
        }
    }
}