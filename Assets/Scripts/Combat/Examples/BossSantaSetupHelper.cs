using UnityEngine;

namespace Combat.Examples
{
    /// <summary>
    /// 보스 산타 설정 예제
    /// 이 스크립트는 에디터에서 자동으로 발사 위치를 생성하는 헬퍼입니다.
    /// </summary>
    public class BossSantaSetupHelper : MonoBehaviour
    {
        [Header("Auto Setup")]
        [SerializeField] private bool autoCreateFirePoints;
        
        [Header("Machine Gun Fire Points")]
        [SerializeField] private Transform leftArmBone; // 왼쪽 팔 본
        [SerializeField] private Transform rightArmBone; // 오른쪽 팔 본
        [SerializeField] private Vector3 armFirePointOffset = new Vector3(0, 0, 1f); // 팔 끝에서의 오프셋
        
        [Header("Missile Fire Points")]
        [SerializeField] private Transform backBone; // 등 본
        [SerializeField] private int missilePointCount = 10;
        [SerializeField] private float missilePointSpacing = 0.3f; // 미사일 발사 위치 간격
        [SerializeField] private Vector3 missileStartOffset = new Vector3(0, 1f, -0.5f); // 첫 미사일 위치 오프셋
        
        [Header("Explosion Points")]
        [SerializeField] private bool autoCreateExplosionPoints;
        [SerializeField] private Transform[] bodyParts; // 몸 부위들
        
        [ContextMenu("Create Fire Points")]
        public void CreateFirePoints()
        {
            if (leftArmBone == null || rightArmBone == null)
            {
                Debug.LogWarning("[Boss Setup] 팔 본이 설정되지 않았습니다!");
                return;
            }
            
            // 왼쪽 팔 발사 위치 생성
            CreateOrUpdateFirePoint("LeftArmFirePoint", leftArmBone, armFirePointOffset);
            
            // 오른쪽 팔 발사 위치 생성
            CreateOrUpdateFirePoint("RightArmFirePoint", rightArmBone, armFirePointOffset);
            
            Debug.Log("[Boss Setup] 머신건 발사 위치 생성 완료!");
        }
        
        [ContextMenu("Create Missile Points")]
        public void CreateMissilePoints()
        {
            if (backBone == null)
            {
                Debug.LogWarning("[Boss Setup] 등 본이 설정되지 않았습니다!");
                return;
            }
            
            // 기존 미사일 포인트 삭제
            Transform existingParent = transform.Find("MissilePoints");
            if (existingParent != null)
            {
                DestroyImmediate(existingParent.gameObject);
            }
            
            // 미사일 포인트 부모 생성
            GameObject missileParent = new GameObject("MissilePoints");
            missileParent.transform.SetParent(backBone);
            missileParent.transform.localPosition = Vector3.zero;
            missileParent.transform.localRotation = Quaternion.identity;
            
            // 미사일 포인트들 생성 (2열로 배치)
            for (int i = 0; i < missilePointCount; i++)
            {
                GameObject point = new GameObject($"MissilePoint_{i + 1:D2}");
                point.transform.SetParent(missileParent.transform);
                
                // 위치 계산 (지그재그 패턴)
                int row = i / 2;
                int col = i % 2;
                
                Vector3 localPos = missileStartOffset;
                localPos.x += (col - 0.5f) * missilePointSpacing; // 좌우
                localPos.y -= row * missilePointSpacing; // 상하
                
                point.transform.localPosition = localPos;
                point.transform.localRotation = Quaternion.identity;
            }
            
            Debug.Log($"[Boss Setup] 미사일 발사 위치 {missilePointCount}개 생성 완료!");
        }
        
        [ContextMenu("Create Explosion Points")]
        public void CreateExplosionPoints()
        {
            if (bodyParts == null || bodyParts.Length == 0)
            {
                Debug.LogWarning("[Boss Setup] 몸 부위가 설정되지 않았습니다!");
                return;
            }
            
            // 기존 폭발 포인트 삭제
            Transform existingParent = transform.Find("ExplosionPoints");
            if (existingParent != null)
            {
                DestroyImmediate(existingParent.gameObject);
            }
            
            // 폭발 포인트 부모 생성
            GameObject explosionParent = new GameObject("ExplosionPoints");
            explosionParent.transform.SetParent(transform);
            explosionParent.transform.localPosition = Vector3.zero;
            explosionParent.transform.localRotation = Quaternion.identity;
            
            // 각 몸 부위에 폭발 포인트 생성
            for (int i = 0; i < bodyParts.Length; i++)
            {
                if (bodyParts[i] == null) continue;
                
                GameObject point = new GameObject($"ExplosionPoint_{i + 1:D2}_{bodyParts[i].name}");
                point.transform.SetParent(explosionParent.transform);
                point.transform.position = bodyParts[i].position;
                point.transform.rotation = Quaternion.identity;
            }
            
            Debug.Log($"[Boss Setup] 폭발 위치 {bodyParts.Length}개 생성 완료!");
        }
        
        private void CreateOrUpdateFirePoint(string pointName, Transform parent, Vector3 offset)
        {
            Transform existingPoint = parent.Find(pointName);
            
            if (existingPoint == null)
            {
                GameObject point = new GameObject(pointName);
                point.transform.SetParent(parent);
                point.transform.localPosition = offset;
                point.transform.localRotation = Quaternion.identity;
            }
            else
            {
                existingPoint.localPosition = offset;
            }
        }
        
        private void OnDrawGizmosSelected()
        {
            // 왼쪽 팔 발사 위치 미리보기
            if (leftArmBone != null)
            {
                Gizmos.color = Color.blue;
                Vector3 leftPos = leftArmBone.position + leftArmBone.TransformDirection(armFirePointOffset);
                Gizmos.DrawWireSphere(leftPos, 0.1f);
                Gizmos.DrawLine(leftArmBone.position, leftPos);
            }
            
            // 오른쪽 팔 발사 위치 미리보기
            if (rightArmBone != null)
            {
                Gizmos.color = Color.cyan;
                Vector3 rightPos = rightArmBone.position + rightArmBone.TransformDirection(armFirePointOffset);
                Gizmos.DrawWireSphere(rightPos, 0.1f);
                Gizmos.DrawLine(rightArmBone.position, rightPos);
            }
            
            // 미사일 발사 위치 미리보기
            if (backBone != null)
            {
                Gizmos.color = Color.red;
                Vector3 startPos = backBone.position + backBone.TransformDirection(missileStartOffset);
                
                for (int i = 0; i < missilePointCount; i++)
                {
                    int row = i / 2;
                    int col = i % 2;
                    
                    Vector3 offset = Vector3.zero;
                    offset.x = (col - 0.5f) * missilePointSpacing;
                    offset.y = -row * missilePointSpacing;
                    
                    Vector3 pos = startPos + backBone.TransformDirection(offset);
                    Gizmos.DrawWireSphere(pos, 0.08f);
                }
            }
            
            // 폭발 위치 미리보기
            if (bodyParts != null)
            {
                Gizmos.color = Color.yellow;
                foreach (Transform part in bodyParts)
                {
                    if (part != null)
                    {
                        Gizmos.DrawWireSphere(part.position, 0.15f);
                    }
                }
            }
        }
    }
}

