using UnityEngine;

[CreateAssetMenu(fileName = "CompanionStat", menuName = "ScriptableObjects/CompanionStat")]
public class CompanionStat : ScriptableObject
{
    public string CompanionName;

    [Range(1, 100)] public int health;
    [Range(1f, 20f)] public float moveSpeed;
    public float attackPower;

    public WeaponHandler weaponPrefab; //무기 있을경우 연결
}
