using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, Controls.IPlayerActions, Controls.IUIActions
{
    public Vector2 MovementValue { get; private set; }

    public bool IsAttacking { get; private set; }
    public bool IsBlocking { get; private set; }

    public bool IsSprinting { get; private set; }

    public bool IsTargeting;

      

    public event Action JumpEvent;
    public event Action DodgeEvent;
    public event Action TargetEvent;
    public event Action SheatheEvent;
    public event Action UseAbilityEvent;
    public event Action CancelTargetEvent;
    public event Action InteractEvent;
    public event Action CancelWindowEvent;
    public event Action InventoryEvent;
    public event Action ShopEvent;
    public static event Action SaveGameEvent;
    public static event Action LoadGameEvent;
    public static event Action DeleteSaveFileEvent;

    private Controls controls;



    private void Start()
    {
        controls = new Controls();
        controls.Player.SetCallbacks(this);
        controls.UI.SetCallbacks(this);

        controls.Player.Enable();
        controls.UI.Enable();

        WindowController.OnAnyWindowOpened += DisableControls;
        WindowController.OnAllWindowsClosed += EnableControls;

    }

    void EnableControls()
    {
        controls.Player.Enable();
    }

    void DisableControls()
    {
        controls.Player.Disable();
    }


    private void OnDestroy()
    {
        WindowController.OnAnyWindowOpened -= DisableControls;
        WindowController.OnAllWindowsClosed -= EnableControls;

        controls.Player.Disable();
        controls.UI.Disable();
    }

    public void OnSaveGame(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }

        SaveGameEvent?.Invoke();
    }

    public void OnLoadGame(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }

        LoadGameEvent?.Invoke();
    }

    public void OnDeleteSave(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }

        DeleteSaveFileEvent?.Invoke();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }

        JumpEvent?.Invoke();
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }

        DodgeEvent?.Invoke();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MovementValue = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {

    }

    public void OnTarget(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }

        TargetEvent?.Invoke();
    }

    public void OnCancelTarget(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }

        CancelTargetEvent?.Invoke();
    }
    
    /*
    Using a bool allows attacks to be done by pressing and holding
    Use an event for individual button presses
    */
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed) 
        { 
            IsAttacking = true; 
        }
        else if (context.canceled)
        {
            IsAttacking = false;
        }
    }

    public void OnBlock(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            IsBlocking = true;
        }
        else if (context.canceled)
        {
            IsBlocking = false;
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            IsSprinting = true;
        }
        else if (context.canceled)
        {
            IsSprinting = false;
        }
    }

    public void OnUseAbility(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }

        UseAbilityEvent?.Invoke();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }

        InteractEvent?.Invoke();
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            InventoryEvent?.Invoke();
        }

    }

    public void OnShop(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ShopEvent?.Invoke();
        }

    }

    public void OnCancelWindow(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            CancelWindowEvent?.Invoke();
        }

    }

    public void OnUnsheatheWeapon(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SheatheEvent?.Invoke();
            
        }
    }
}
