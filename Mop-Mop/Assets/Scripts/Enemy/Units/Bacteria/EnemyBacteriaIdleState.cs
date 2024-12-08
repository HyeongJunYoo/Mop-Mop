using Interface;

namespace Enemy.Units.Bacteria
{
    public class EnemyBacteriaIdleState : IState
    {
        private readonly EnemyBacteriaController _enemy;

        public EnemyBacteriaIdleState(BaseEnemy enemy)
        {
            _enemy = (EnemyBacteriaController)enemy;
        }

        public void Enter()
        {

        }

        public void Update()
        {
            
        }

        public void FixedUpdate()
        {
            
        }

        public void Exit()
        {
            
        }
    }
}
