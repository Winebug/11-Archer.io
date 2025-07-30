using UnityEngine;
using System.Collections;

public class DamageObstacle : MonoBehaviour
{
    [Header("데미지 설정")]
    [SerializeField] private int damage = 1;
    [SerializeField] private float damageCooldown = 1f;

    private bool canDamage = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canDamage) return;

        if (collision.CompareTag("Player"))
        {
            UnitController player = collision.GetComponent<UnitController>();
            if (player != null)
            {
                player.ChangeHealth(-damage); // 음수로 전달해서 체력 감소
                StartCoroutine(Cooldown());
                Debug.Log("아파요");
            }
        }
    }

    private IEnumerator Cooldown()
    {
        canDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        canDamage = true;
    }
}
