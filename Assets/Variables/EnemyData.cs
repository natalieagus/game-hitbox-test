using UnityEngine;

[CreateAssetMenu]
public class EnemyData : ScriptableObject
{
    public bool isActive = true;
    public float health = 100f;
    public float moveSpeed = 3f;
    public float attackDamage = 10f;
    public float attackRange = 1.5f;
    public float attackCooldown = 2f;
    public float detectionRange = 10f;

    public float dashSpeed = 25f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 5f;
    public float minDashRange = 4f;
    public float maxDashRange = 12f;


}