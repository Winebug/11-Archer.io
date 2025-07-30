using UnityEngine;

public class NonDamageObstacle : MonoBehaviour
{
    // 단순히 이동만 막는 오브젝트
    // Collider2D의 isTrigger = false로 설정하면 별도 코드 없이 물리적으로 막힘

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            // 필요하면 여기서 사운드 효과나 파티클 재생 가능
            Debug.Log("플레이어가 NonDamageObstacle에 막힘");
        }
    }
}
