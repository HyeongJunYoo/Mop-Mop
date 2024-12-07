using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        private PlayerStateMachine _stateMachine;
        private PlayerInput _playerInput;
        private PlayerMovement _playerMovement;
        private PlayerEnemyDetector _playerEnemyDetector;
        private PlayerAttack _playerAttack;
        public CharacterController CharacterController { get; private set; }
        
#if UNITY_EDITOR
        [SerializeField] private Renderer playerRenderer;
        private void ChangeColor(Color color)
        {
            playerRenderer.material.color = color;
        }
#endif
        private void Awake()
        {
            CharacterController = GetComponent<CharacterController>();       
            _stateMachine = GetComponent<PlayerStateMachine>();
            _playerInput = GetComponent<PlayerInput>();
            _playerMovement = GetComponent<PlayerMovement>();
            _playerEnemyDetector = GetComponent<PlayerEnemyDetector>();
            _playerAttack = GetComponent<PlayerAttack>();
        }
        
        private void Update()
        {
            HandleStateTransition();
            
            switch (_stateMachine.CurrentState)
            {
                case PlayerStateMachine.State.Idle:
                    ChangeColor(Color.white);
                    break;
                case PlayerStateMachine.State.Move:
                    ChangeColor(Color.green);
                    _playerMovement.MoveCharacter();
                    break;
                case PlayerStateMachine.State.Attack:
                    ChangeColor(Color.red);
                    _playerAttack.Attack(_playerEnemyDetector.GetClosestEnemy());
                    break;
            }
        }
        
        private void FixedUpdate()
        {
            switch (_stateMachine.CurrentState)
            {
                case PlayerStateMachine.State.Idle:
                    break;
                case PlayerStateMachine.State.Move:
                    _playerMovement.CalculateNextFixedPosition(_playerInput.MoveInput);
                    break;
                case PlayerStateMachine.State.Attack:
                    break;
            }
        }
        
        private void HandleStateTransition()
        {
            _stateMachine.ChangeState(_playerInput.MoveInput != Vector2.zero
                ? PlayerStateMachine.Trigger.StartMoving
                : PlayerStateMachine.Trigger.StopMoving);

            _stateMachine.ChangeState(_playerEnemyDetector.IsEnemyDetected()
                ? PlayerStateMachine.Trigger.StartAttack
                : PlayerStateMachine.Trigger.StopAttack);
        }
    }
}
