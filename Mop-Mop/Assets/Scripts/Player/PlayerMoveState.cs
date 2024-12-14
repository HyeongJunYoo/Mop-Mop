using Interface;
using UnityEngine;

namespace Player
{
    public class PlayerMoveState : IState
    {
        private readonly PlayerController _player;
        private readonly LayerMask _obstacleLayer;
        private Vector3 _lastFixedPosition;
        private Vector3 _nextFixedPosition;
        private Vector3 _velocity;
        
        private const float Speed = 5.0f;
        
        public PlayerMoveState(PlayerController player)
        {
            _player = player;
            _obstacleLayer = LayerMask.GetMask("Wall", "Enemy");
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
        }

        private void CalculateNextFixedPosition(Vector2 moveInput)
        {
            // 이전 위치를 저장합니다.
            _lastFixedPosition = _nextFixedPosition;

            // 입력값을 받아서 이동 방향을 계산합니다.
            var planeVelocity = GetXZVelocity(moveInput.x, moveInput.y);

            // 이동 방향을 y축을 제외한 방향으로 설정합니다.
            _velocity = new Vector3(planeVelocity.x, 0, planeVelocity.z);
            
            var targetPosition = _nextFixedPosition + _velocity * Time.fixedDeltaTime;
            
            // 실제 이동 가능 여부를 체크 (충돌 등을 고려)
            if (CanMoveToNextPosition(targetPosition))
                _nextFixedPosition = targetPosition;
            else
                _nextFixedPosition = _player.CharacterController.transform.position + _velocity * Time.fixedDeltaTime;

            Debug.Log(_nextFixedPosition);
        }
        
        private bool CanMoveToNextPosition(Vector3 targetPosition)
        {
            return !Physics.CheckCapsule(_lastFixedPosition, targetPosition, 0.5f, _obstacleLayer);
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
