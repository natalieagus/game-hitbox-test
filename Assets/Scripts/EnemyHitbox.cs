using UnityEngine;

public class EnemyHitbox : MonoBehaviour
{
    public EnemyData enemyData;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"[ENEMY HITBOX] A collider has {other.name} entered the Enemy trigger with tag {other.tag}");
        if (other.CompareTag("Player"))
        {
            PlayerCombat player = other.GetComponent<PlayerCombat>();
            if (player != null)
            {
                player.TakeDamage(enemyData.attackDamage);

                gameObject.SetActive(false);
            }
        }
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log($"[ENEMY HITBOX] A collider {other.name} is inside the Enemy trigger with tag {other.tag}");
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log($"[ENEMY HITBOX] A collider has {other.name} exited the Enemy trigger with tag {other.tag}");
    }
}