using UnityEngine;
using UnityEngine.InputSystem;

public abstract class NPC : MonoBehaviour, IInteractable
{
    [SerializeField] private SpriteRenderer _interactSprite;
    [SerializeField] private Transform _playerTransform;
    public DevConsole devConsole;

    void Start()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Update()
    {
        if (InputSystem.actions.FindAction("Interact").WasPressedThisFrame() && IsWithinInteractDistance())
        {
            Interact();
        }

        if (!IsWithinInteractDistance())
        {
            // turn off sprite
            _interactSprite.enabled = false;
        }
        else if (IsWithinInteractDistance()){
            // turn on sprite
            _interactSprite.enabled = true;
        }
    }

    public abstract void Interact();

    private bool IsWithinInteractDistance()
    {
        return Vector2.Distance(_playerTransform.position, transform.position) < devConsole.npcInteractableRange;
    }
}
