using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    public AudioSource audioSource;

    public void PlayAttackSound()
    {
        audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.Play();
    }

}
