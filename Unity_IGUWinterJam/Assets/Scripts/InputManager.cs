using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private static InputManager instance;

    void Awake()
    {
        instance = this;
    }


    public Vector2 movementInput;
    public bool interact;
    
    public void OnMovement(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started)
            interact = true;
        else if (context.canceled)
                interact = false;

    }

}
