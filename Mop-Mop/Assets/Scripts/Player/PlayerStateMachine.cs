using Stateless;
using UnityEngine;

namespace Player
{
    public class PlayerStateMachine : MonoBehaviour
    {
        public enum State { Idle, Move, Attack } 
        public enum Trigger { StartMoving, StopMoving, StartAttack, StopAttack }

        private StateMachine<State, Trigger> _stateMachine;
        private void Awake()
        {
            _stateMachine = new StateMachine<State, Trigger>(State.Idle);
        }

        private void Start()
        {
            _stateMachine.Configure(State.Idle)
                .OnEntry(()=> Debug.Log("Entering Idle State"))
                .OnExit(()=> Debug.Log("Exiting Idle State"))
                .Permit(Trigger.StartMoving, State.Move)
                .Permit(Trigger.StartAttack, State.Attack);

            _stateMachine.Configure(State.Move)
                .OnEntry(() => Debug.Log("Entering Move State"))
                .OnExit(() => Debug.Log("Exiting Move State"))
                .Permit(Trigger.StopMoving, State.Idle);
            
            _stateMachine.Configure(State.Attack)
                .OnEntry(()=> Debug.Log("Entering Attack State"))
                .OnExit(()=> Debug.Log("Exiting Attack State"))
                .Permit(Trigger.StartMoving, State.Move)
                .Permit(Trigger.StopAttack, State.Idle);
        }

        public void ChangeState(Trigger trigger)
        {
            if (_stateMachine.CanFire(trigger))
            {
                _stateMachine.Fire(trigger);
            }
        }
        
        public State CurrentState => _stateMachine.State;
    }
}
