using Enemy.Units.Bacteria;
using Interface;
using Stateless;
using UnityEngine;

namespace Enemy
{
    public class EnemyStateMachine
    {
        private enum State { Idle, Move, Flee } 
        public enum Trigger { StartMoving, StopMoving,  Damaged }
        private readonly StateMachine<State, Trigger> _stateMachine;
        
        private IState CurrentState { get; set; }
        private readonly EnemyBacteriaIdleState _idleState;
        private readonly EnemyBacteriaFleeState _fleeState;
        
        public EnemyStateMachine(BaseEnemy enemy)
        {
            _idleState = new EnemyBacteriaIdleState(enemy);
            _fleeState = new EnemyBacteriaFleeState(enemy);
            CurrentState = _idleState;
            
            _stateMachine = new StateMachine<State, Trigger>(State.Idle);
            _stateMachine.Configure(State.Idle)
                .Permit(Trigger.StartMoving, State.Move)
                .Permit(Trigger.Damaged, State.Flee);

            _stateMachine.Configure(State.Move)
                .Permit(Trigger.StopMoving, State.Idle)
                .Permit(Trigger.Damaged, State.Flee);

            _stateMachine.Configure(State.Flee)
                .Permit(Trigger.StopMoving, State.Idle)
                .Permit(Trigger.StartMoving, State.Move);
        }

        public void ChangeState(Trigger trigger)
        {
            if (!_stateMachine.CanFire(trigger)) return;
            
            CurrentState?.Exit();
            _stateMachine.Fire(trigger);
            CurrentState = _stateMachine.State switch
            {
                State.Idle => _idleState,
                State.Move => null,
                State.Flee => _fleeState,
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
