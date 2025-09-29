using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public PlayerData playerData;
    public Transform playerTransform;
    public BoxCollider2D playerCollider;
    public LayerMask groundLayer;

    public void FixedUpdate()
    {
        float rayLength = 0.1f;

        // Left ray
        Vector3 leftOrigin = new Vector3(
            playerTransform.position.x - (playerCollider.size.x * 2) - playerCollider.offset.x  ,
            playerTransform.position.y - (playerCollider.size.y * 2) + playerCollider.offset.y,
            playerTransform.position.z
        );
        RaycastHit2D leftRayCast = Physics2D.Raycast(leftOrigin, Vector2.down, rayLength, groundLayer);
        Debug.DrawRay(leftOrigin, Vector2.down * rayLength, leftRayCast.collider ? Color.green : Color.red);

        // Right ray
        Vector3 rightOrigin = new Vector3(
            playerTransform.position.x + (playerCollider.size.x * 2) + playerCollider.offset.x,
            playerTransform.position.y - (playerCollider.size.y * 2) + playerCollider.offset.y,
            playerTransform.position.z
        );
        RaycastHit2D rightRayCast = Physics2D.Raycast(rightOrigin, Vector2.down, rayLength, groundLayer);
        Debug.DrawRay(rightOrigin, Vector2.down * rayLength, rightRayCast.collider ? Color.green : Color.red);

        playerData.isGrounded = (leftRayCast || rightRayCast);
        if (playerData.isJumping && playerData.isGrounded)
            playerData.isJumping = false;
    }
}
