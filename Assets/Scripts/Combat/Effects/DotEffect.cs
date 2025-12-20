using System.Collections;
using UnityEngine;

namespace Game.Combat
{
    /// <summary>
    /// DoT(Damage over Time) 효과 클래스
    /// 화염방사기 등의 지속 데미지를 처리합니다
    /// </summary>
    public class DotEffect : MonoBehaviour
    {
        [Header("DoT Settings")]
        [SerializeField] private float damagePerTick = 5f;
        [SerializeField] private float tickInterval = 0.5f;
        
        private float remainingDuration = 0f;
        private DamageType damageType;
        private GameObject attacker;
        private IDamageable target;
        private Coroutine dotCoroutine;
        
        /// <summary>
        /// 틱당 데미지 설정
        /// </summary>
        public void SetDamagePerTick(float damage)
        {
            damagePerTick = damage;
        }
        
        /// <summary>
        /// 틱 간격 설정
        /// </summary>
        public void SetTickInterval(float interval)
        {
            tickInterval = interval;
        }
        
        /// <summary>
        /// DoT 효과 적용
        /// </summary>
        /// <param name="target">대상</param>
        /// <param name="duration">지속 시간</param>
        /// <param name="damageType">데미지 타입</param>
        /// <param name="attacker">공격자</param>
        public void ApplyDot(IDamageable target, float duration, DamageType damageType, GameObject attacker)
        {
            this.target = target;
            this.damageType = damageType;
            this.attacker = attacker;
            
            if (dotCoroutine != null)
            {
                // 이미 DoT가 진행 중이면 시간 갱신
                RefreshDuration(duration);
            }
            else
            {
                // 새로운 DoT 시작
                remainingDuration = duration;
                dotCoroutine = StartCoroutine(DotCoroutine());
            }
        }
        
        /// <summary>
        /// DoT 지속 시간 갱신
        /// </summary>
        public void RefreshDuration(float duration)
        {
            remainingDuration = duration;
        }
        
        /// <summary>
        /// DoT 중지
        /// </summary>
        public void StopDot()
        {
            if (dotCoroutine != null)
            {
                StopCoroutine(dotCoroutine);
                dotCoroutine = null;
            }
            
            remainingDuration = 0f;
        }
        
        /// <summary>
        /// DoT 틱 처리 코루틴
        /// </summary>
        private IEnumerator DotCoroutine()
        {
            while (remainingDuration > 0f)
            {
                // 대상이 사망했거나 null이면 중지
                if (target == null || target.IsDead)
                {
                    StopDot();
                    yield break;
                }
                
                // 데미지 적용
                MonoBehaviour targetMono = target as MonoBehaviour;
                if (targetMono != null)
                {
                    DamageInfo dotDamage = new DamageInfo(
                        damagePerTick,
                        damageType,
                        targetMono.transform.position,
                        Vector3.zero,
                        attacker
                    );
                    
                    target.TakeDamage(dotDamage);
                }
                
                yield return new WaitForSeconds(tickInterval);
                remainingDuration -= tickInterval;
            }
            
            // DoT 종료
            dotCoroutine = null;
            Destroy(gameObject);
        }
        
        private void OnDestroy()
        {
            if (dotCoroutine != null)
            {
                StopCoroutine(dotCoroutine);
                dotCoroutine = null;
            }
        }
    }
}

