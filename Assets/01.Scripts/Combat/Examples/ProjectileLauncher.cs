using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Combat.Examples
{
    /// <summary>
    /// 투사체 발사 예시 클래스
    /// 눈사람의 눈덩이 공격 등에 사용
    /// </summary>
    public class ProjectileLauncher : MonoBehaviour
    {
        [Header("Projectile Settings")]
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private Transform firePoint;
        [SerializeField] private float projectileSpeed = 10f;
        
        [Header("Attack Settings")]
        [SerializeField] private float attackCooldown = 1f;
        [SerializeField] private bool autoTarget = true;
        
        [Header("Input (Player용) - New Input System")]
        [SerializeField] private bool useInput = false;
        [SerializeField] private Key shootKey = Key.E;
        
        private float lastShootTime = 0f;
        private Transform target;
        
        private void Update()
        {
            if (useInput && Keyboard.current != null && Keyboard.current[shootKey].wasPressedThisFrame && CanShoot())
            {
                ShootAtDirection(transform.forward);
            }
        }
        
        /// <summary>
        /// 발사 가능 여부 확인
        /// </summary>
        public bool CanShoot()
        {
            return Time.time >= lastShootTime + attackCooldown;
        }
        
        /// <summary>
        /// 특정 방향으로 발사
        /// </summary>
        public void ShootAtDirection(Vector3 direction)
        {
            if (!CanShoot() || projectilePrefab == null)
                return;
            
            lastShootTime = Time.time;
            
            Vector3 spawnPosition = firePoint != null ? firePoint.position : transform.position;
            
            // 투사체 생성
            GameObject projectileObj = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
            Projectile projectile = projectileObj.GetComponent<Projectile>();
            
            if (projectile != null)
            {
                projectile.SetOwner(gameObject);
                projectile.Launch(direction.normalized);
            }
            
            Debug.Log($"{gameObject.name} shot projectile in direction {direction}");
        }
        
        /// <summary>
        /// 특정 타겟을 향해 발사
        /// </summary>
        public void ShootAtTarget(Transform target)
        {
            if (target == null)
                return;
            
            Vector3 direction = (target.position - transform.position).normalized;
            ShootAtDirection(direction);
        }
        
        /// <summary>
        /// 플레이어를 향해 발사 (적 AI용)
        /// </summary>
        public void ShootAtPlayer()
        {
            if (target == null)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    target = player.transform;
                }
            }
            
            if (target != null)
            {
                ShootAtTarget(target);
            }
        }
        
        /// <summary>
        /// 타겟 설정
        /// </summary>
        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
        }
    }
}

