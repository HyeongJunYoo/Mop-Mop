using Interface;
using UnityEngine;

namespace Player
{
    public class PlayerMoveState : IState
    {
        private readonly PlayerController _player;
        private Vector3 _lastFixedPosition;
        private Vector3 _nextFixedPosition;
        private Vector3 _velocity;
        
        private const float RayDistance = 0.65f;
        private const float Speed = 5.0f;
        
        public PlayerMoveState(PlayerController player)
        {
            _player = player;
        }
        
        public void Enter()
        {
            _player.ChangeColor(Color.green);
        }

        public void Update()
        {
            MoveCharacter();
        }

        public void FixedUpdate()
        {
            CalculateNextFixedPosition(_player.input.MoveInput);
        }

        public void Exit()
        {
            
        }

        private void MoveCharacter()
        {
            var interpolationAlpha = (Time.time - Time.fixedTime) / Time.fixedDeltaTime;
            var moveDirection = Vector3.Lerp(_lastFixedPosition, _nextFixedPosition, interpolationAlpha) -
                                _player.transform.position;
            _player.CharacterController.Move(moveDirection);
            
            if(_player.CharacterController.velocity.magnitude < 0.1f)
                _nextFixedPosition = _player.transform.position;
        }

        private void CalculateNextFixedPosition(Vector2 moveInput)
        {
            // 이전 위치를 저장합니다.
            _lastFixedPosition = _nextFixedPosition;

            // 입력값을 받아서 이동 방향을 계산합니다.
            var planeVelocity = GetXZVelocity(moveInput.x, moveInput.y);

            // 이동 방향을 y축을 제외한 방향으로 설정합니다.
            _velocity = new Vector3(planeVelocity.x, 0, planeVelocity.z);
            
            _nextFixedPosition += _velocity * Time.fixedDeltaTime;
            
            Debug.Log(_nextFixedPosition);
        }
        
        
        
        /// <summary>
        /// 대각선 이동에서도 일정한 속도로 이동하기 위해 x, z축의 속도를 계산합니다.
        /// </summary>
        /// <param name="horizontalInput"></param>
        /// <param name="verticalInput"></param>
        /// <returns></returns>
        private Vector3 GetXZVelocity(float horizontalInput, float verticalInput)
        {
            var moveVelocity = _player.transform.forward * verticalInput + _player.transform.right * horizontalInput;
            var moveDirection = moveVelocity.normalized;
            var moveSpeed = Mathf.Min(moveVelocity.magnitude, 1.0f) * Speed;

            return moveDirection * moveSpeed;
        }
    }
}
