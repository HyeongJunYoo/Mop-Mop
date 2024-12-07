using Enemy.Data;
using UnityEngine;

namespace Enemy
{
    public abstract class BaseEnemy : MonoBehaviour
    { 
        public abstract EnemyData EnemyData { get; set; }
    }
}
