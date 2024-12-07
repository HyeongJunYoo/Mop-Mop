using Stateless;
using UnityEngine;

namespace Enemy
{
    public class EnemyStateMachine : MonoBehaviour
    {
        public enum State { Idle, Move, RunAway } 
        public enum Trigger { StartMoving, StopMoving,  Damaged }

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
                .Permit(Trigger.Damaged, State.RunAway);

            _stateMachine.Configure(State.Move)
                .OnEntry(()=> Debug.Log("Entering Move State"))
                .OnExit(()=> Debug.Log("Exiting Move State"))
                .Permit(Trigger.StopMoving, State.Idle)
                .Permit(Trigger.Damaged, State.RunAway);

            _stateMachine.Configure(State.RunAway)
                .OnEntry(()=> Debug.Log("Entering Flee State"))
                .OnExit(()=> Debug.Log("Exiting Flee State"))
                .Permit(Trigger.StopMoving, State.Idle)
                .Permit(Trigger.StartMoving, State.Move);
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
