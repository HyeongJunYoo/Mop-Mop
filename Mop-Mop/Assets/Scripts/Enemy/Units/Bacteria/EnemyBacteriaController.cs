using Enemy.Data;
using UnityEngine;

namespace Enemy.Units.Bacteria
{
    public class EnemyBacteriaController : BaseEnemy
    {
        [SerializeField] private EnemyData enemyData;
        public override EnemyData EnemyData
        {
            get => enemyData;
            set => enemyData = value;
        }

        private EnemyStateMachine _stateMachine;
        private EnemyBacteriaMovement _movement;
        private EnemyBacteriaHealth _health;
        
#if UNITY_EDITOR
        [SerializeField] private Renderer playerRenderer;
        private void ChangeColor(Color color)
        {
            playerRenderer.material.color = color;
        }
#endif
        private void Awake()
        {
            _stateMachine = GetComponent<EnemyStateMachine>();
            _movement = GetComponent<EnemyBacteriaMovement>();
            _health = GetComponent<EnemyBacteriaHealth>();
        }
        
        private void Start()
        {
            Initialize();
        }
        
        private void Initialize()
        {
            _movement.SetSpeed(EnemyData.speed);
            _health.SetHp(EnemyData.health);
        }
        
        private void Update()
        {
            HandleStateTransition();
            
            switch (_stateMachine.CurrentState)
            {
                case EnemyStateMachine.State.Idle:
                    break;
                case EnemyStateMachine.State.Move:
                    break;
                case EnemyStateMachine.State.RunAway:
                    _movement.Flee();
                    break;
            }
        }
        
        private void HandleStateTransition()
        {
            if(_health.IsDamaged)
                _stateMachine.ChangeState(EnemyStateMachine.Trigger.Damaged);
            
        }
        
    }
}
