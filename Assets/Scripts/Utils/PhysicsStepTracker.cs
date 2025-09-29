using UnityEngine;

public class PhysicsStepTracker : MonoBehaviour
{
    public static int fixedStepCount = 0;

    void FixedUpdate()
    {
        fixedStepCount++;
    }
}
