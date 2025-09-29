using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    public PlayerMovement playerMovement;
    public PlayerCombat playerCombat;
    public PlayerInput playerInput;
    public PlayerData playerData;
    public void Start()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        playerInput = GetComponent<PlayerInput>();
    }

    public void Update()
    {
        playerCombat.HandleCombat();
        playerMovement.HandleMovement();
    }

    public void FixedUpdate()
    {
        playerData.dashCooldownTimer -= Time.deltaTime;
    }
}
