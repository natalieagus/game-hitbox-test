using UnityEngine;
using Unity.Cinemachine;

public class SecretRoom1Camera : MonoBehaviour
{
    public CinemachineCamera leftCamera;
    public CinemachineCamera rightCamera;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // From left of collider to right
            if (collision.transform.position.x > transform.position.x)
            {
                rightCamera.Priority = 10;
                leftCamera.Priority = 5;
            }
            else
            {
                leftCamera.Priority = 10;
                rightCamera.Priority = 5;
            }
        }
        
    }
}
