using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public AudioSource calmBgm;
    public AudioSource actionBgm;

    [SerializeField] private float crossFadeDuration = 2f;

    private void Start()
    {
        calmBgm.Play();
        calmBgm.Pause();
        actionBgm.Play();
        actionBgm.Pause();
        if (calmBgm != null) calmBgm.Play();
    }

    public void OnAggro()
    {
        CrossFade(calmBgm, actionBgm, crossFadeDuration);
    }

    public void OnDeAggro()
    {
        CrossFade(actionBgm, calmBgm, crossFadeDuration);
    }

    private void CrossFade(AudioSource fadeOutSource, AudioSource fadeInSource, float crossFadeSeconds)
    {
        StartCoroutine(CrossFadeCoroutine(fadeOutSource, fadeInSource, crossFadeSeconds));
    }

    private System.Collections.IEnumerator CrossFadeCoroutine(AudioSource fadeOutSource, AudioSource fadeInSource, float duration)
    {
        if (fadeOutSource == null || fadeInSource == null) yield break;

        fadeInSource.volume = 0f;
        if (!fadeInSource.isPlaying) fadeInSource.Play();

        float time = 0f;
        float startVolOut = fadeOutSource.volume;
        float startVolIn = fadeInSource.volume;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            fadeOutSource.volume = Mathf.Lerp(startVolOut, 0f, t);
            fadeInSource.volume = Mathf.Lerp(startVolIn, 0.8f, t);

            yield return null;
        }

        fadeOutSource.Stop();
        fadeOutSource.volume = startVolOut; // reset for future use
        fadeInSource.volume = 0.8f;
    }
}