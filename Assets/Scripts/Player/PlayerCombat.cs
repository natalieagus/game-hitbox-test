using System.Collections;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;
    public Collider2D attackHitbox;
    public PlayerData playerData;
    public PlayerInput input;
    public GameEvent attackEvent;
    [SerializeField] private HitStop hitStop;

    public void HandleCombat()
    {
        if (isDead())
        {
            // Play death animation + ui screen
        }
        if (input.attackInput.WasPressedThisFrame())
        {
            Attack();
        }
    }
    public bool isDead()
    {
        return playerData.hp < 0;
    }

    public void TakeDamage(float damage)
    {
        if (playerData.isInvincible) return;
        playerData.hp -= damage;
    }

    public void Heal(float health)
    {
        playerData.hp += health;
    }

    public void Attack()
    {
        if (playerData.isDashing) return;
        animator.SetTrigger("Attack");
        attackEvent.Raise();
    }

    public void EnableAttackHitboxWindow()
    {
        attackHitbox.enabled = true;
        CheckEnemyOverlapAfterHitboxIsEnabledManually();
    }

    public void CheckEnemyOverlapAfterHitboxIsEnabledManually()
    {
        var results = new Collider2D[10];
        int count = attackHitbox.Overlap(ContactFilter2D.noFilter, results);
        if (count > 0)
        {
            foreach (Collider2D collider in results)
            {
                if (collider && collider.CompareTag("Enemy"))
                {
                    EnemyAI enemy = collider.GetComponent<EnemyAI>();
                    if (enemy != null)
                    {
                        hitStop.Stop(playerData.hitStopDuration);
                        enemy.TakeDamage(playerData.damage);
                    }
                }
            }
        }
    }

    public void DisableAttackHitBoxWindow()
    {
        attackHitbox.enabled = false;
    }


}
