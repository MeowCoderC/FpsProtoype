using UnityEngine;
using UnityEngine.InputSystem;

namespace FPSPrototype
{
    public class PlayerInputHandler : MonoBehaviour
    {
        public Vector2 MoveInput   { get; private set; }
        public Vector2 LookInput   { get; private set; }
        public bool    JumpPressed { get; private set; }
        public bool    SprintHeld  { get; private set; }
        public bool    JetpackHeld { get; private set; }

        public bool IsAiming { get; private set; }
        public bool IsFiring { get; private set; }

        private PlayerInputActions inputActions;

        private void Awake()
        {
            inputActions = new PlayerInputActions();

            inputActions.Player.Move.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
            inputActions.Player.Move.canceled  += ctx => MoveInput = Vector2.zero;

            inputActions.Player.Look.performed += ctx => LookInput = ctx.ReadValue<Vector2>();
            inputActions.Player.Look.canceled  += ctx => LookInput = Vector2.zero;

            inputActions.Player.Jump.performed += ctx => JumpPressed = true;
            inputActions.Player.Jump.canceled  += ctx => JumpPressed = false;

            inputActions.Player.Sprint.performed += ctx => SprintHeld = true;
            inputActions.Player.Sprint.canceled  += ctx => SprintHeld = false;

            inputActions.Player.Jetpack.performed += ctx => JetpackHeld = true;
            inputActions.Player.Jetpack.canceled  += ctx => JetpackHeld = false;

            inputActions.Player.Aim.performed += ctx => IsAiming = true;
            inputActions.Player.Aim.canceled  += ctx => IsAiming = false;

            inputActions.Player.Fire.performed += ctx => IsFiring = true;
            inputActions.Player.Fire.canceled  += ctx => IsFiring = false;
        }

        private void OnEnable() => inputActions.Enable();

        private void OnDisable() => inputActions.Disable();
    }
}