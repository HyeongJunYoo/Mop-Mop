using Interface;
using UnityEngine;

namespace Player
{
    public class PlayerIdleState : IState
    {
        private readonly PlayerController _player;

        public PlayerIdleState(PlayerController player)
        {
            _player = player;
        }
        
        public void Enter()
        {
            _player.ChangeColor(Color.gray);
        }

        public void Update()
        {
        }

        public void FixedUpdate()
        {
        }

        public void Exit()
        {
        }
    }
}
