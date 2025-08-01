using UnityEngine;

[CreateAssetMenu(fileName = "MonsterStat", menuName = "ScriptableObjects/MonsterStat")]
public class MonsterStat : ScriptableObject
{
    public string monsterName;

    public int health;
    [Range(1f, 20f)] public float moveSpeed;
    public float attackPower;

    public WeaponHandler weaponPrefab; //무기 있을경우 연결
}