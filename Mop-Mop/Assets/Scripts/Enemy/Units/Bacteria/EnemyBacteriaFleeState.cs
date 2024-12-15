using Interface;
using Manager;
using UnityEngine;

namespace Enemy.Units.Bacteria
{
    public class EnemyBacteriaFleeState : IState, IMovable
    {
        private readonly EnemyBacteriaController _enemy;
        private Vector3 _fleePosition;

        public EnemyBacteriaFleeState(BaseEnemy enemy)
        {
            _enemy = (EnemyBacteriaController)enemy;
            SetSpeed(_enemy.EnemyData.speed);
        }
        
        public void Enter()
        {
           
        }

        public void Update()
        {
            Move();
        }

        public void FixedUpdate()
        {
            
        }

        public void Exit()
        {
            
        }

        public void Move()
        {
            // 플레이어의 위치를 가져옴
            var playerTransform = PlayerManager.Instance.PlayerTransform;
            
            // 플레이어로부터 도망치기 위한 방향 계산
            var directionAwayFromPlayer = _enemy.transform.position - playerTransform.position;
    
            // 방향 벡터를 정규화 (길이를 1로 만듦)
            directionAwayFromPlayer.Normalize();
    
            // 현재 위치에서 플레이어로부터 멀어지는 방향으로 이동
            _fleePosition = _enemy.transform.position + directionAwayFromPlayer;

            // NavMeshAgent를 사용하여 도망칠 위치로 이동
            _enemy.navMeshAgent.SetDestination(_fleePosition);
        }

        public void SetSpeed(float speed)
        {
            _enemy.navMeshAgent.speed = speed;
        }
    }
}
