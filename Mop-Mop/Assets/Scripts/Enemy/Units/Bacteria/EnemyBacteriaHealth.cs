using System.Threading;
using Cysharp.Threading.Tasks;
using Interface;
using UnityEngine;

namespace Enemy.Units.Bacteria
{
    public class EnemyBacteriaHealth : MonoBehaviour, IDamageable
    {
        public bool IsDamaged { get; set; }
        private int _currentHp;
        public void Awake()
        { 
            _colorChangeCts = new CancellationTokenSource();
            IsDamaged = false;
        }
        
        public void TakeDamage(int damage)
        {
            _currentHp -= damage;
           
            IsDamaged = true;
            _colorChangeCts.Cancel();  // 이전에 실행 중인 작업을 취소
            _colorChangeCts = new CancellationTokenSource();  // 새로운 취소 토큰 소스 생성
            
            ChangeColorAsync(Color.red,  _colorChangeCts.Token).Forget();  // Forget 메서드로 예외 처리
            Debug.Log(name + " took " + damage + " damage. Remaining Health: " + _currentHp);

            if (!(_currentHp <= 0)) return;
            
            _colorChangeCts.Cancel();
            Die();
        }

        public void Die()
        {
            Destroy(gameObject);
        }
        
        public void SetHp(int hp)
        {
            _currentHp = hp;
        }


#if UNITY_EDITOR
        public Renderer enemyRenderer;
        private CancellationTokenSource _colorChangeCts;  // 취소 토큰 소스 추가
        private async UniTask ChangeColorAsync(Color color, CancellationToken token = default)
        {
            enemyRenderer.material.color = color;
            await UniTask.WaitForSeconds(0.05f, cancellationToken: token);
            enemyRenderer.material.color = Color.white;
        }
#endif
    }
}
