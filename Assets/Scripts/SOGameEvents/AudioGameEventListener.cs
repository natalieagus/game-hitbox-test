using UnityEngine;
using UnityEngine.Events;

// if attached to an object that might be disabled, callback will not work
// attach it on a parent object that wont be disabled
public class AudioGameEventListener : GameEventListener<AudioClip> {}
