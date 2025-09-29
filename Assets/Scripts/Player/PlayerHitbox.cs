using UnityEngine;

public class PlayerHitbox : MonoBehaviour
{
    [SerializeField] private HitStop hitStop;
    public PlayerData playerData;
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"[PLAYER HITBOX] A collider has {other.name} entered the Player trigger");
        if (other.CompareTag("Enemy"))
        {
            EnemyAI enemy = other.GetComponent<EnemyAI>();
            if (enemy != null)
            {
                hitStop.Stop(playerData.hitStopDuration);
                enemy.TakeDamage(playerData.damage);
            }
        }
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log($"[PLAYER HITBOX] A collider {other.name} is inside the Player trigger");
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log($"[PLAYER HITBOX] A collider has {other.name} exited the Player trigger");
    }
}