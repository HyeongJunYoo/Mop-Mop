using Interface;
using Stateless;
using UnityEngine;

namespace Player
{
    public class PlayerStateMachine
    {
        private enum State { Idle, Move, Attack } 
        public enum Trigger { StartMoving, StopMoving, StartAttack, StopAttack }
        private readonly StateMachine<State, Trigger> _stateMachine;

        private IState CurrentState { get; set; }
        private readonly PlayerIdleState _idleState;
        private readonly PlayerMoveState _moveState;
        private readonly PlayerAttackState _attackState;
        
        public PlayerStateMachine(PlayerController player)
        {
            _idleState = new PlayerIdleState(player);
            _moveState = new PlayerMoveState(player);
            _attackState = new PlayerAttackState(player);
            CurrentState = _idleState;
            
            _stateMachine = new StateMachine<State, Trigger>(State.Idle);
            _stateMachine.Configure(State.Idle)
                .Permit(Trigger.StartMoving, State.Move)
                .Permit(Trigger.StartAttack, State.Attack);

            _stateMachine.Configure(State.Move)
                .Permit(Trigger.StopMoving, State.Idle);
            
            _stateMachine.Configure(State.Attack)
                .Permit(Trigger.StartMoving, State.Move)
                .Permit(Trigger.StopAttack, State.Idle);
        }
        
        public void ChangeState(Trigger trigger)
        {
            if (!_stateMachine.CanFire(trigger)) return;
            
            CurrentState?.Exit();
            _stateMachine.Fire(trigger);
            CurrentState = _stateMachine.State switch
            {
                State.Idle => _idleState,
                State.Move => _moveState,
                State.Attack => _attackState,
                _ => null
            };
            CurrentState?.Enter();
        }
        
        public void Update()
        {
            CurrentState?.Update();
        }
        
        public void FixedUpdate()
        {
            CurrentState?.FixedUpdate();
        }
    }
}
