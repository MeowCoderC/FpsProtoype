using UnityEngine;


namespace FPSPrototype
{
    using System;

    [RequireComponent(typeof(PlayerInputHandler))]
    public class PlayerWeaponController : MonoBehaviour
    {
        private PlayerInputHandler playerInputHandler;
        public WeaponBase         currentWeapon;

        private void Awake()
        {
            playerInputHandler = GetComponent<PlayerInputHandler>();
        }

        private void Update()
        {
            if (this.playerInputHandler.IsFiring)
            {
                this.currentWeapon.Attack();
            }
        }
    }
}


