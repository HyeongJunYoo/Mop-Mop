using System;
using Cysharp.Threading.Tasks;
using Interface;
using UnityEngine;

namespace Player
{
    public class PlayerAttackState : IState
    {
        private PlayerController _player;
        public float AttackCooldown { get; private set; }
        public float ReloadTime { get; private set; }
        public int Damage { get; private set; }
        
        public int TotalAmmo { get; private set; }
        public int CurrentAmmo { get; private set; }

        public bool IsAttacking { get; private set; }
        public bool IsReloading { get; private set; }

        public PlayerAttackState(PlayerController player)
        {
            _player = player;
            
            AttackCooldown = 1f;
            ReloadTime = 2.0f;
            Damage = 10;
            TotalAmmo = 10;
            CurrentAmmo = TotalAmmo;
            
            IsAttacking = false;
            IsReloading = false;
        }
        
        public void Enter()
        {
            _player.ChangeColor(Color.red);
        }

        public void Update()
        {
            Attack(_player.enemyDetector.GetClosestEnemy());
        }

        public void FixedUpdate()
        {
            
        }

        public void Exit()
        {
            
        }


        private void Attack(GameObject target)
        {
            if (target == null) return;
            
            // 공격 중이거나 재장전 중이면 공격하지 않음
            if (IsAttacking || IsReloading) return;
            
            // 총알이 남아있으면 공격, 없으면 재장전
            if (CurrentAmmo > 0)
            {
                TryAttackAsync(target).Forget();
            }
            else
            {
                TryReloadAsync().Forget();
            }
        }
        
        private async UniTask TryAttackAsync(GameObject enemy)
        {
            IsAttacking = true;
            CurrentAmmo--;
            Debug.Log($"Attacking - Ammo Remaining: {CurrentAmmo} / {TotalAmmo}");
            
            // 적에게 데미지 입히기
            if (enemy.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.TakeDamage(Damage);
            }else
            {
                Debug.LogWarning("Enemy does not have IDamageable component!");
            }
            
            // 발사 후 쿨다운 대기
            await UniTask.Delay(TimeSpan.FromSeconds(AttackCooldown));
            IsAttacking = false;

            // 총알이 다 떨어졌으면 재장전
            if (CurrentAmmo <= 0)
            {
                await TryReloadAsync();
            }
        }

        private async UniTask TryReloadAsync()
        {
            IsReloading = true;
            Debug.Log("Reloading...");

            // 장전 시간 대기
            await UniTask.Delay(TimeSpan.FromSeconds(ReloadTime));

            // 총알 재장전
            CurrentAmmo = TotalAmmo;
            Debug.Log("Reload Complete!");
            IsReloading = false;
        }
    }
}
