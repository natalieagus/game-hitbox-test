using System.Collections;
using UnityEngine;

public class HitStop : MonoBehaviour
{
    public bool isWaiting = false;
    public void Stop(float duration_seconds)
    {
        if (isWaiting) return;
        StartCoroutine(WaitAsync(duration_seconds));
    }

    public IEnumerator WaitAsync(float duration_seconds)
    {
        isWaiting = true;
        Time.timeScale = 0f;
        yield return null;
        yield return new WaitForSecondsRealtime(duration_seconds);
        Time.timeScale = 1f;
        isWaiting = false ;
    }
}
