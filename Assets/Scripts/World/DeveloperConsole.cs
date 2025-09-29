using UnityEngine;

public class DeveloperConsole : MonoBehaviour
{
    public DevConsole devConsole;
    public HitStop hitStop;
    // Update is called once per frame
    void Update()
    {
        if (!devConsole.cheatsOn) return;
        if (devConsole.overrideTime)
        {
            if (!hitStop.isWaiting) Time.timeScale = devConsole.timeScale;
        }
        Application.targetFrameRate = devConsole.targetFrameRate;
    }
}
