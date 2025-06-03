using UnityEngine;

namespace FPSPrototype
{
    using System;

    [RequireComponent(typeof(PlayerInputHandler),typeof(PlayerLookController), typeof(PlayerMovementController))]
    public class PlayerController : MonoBehaviour
    {
        private PlayerLookController lookController;
        private PlayerMovementController movementController;
        private PlayerInputHandler inputHandler;

        private void Awake()
        {
            inputHandler       = GetComponent<PlayerInputHandler>();
            lookController     = GetComponent<PlayerLookController>();
            movementController = GetComponent<PlayerMovementController>();
        }

        private void Update()
        {
            // Gửi input xoay nhìn
            lookController?.Rotate(inputHandler.LookInput);

            // Gửi input di chuyển và nhảy
            movementController?.Move(inputHandler.MoveInput, inputHandler.JumpPressed);
        }
    }
}