namespace Enemy.Interface
{
    public interface IDamageable
    {
        public bool IsDamaged { get; set; }
        public void SetHp(int hp);
        public void TakeDamage(int damage);
        public void Die(); 
    }
}
