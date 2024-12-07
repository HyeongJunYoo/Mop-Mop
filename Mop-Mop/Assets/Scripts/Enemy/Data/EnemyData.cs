using UnityEngine;

namespace Enemy.Data
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyData", order = 1)]
    public class EnemyData : ScriptableObject
    {
        public string enemyName;          // 적 이름
        public string description;        // 적 설명
        public int health;                // 체력
        public float speed;               // 이동 속도
    }
}
