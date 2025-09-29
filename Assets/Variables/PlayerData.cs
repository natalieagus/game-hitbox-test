using System;
using UnityEngine;

[CreateAssetMenu]
public class PlayerData: ScriptableObject
{
    // Movement
    public float targetSpeed = 5;
    public float maxSpeed = 5;
    public float jumpHangTimeThreshold = 0.1f;
    public float jumpHangGravityScale = 1f;
    public float fallingGravityScale = 5;
    public float defaultGravityScale = 3;
    public float dashDuration = 0.2f;
    public float dashDelay = 0.2f;
    public float dashCooldown = 1f;
    public float dashCooldownTimer = -1f;
    public float dashForce = 33f;
    public float jumpForce = 10f;
    public float maxFallSpeed = 20f;

    // Combat
    public float hp = 20;
    public float damage = 10;
    public float hitStopDuration = 0.2f;
    public float attackHitboxDelay = 0.2f;
    public float attackHitboxDuration = 0.2f;

    // States
    public bool isJumping = false;
    public bool isDashing = false;
    public bool isGrounded = true;
    public bool isInvincible = false;
    public bool isMovementDisabled = false;
    public bool isAggroed = false;
}
