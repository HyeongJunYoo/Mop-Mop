using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        private PlayerStateMachine _stateMachine;
        public PlayerInput input;
        public PlayerEnemyDetector enemyDetector;
      
        public CharacterController CharacterController { get; private set; }
        
#if UNITY_EDITOR
        [SerializeField] private Renderer playerRenderer;
        public void ChangeColor(Color color)
        {
            playerRenderer.material.color = color;
        }
#endif
        private void Awake()
        {
            CharacterController = GetComponent<CharacterController>();       
            input = GetComponent<PlayerInput>();
            enemyDetector = GetComponent<PlayerEnemyDetector>();
            _stateMachine = new PlayerStateMachine(this);
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
            _stateMachine.ChangeState(input.MoveInput != Vector2.zero
                ? PlayerStateMachine.Trigger.StartMoving
                : PlayerStateMachine.Trigger.StopMoving);

            _stateMachine.ChangeState(enemyDetector.IsEnemyDetected()
                ? PlayerStateMachine.Trigger.StartAttack
                : PlayerStateMachine.Trigger.StopAttack);
        }
    }
}
