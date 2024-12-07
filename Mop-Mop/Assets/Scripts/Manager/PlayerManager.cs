using Player;
using UnityEngine;

namespace Manager
{
    public class PlayerManager : SingletonBehaviour<PlayerManager>
    {
        public PlayerController playerController;
        public Transform PlayerTransform => playerController.gameObject.transform;
        protected override void Awake()
        {
            playerController = FindFirstObjectByType<PlayerController>();
        }
        
    }
}