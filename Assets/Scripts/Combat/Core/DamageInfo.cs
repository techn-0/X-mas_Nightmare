using UnityEngine;

namespace Game.Combat
{
    /// <summary>
    /// 데미지 정보를 담는 구조체
    /// </summary>
    public struct DamageInfo
    {
        /// <summary>
        /// 데미지 양
        /// </summary>
        public float damage;
        
        /// <summary>
        /// 데미지 타입
        /// </summary>
        public DamageType damageType;
        
        /// <summary>
        /// 피격 위치 (이펙트 생성용)
        /// </summary>
        public Vector3 hitPoint;
        
        /// <summary>
        /// 피격 방향 (넉백용)
        /// </summary>
        public Vector3 hitDirection;
        
        /// <summary>
        /// 공격자 오브젝트
        /// </summary>
        public GameObject attacker;
        
        /// <summary>
        /// DamageInfo 생성자
        /// </summary>
        public DamageInfo(float damage, DamageType damageType, Vector3 hitPoint, Vector3 hitDirection, GameObject attacker)
        {
            this.damage = damage;
            this.damageType = damageType;
            this.hitPoint = hitPoint;
            this.hitDirection = hitDirection;
            this.attacker = attacker;
        }
    }
}

