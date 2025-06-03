using UnityEngine;

namespace FPSPrototype
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "FPSPrototype/PlayerData")]
    public class PlayerDataSO : ScriptableObject
    {
        [Header("Movement")]
        public float moveSpeed = 5f;
        public float sprintMultiplier = 1.5f;

        [Header("Jump")]
        public float jumpForce = 2f;
        public float gravity        = -9.81f;
        public float fallMultiplier = 5f;
        public float riseMultiplier = 1.5f;

        [Header("Ground Check")]
        public float groundedOffset = -0.63f;
        public float     groundedRadius = 0.6f;
        public LayerMask groundLayers;
    }
}