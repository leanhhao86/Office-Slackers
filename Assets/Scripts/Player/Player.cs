using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;
    private Vector3 _moveDirection;
    public bool IsWalking => _moveDirection != Vector3.zero;
    private PlayerInputActions _playerInputActions;
    private Vector3 _lastMoveDirection;

    private void Start()
    {
        gameInput.OnInteract += HandleInteractions;
    }
    private void Update()
    {
        HandleMovement();
    }
    
    private void HandleMovement()
    {
        Vector2 movementVector = gameInput.GetMovementVectorNormalized();
        _moveDirection = new Vector3(movementVector.x, 0f, movementVector.y);
        float playerSize = 0.7f;
        float playerHeight = 2f;
        float moveDistance = Time.deltaTime * moveSpeed;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerSize, _moveDirection, moveDistance);
        
        // Resolve diagonal movement
        if (!canMove)
        {
            Vector3 direction = _moveDirection;
            Vector3 testDirection = new Vector3(direction.x, 0f, 0f).normalized;
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerSize, testDirection, moveDistance);
            if (canMove)
            {
                _moveDirection = testDirection;
            }
            else
            {
                testDirection = new Vector3(0f, 0f, direction.z).normalized;
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerSize, testDirection, moveDistance);
                if (canMove)
                {
                    _moveDirection = testDirection;
                }
            }
        }

        _lastMoveDirection = _moveDirection != Vector3.zero? _moveDirection : _lastMoveDirection;
        
        if (canMove)
        {
            transform.position += moveDistance * _moveDirection;
        }
        
        // Rotate over time
        transform.forward = Vector3.Slerp(transform.forward, _moveDirection, 10f * Time.deltaTime);
    }

    private void HandleInteractions()
    {
        float interactionDisntace = 2f;
        if (Physics.Raycast(transform.position, _lastMoveDirection, out RaycastHit hit, interactionDisntace, countersLayerMask))
        {
            if (hit.transform.TryGetComponent(out ClearCounter clearCounter))
            {
                clearCounter.Interact();
            }
        }
    }
}
