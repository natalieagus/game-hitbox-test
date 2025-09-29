using UnityEngine;

public static class FrameLogger
{
    public static void Log(string context)
    {
        Debug.Log(
            $"[{context}] " +
            $"Frame: {Time.frameCount}, " +
            $"deltaTime: {Time.deltaTime:F4}, " +
            $"fixedDeltaTime: {Time.fixedDeltaTime:F4}, " +
            $"time: {Time.time:F4}, " +
            $"PhysicsStep={PhysicsStepTracker.fixedStepCount}, "
        );
    }
}
