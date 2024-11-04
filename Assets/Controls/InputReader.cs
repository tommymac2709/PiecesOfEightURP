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


    public event Action AttackPressed;
    public event Action JumpEvent;
    public event Action DodgeEvent;
    public event Action TargetEvent;
    public event Action CycleTargetEvent;
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

        GetComponent<Stamina>().OnStaminaDepleted += HandleStaminaDepleted;

        WindowController.OnAnyWindowOpened += DisableControls;
        WindowController.OnAllWindowsClosed += EnableControls;
        Health.OnDeathUI += DisableAllControls;

    }

    private void HandleStaminaDepleted()
    {
        IsSprinting = false;
    }

    private void DisableAllControls()
    {
        controls.Player.Disable();
        controls.UI.Disable();
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

        Health.OnDeathUI -= DisableAllControls;
        if (GetComponent<Stamina>() != null)
        {
            GetComponent<Stamina>().OnStaminaDepleted -= HandleStaminaDepleted;
        }

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
        if (GetComponent<Stamina>().GetCurrentStamina() <= 0) { return; }   
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

    public void OnCycleTarget(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }

        CycleTargetEvent?.Invoke();
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
        //if (context.performed) 
        //{ 
        //    IsAttacking = true; 
        //}
        //else if (context.canceled)
        //{
        //    IsAttacking = false;
        //}
        if (!context.performed) { return; }

        AttackPressed?.Invoke();
    }

    public void OnBlock(InputAction.CallbackContext context)
    {
        if (context.performed && GetComponent<Stamina>().GetCurrentStamina() > 0)
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
        if (context.performed && GetComponent<Stamina>().GetCurrentStamina() > 0)
        {
            IsSprinting = true;
            GetComponent<Stamina>().StartSprintDrain();
        }
        else if (context.canceled)
        {
            IsSprinting = false;
            GetComponent<Stamina>().StopSprintDrain();
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
        if (!context.performed)
        {
            return;
            
        }
        InventoryEvent?.Invoke();

    }

    public void OnShop(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
            
        }
        ShopEvent?.Invoke();
    }

    public void OnCancelWindow(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
            
        }
        CancelWindowEvent?.Invoke();

    }

    public void OnUnsheatheWeapon(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
            
            
        }
        SheatheEvent?.Invoke();
    }
}
