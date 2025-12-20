namespace Game.Combat
{
    /// <summary>
    /// 데미지를 받을 수 있는 객체가 구현해야 하는 인터페이스
    /// </summary>
    public interface IDamageable
    {
        /// <summary>
        /// 데미지를 받습니다
        /// </summary>
        /// <param name="damageInfo">데미지 정보</param>
        void TakeDamage(DamageInfo damageInfo);
        
        /// <summary>
        /// 현재 체력
        /// </summary>
        float CurrentHP { get; }
        
        /// <summary>
        /// 최대 체력
        /// </summary>
        float MaxHP { get; }
        
        /// <summary>
        /// 사망 여부
        /// </summary>
        bool IsDead { get; }
    }
}

