using UnityEngine;

namespace Player
{
    public class PlayerEnemyDetector : MonoBehaviour
    {
        private LayerMask _enemyLayer;
        private Collider[] EnemiesCollider { get; set; }
        public float AttackRange { get; private set; }

        private void Awake()
        {
            EnemiesCollider = new Collider[10];
            _enemyLayer = LayerMask.GetMask("Enemy");
            AttackRange = 5.0f;
        }
        
        public bool IsEnemyDetected()
        {
            // 적이 감지되었는지 여부 반환
            return Physics.OverlapSphereNonAlloc(transform.position, AttackRange, EnemiesCollider, _enemyLayer) > 0;
        }
        
        public GameObject GetClosestEnemy()
        {
            Collider closestEnemy = null;
            var minDistanceSqr = float.MaxValue;
            
            // 주변에 있는 적들을 검색합니다.
            var enemyCount = Physics.OverlapSphereNonAlloc(transform.position, AttackRange, EnemiesCollider, _enemyLayer);
            
            // 가장 가까운 적을 찾습니다.
            for (var i = 0; i < enemyCount; i++)
            {
                var col = EnemiesCollider[i];
                if (col == null) continue;

                var directionToEnemy = col.transform.position - transform.position;
                var distanceSqr = directionToEnemy.sqrMagnitude;

                if (distanceSqr < minDistanceSqr)
                {
                    minDistanceSqr = distanceSqr;
                    closestEnemy = col;
                }
            }

            // 가장 가까운 적의 게임 오브젝트를 반환합니다.
            return closestEnemy?.gameObject;
        }
        

#if UNITY_EDITOR
        // 에디터 상에서만 적의 감지 범위를 표시합니다.
        public void OnDrawGizmos()
        {
            Gizmos.color = Color.red;   
            Gizmos.DrawWireSphere(transform.position, AttackRange);
        }
#endif
    }
}
