using UnityEngine;

[CreateAssetMenu]
public class DevConsole : ScriptableObject
{
    public bool cheatsOn = false;
    public bool overrideTime = false;
    public float timeScale = 1.0f;
    public int targetFrameRate = 60;
    public float npcInteractableRange = 5f;
}
