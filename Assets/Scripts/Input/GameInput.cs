using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    private PlayerInputActions _playerInputActions;
    public event Action OnInteract;
    private void Awake()
    {
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Player.Enable();
        _playerInputActions.Player.Interact.performed += Interact_performed;
    }
    
    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Debug.Log(obj);
        OnInteract?.Invoke();
    }
    
    public Vector2 GetMovementVectorNormalized()
    {
        return _playerInputActions.Player.Move.ReadValue<Vector2>().normalized;
    }
}
