using UnityEngine;
using System.Collections.Generic;
public class PlayerHitbox : MonoBehaviour
{
    [SerializeField] private HitStop hitStop;
    public PlayerData playerData;

    [SerializeField] private LayerMask activeLayerMask;
    [SerializeField] LayerMask enemyLayers;         // who counts as enemy   

    BoxCollider2D hitbox;
    private static readonly Collider2D[] buffer = new Collider2D[32];

    private bool isHitting = false;
    void Awake()
    {
        hitbox = GetComponent<BoxCollider2D>();
    }
    public void CheckImmediateHits()
    {
        isHitting = false;
        Vector2 center = hitbox.bounds.center;
        Vector2 size = hitbox.bounds.size;
        float angle = transform.eulerAngles.z;

        ContactFilter2D filter = new ContactFilter2D
        {
            useLayerMask = true,
            layerMask = enemyLayers,
            useTriggers = true
        };

        // find overlaps with all enemies
        int count = Physics2D.OverlapBox(center, size, angle, filter, buffer);

        // try hit all of them
        for (int i = 0; i < count; i++)
        {
            TryHit(buffer[i], "[Immediate]");
        }
    }

    // to handle cases where the player slides forward while the attack is still ongoing and you want to register that attack
    private void OnTriggerEnter2D(Collider2D other)
    {
        FrameLogger.Log($"[[PLAYER HITBOX] OnTriggerEnter2D] collision with {other.name}");
        if (CanHitTarget(other.gameObject.layer, other.gameObject.name))
        {
            TryHit(other, "[OnTriggerEnter2D]");

        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        FrameLogger.Log($"[[PLAYER HITBOX] OnTriggerExit2D] collision with {other.name}");
    }

    private bool IsHitBoxaActive()
    {
        int activeLayer = (int)Mathf.Log(activeLayerMask.value, 2);
        if (gameObject.layer != activeLayer) return false;
        return true;
    }

    bool CanHitTarget(int targetLayer, string name)
    {
        Debug.Log($"[PLAYER HITBOX[CanHitTarget]] hitting {name} isHitBoxActive {IsHitBoxaActive()} isHitting {isHitting} layermask calc {(enemyLayers.value & (1 << targetLayer))}");
        return IsHitBoxaActive() && !isHitting && (enemyLayers.value & (1 << targetLayer)) != 0;
    }
    void TryHit(Collider2D other, string src)
    {
        FrameLogger.Log($"[[PLAYER HITBOX] TRYHIT] {src}");

        var enemy = other.GetComponent<EnemyAI>();
        if (enemy == null) return;

        enemy.TakeDamage(playerData.damage);
        hitStop.Stop(playerData.hitStopDuration);
        isHitting = true;
    }

}