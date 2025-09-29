using System.Collections;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] GameEvent onAggroPlayer;
    [SerializeField] GameEvent onDeaggroPlayer;
    public EnemyData enemyData;

    public GameObject attackHitbox;

    public TrailRenderer dashTrail;
    private Rigidbody2D rb;
    private Animator animator;
    private Transform playerTransform;
    private enum AIState { Idle, Chasing, Attacking, Dashing }
    private AIState currentState;
    private AIState lastState;

    private Coroutine dashCoroutine;

    private bool isAttacking = false;
    private float currentHealth;
    private float attackTimer;
    private float dashTimer;
    private bool isDead = false;
    private int attackStateHash;
    private bool isAggroed = false;
    private int idleStateHash;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        dashTrail = GetComponent<TrailRenderer>();

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        currentHealth = enemyData.health;

        attackStateHash = Animator.StringToHash("boss-attack");
        idleStateHash = Animator.StringToHash("boss-idle");

        // Start in the Idle state
        currentState = AIState.Idle;
    }

    void Update()
    {
        if (!enemyData.isActive) return;

        if (isDead) return;

        if (attackTimer > 0) attackTimer -= Time.deltaTime;
        if (dashTimer > 0) dashTimer -= Time.deltaTime;

        if (lastState != currentState)
        {
            if (currentState != AIState.Idle && !isAggroed)
            {
                isAggroed = true;
                onAggroPlayer.Raise();
            }
            else if (currentState == AIState.Idle && isAggroed)
            {
                isAggroed = false;
                onDeaggroPlayer.Raise();
            }

            lastState = currentState;
        }
        switch (currentState)
        {
            case AIState.Idle:
                IdleState();
                break;
            case AIState.Chasing:
                ChaseState();
                break;
            case AIState.Attacking:
                AttackState();
                break;
            case AIState.Dashing:
                break;
        }
    }

    private void IdleState()
    {
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        animator.SetBool("isWalking", false);

        if (Vector2.Distance(transform.position, playerTransform.position) < enemyData.detectionRange)
        {
            currentState = AIState.Chasing;
        }
    }

    private void ChaseState()
    {
        if (dashTrail != null && dashTrail.emitting)
        {
            StopDashTrail();
        }


        animator.SetBool("isWalking", true);
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer < enemyData.attackRange)
        {
            currentState = AIState.Attacking;
            return;
        }

        if (dashTimer <= 0 && distanceToPlayer > enemyData.minDashRange && distanceToPlayer < enemyData.maxDashRange)
        {
            InitiateDash();
            return;
        }

        if (distanceToPlayer > enemyData.detectionRange)
        {
            currentState = AIState.Idle;
            return;
        }

        Vector2 direction = (playerTransform.position - transform.position).normalized;
        rb.linearVelocity = new Vector2(direction.x * enemyData.moveSpeed, rb.linearVelocity.y);

        if (direction.x > 0)
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else if (direction.x < 0)
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    private void AttackState()
    {
        if (!isAttacking)
        {
            rb.linearVelocity = Vector2.zero;
            animator.SetBool("isWalking", false);
            if (Vector2.Distance(transform.position, playerTransform.position) > enemyData.attackRange)
            {
                currentState = AIState.Chasing;
                return;
            }

            if (attackTimer <= 0)
            {
                animator.SetTrigger("Attack");
                attackTimer = enemyData.attackCooldown;
            }

        }

        else
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.shortNameHash == idleStateHash && stateInfo.normalizedTime >= 0.95f)
            {
                float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
                if (distanceToPlayer > enemyData.attackRange)
                {
                    currentState = AIState.Chasing;
                }
                else if (attackTimer <= 0)
                {
                    animator.SetTrigger("Attack");
                    attackTimer = enemyData.attackCooldown;
                }
            }
        }

        // AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // if (stateInfo.shortNameHash == idleStateHash)
        // {
        //     if (Vector2.Distance(transform.position, playerTransform.position) > enemyData.attackRange)
        //     {
        //         currentState = AIState.Chasing;
        //     }
        // }
    }

    private void InitiateDash()
    {
        currentState = AIState.Dashing;
        animator.SetTrigger("Dash");
        dashCoroutine = StartCoroutine(DashCoroutine());
    }

    private IEnumerator DashCoroutine()
    {
        Vector2 dashDirection = (playerTransform.position - transform.position).normalized;
        if (dashDirection.x > 0)
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else if (dashDirection.x < 0)
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

        float elapsedTime = 0f;
        while (elapsedTime < enemyData.dashDuration)
        {
            rb.linearVelocity = new Vector2(dashDirection.x * enemyData.dashSpeed, rb.linearVelocity.y);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rb.linearVelocity = Vector2.zero;
        dashTimer = enemyData.dashCooldown;
        StopDashTrail();
        currentState = AIState.Chasing;

    }

    public void AttackStart()
    {
        isAttacking = true;
        rb.linearVelocity = Vector2.zero;
    }

    public void AttackEnd()
    {
        isAttacking = false;
    }

    public void EnableAttackHitbox()
    {
        attackHitbox.SetActive(true);
        rb.linearVelocity = Vector2.zero;
    }

    public void DisableAttackHitbox()
    {
        attackHitbox.SetActive(false);
        rb.linearVelocity = Vector2.zero;
    }

    public void StartDashTrail()
    {
        if (dashTrail != null) dashTrail.emitting = true;
    }

    public void StopDashTrail()
    {
        if (dashTrail != null)
        {
            dashTrail.emitting = false;
            dashTrail.Clear();
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        if (currentState == AIState.Dashing)
        {
            StopCoroutine(dashCoroutine);
            dashCoroutine = null;
            currentState = AIState.Chasing;
        }
        StopDashTrail();

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        animator.SetTrigger("Hurt");

    }

    private void Die()
    {
        isDead = true;
        animator.SetTrigger("Death");
        onDeaggroPlayer.Raise();

        rb.linearVelocity = Vector2.zero;
        // GetComponent<Collider2D>().enabled = false;
        // this.enabled = false;
        StartCoroutine(DisableAfterDeath());
    }

    private System.Collections.IEnumerator DisableAfterDeath()
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }



}