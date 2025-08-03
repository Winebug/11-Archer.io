using UnityEngine;

public class PunchTrigger : MonoBehaviour
{
    private PunchWeaponHandler punch;

    private void Awake()
    {
        punch = GetComponentInParent<PunchWeaponHandler>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        punch.PunchProcess(other);
    }

    public void TurnOnCollider()
    {
        punch.TurnOnCollider();
    }

    public void TurnOffCollider()
    {
        punch.TurnOffCollider();
    }
}
