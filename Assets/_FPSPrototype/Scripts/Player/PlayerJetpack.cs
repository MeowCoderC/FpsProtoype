using UnityEngine;

namespace FPSPrototype
{
    public class PlayerJetpack : MonoBehaviour
    {
        [SerializeField] private float jetpackForce        = 10f;
        [SerializeField] private float fuelCapacity        = 5f;
        [SerializeField] private float fuelRechargeRate    = 2f;
        [SerializeField] private float fuelConsumptionRate = 1f;

        private float               currentFuel;
        private PlayerInputHandler  inputHandler;
        private PlayerController    playerController;
        private bool                wasJetpackingLastFrame;

        private void Awake()
        {
            inputHandler        = GetComponent<PlayerInputHandler>();
            playerController    = GetComponent<PlayerController>();
            currentFuel         = fuelCapacity;
        }

        private void Update()
        {
            bool isJetpacking = inputHandler.JetpackHeld && currentFuel > 0f;

            if (isJetpacking)
            {
                playerController.Velocity = new Vector3(
                    playerController.Velocity.x,
                    jetpackForce,
                    playerController.Velocity.z
                );

                currentFuel -= fuelConsumptionRate * Time.deltaTime;
            }
            else if (wasJetpackingLastFrame)
            {
                if (playerController.Velocity.y > 0f)
                {
                    playerController.Velocity = new Vector3(
                        playerController.Velocity.x,
                        0f,
                        playerController.Velocity.z
                    );
                }
            }

            if (!isJetpacking && playerController.IsGrounded)
            {
                currentFuel = Mathf.Min(currentFuel + fuelRechargeRate * Time.deltaTime, fuelCapacity);
            }

            wasJetpackingLastFrame = isJetpacking;
        }





        public float FuelPercent => currentFuel / fuelCapacity;
    }
}