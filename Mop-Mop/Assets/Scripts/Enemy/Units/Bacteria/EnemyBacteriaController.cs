using Enemy.Data;
using UnityEngine;
using UnityEngine.AI;

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
        private EnemyBacteriaHealth _health;
        public NavMeshAgent navMeshAgent;

        
#if UNITY_EDITOR
        [SerializeField] private Renderer playerRenderer;
        public void ChangeColor(Color color)
        {
            playerRenderer.material.color = color;
        }
#endif
        private void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            _health = GetComponent<EnemyBacteriaHealth>();
        }
        
        private void Start()
        {
            _stateMachine = new EnemyStateMachine(this);
            Initialize();
        }
        
        private void Initialize()
        {
            _health.SetHp(EnemyData.health);
        }
        
        private void Update()
        {
            HandleStateTransition();
            
            _stateMachine.Update();
        }
        
        private void FixedUpdate()
        {
            _stateMachine.FixedUpdate();
        }
        
        private void HandleStateTransition()
        {
            if(_health.IsDamaged)
                _stateMachine.ChangeState(EnemyStateMachine.Trigger.Damaged);
            
        }
        
    }
}
