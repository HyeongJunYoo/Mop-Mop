using Enemy.Interface;
using Manager;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy.Units.Bacteria
{
    public class EnemyBacteriaMovement : MonoBehaviour, IMovable
    {
        private NavMeshAgent _navMeshAgent;
        private Vector3 _fleePosition;

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        public void Flee()
        {
            // 플레이어의 위치를 가져옴
            var playerTransform = PlayerManager.Instance.PlayerTransform;
            
            // 플레이어로부터 도망치기 위한 방향 계산
            var directionAwayFromPlayer = transform.position - playerTransform.position;
    
            // 방향 벡터를 정규화 (길이를 1로 만듦)
            directionAwayFromPlayer.Normalize();
    
            // 현재 위치에서 플레이어로부터 멀어지는 방향으로 이동
            _fleePosition = transform.position + directionAwayFromPlayer;

            // NavMeshAgent를 사용하여 도망칠 위치로 이동
            _navMeshAgent.SetDestination(_fleePosition);
        }

        public void SetSpeed(float speed)
        {
            _navMeshAgent.speed = speed;
        }

#if UNITY_EDITOR
        // 에디터 상에서만 도망칠 위치를 표시합니다.
        private  void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(_fleePosition, 0.5f);
        }
#endif
    }
}
