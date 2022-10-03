using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PInput : MonoBehaviour
{
   PControls pInput;
   PMove playerMove;


    // Start is called before the first frame update
    void Awake()
    {
        pInput = new PControls();
        playerMove = GetComponent<PMove>();
        InitializeInputActions();
    }

    private void OnEnable()
    {
        pInput.Enable();
    }
    
    private void OnDisable()
    {
        pInput.Disable();
    }

void InitializeInputActions()
{
    pInput.Player.Move.performed += ctx => playerMove.GetMovementDirection(ctx.ReadValue<float>());
    pInput.Player.Jump.performed += ctx => playerMove.Jump();
} 
}
