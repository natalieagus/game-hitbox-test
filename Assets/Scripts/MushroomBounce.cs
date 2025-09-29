using UnityEngine;
public class BouncyMushroom : MonoBehaviour
{
    public float bounceForce = 25f;
    public AudioClip bounceSound;
    private AudioSource audioSource;

    private Animator animator;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {

                Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();

                if (playerRb != null)
                {
                    playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, bounceForce);
                    audioSource.PlayOneShot(bounceSound);

                }

                if (animator != null)
                {
                    animator.SetTrigger("Bounce");
                }

                return;
            }
        }
    }
}