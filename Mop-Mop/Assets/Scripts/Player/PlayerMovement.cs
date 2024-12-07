using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        private CharacterController _characterController;
        private Vector3 _lastFixedPosition;
        private Vector3 _nextFixedPosition;
        private Vector3 _velocity;
        
        private const float RayDistance = 0.65f;
        private const float Speed = 5.0f;
        
        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
        }
        
        public void MoveCharacter()
        {
            var interpolationAlpha = (Time.time - Time.fixedTime) / Time.fixedDeltaTime;
            _characterController.Move(Vector3.Lerp(_lastFixedPosition, _nextFixedPosition, interpolationAlpha) - transform.position);
        }
        
        public void CalculateNextFixedPosition(Vector2 moveInput)
        {
            // 이전 위치를 저장합니다.
            _lastFixedPosition = _nextFixedPosition;

            // 입력값을 받아서 이동 방향을 계산합니다.
            var planeVelocity = GetXZVelocity(moveInput.x, moveInput.y);

            // 이동 방향을 y축을 제외한 방향으로 설정합니다.
            _velocity = new Vector3(planeVelocity.x, 0, planeVelocity.z);
            
            // 이동 방향으로 Ray를 쏩니다. RayDistance 만큼의 거리만큼 Ray를 쏩니다.
            var distance = moveInput.magnitude * RayDistance;
            var ray = new Ray(transform.position, _velocity.normalized);
            Debug.DrawRay(ray.origin, ray.direction * distance, Color.blue);
            
            // 벽에 부딪히면 벽의 법선 방향에 수직으로 이동합니다.
            if (Physics.Raycast(ray, out var hit, distance, LayerMask.GetMask("Wall")))
            {
                var wallNormal = hit.normal;

                // 수평 방향키 입력이 있을 경우
                if ((moveInput.x < 0 && wallNormal.x > 0) || (moveInput.x > 0 && wallNormal.x < 0)) // 왼쪽 또는 오른쪽 벽
                {
                    // 수직 입력이 있으면 위아래로 이동
                    if (moveInput.y != 0) // 위 또는 아래 방향키
                    {
                        _nextFixedPosition = transform.position + new Vector3(0, 0, moveInput.y * Time.fixedDeltaTime);
                    }
                    else
                    {
                        _nextFixedPosition = transform.position; // 정지
                    }
                }
                else if ((moveInput.y < 0 && wallNormal.z > 0) || (moveInput.y > 0 && wallNormal.z < 0)) // 아래쪽 또는 위쪽 벽
                {
                    // 수평 입력이 있으면 좌우로 이동
                    if (moveInput.x != 0) // 왼쪽 또는 오른쪽 방향키
                    {
                        _nextFixedPosition = transform.position + new Vector3(moveInput.x * Time.fixedDeltaTime, 0, 0);
                    }
                    else
                    {
                        _nextFixedPosition = transform.position; // 정지
                    }
                }
                else
                {
                    // 벽에 닿았지만 수평 방향키가 없을 경우
                    _nextFixedPosition = transform.position; // 정지
                }
            }
            else
            {
                // 벽에 닿지 않은 경우 일반적인 이동 로직
                _nextFixedPosition += _velocity * Time.fixedDeltaTime;    
            }
        }
        
        /// <summary>
        /// 대각선 이동에서도 일정한 속도로 이동하기 위해 x, z축의 속도를 계산합니다.
        /// </summary>
        /// <param name="horizontalInput"></param>
        /// <param name="verticalInput"></param>
        /// <returns></returns>
        private Vector3 GetXZVelocity(float horizontalInput, float verticalInput)
        {
            var moveVelocity = transform.forward * verticalInput + transform.right * horizontalInput;
            var moveDirection = moveVelocity.normalized;
            var moveSpeed = Mathf.Min(moveVelocity.magnitude, 1.0f) * Speed;

            return moveDirection * moveSpeed;
        }
    }
}
