using Interface;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy.Units.Bacteria
{
    public class EnemyBacteriaMoveState : IState
    {
        private readonly EnemyBacteriaController _enemy;
        private readonly float _updateRate = 5f; // 목표 위치를 갱신할 시간 간격 (초)
        private float _timeElapsed;

        public EnemyBacteriaMoveState(BaseEnemy enemy)
        {
            _enemy = (EnemyBacteriaController)enemy;
        }
        
        public void Enter()
        {
            _timeElapsed = _updateRate;
        }

        public void Update()
        {
            _timeElapsed += Time.deltaTime; // 시간 값을 갱신합니다.
 
            if (_timeElapsed >= _updateRate) // 설정한 시간 간격이 지났는지 확인합니다.
            {
                _enemy.navMeshAgent.SetDestination( GetRandomPositionOnNavMesh()); // NavMeshAgent의 목표 위치를 랜덤 위치로 설정합니다.
            }
            
            if (_enemy.navMeshAgent.remainingDistance <= _enemy.navMeshAgent.stoppingDistance) // 목표 위치에 도착했는지 확인합니다.
            {
                _enemy.navMeshAgent.SetDestination(GetRandomPositionOnNavMesh()); // 새로운 랜덤 위치로 이동합니다.
            }
        }

        public void FixedUpdate()
        {
            
        }

        public void Exit()
        {
            
        }
        
        private Vector3 GetRandomPositionOnNavMesh()
        {
            var randomDirection = Random.insideUnitSphere * 20; // 원하는 범위 내의 랜덤한 방향 벡터를 생성합니다.

            if (NavMesh.SamplePosition(randomDirection, out var hit, 20f, NavMesh.AllAreas)) // 랜덤 위치가 NavMesh 위에 있는지 확인합니다.
            {
                _timeElapsed = 0f; // 시간 값을 초기화합니다.
                return hit.position; // NavMesh 위의 랜덤 위치를 반환합니다.
            }

            return _enemy.transform.position; // NavMesh 위에 없다면 현재 위치를 반환합니다.
        }
    }
}
