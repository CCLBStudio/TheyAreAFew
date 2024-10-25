using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "NewInputReader", menuName = "CCLBStudio/Inputs/InputReader")]
public class InputReader : ScriptableObject, PlayerControls.IPlayerActions, ISerializationCallbackReceiver
{
    [SerializeField] private bool inputListeningRequested;
    [SerializeField] private bool autoInit = true;
    
    public event UnityAction<Vector2> MoveEvent;
    public event UnityAction<Vector2> AimEvent;
    public event UnityAction JumpBeginEvent;
    public event UnityAction JumpReleaseEvent;
    
    private event UnityAction OnValidateEvent;
    private event UnityAction OnCancelEvent;

    [NonSerialized] private PlayerControls _playerInputs;

    private void OnEnable()
    {
        if (autoInit)
        {
            Init();
        }
    }

    public void Init()
    {
        if (_playerInputs != null)
        {
            return;
        }
        
        _playerInputs = new PlayerControls();
        _playerInputs.Player.SetCallbacks(this);
        _playerInputs.devices = null;
        inputListeningRequested = true;

        _playerInputs.Player.Enable();
        _playerInputs.UI.Disable();
    }
    
    public void SetDevice(InputDevice device)
    {
        if (device == null)
        {
            return;
        }
        
        _playerInputs.devices = new[] {device};
        inputListeningRequested = false;
    }

    public bool IsInputListeningRequested()
    {
        return inputListeningRequested;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            MoveEvent?.Invoke(context.ReadValue<Vector2>());
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            MoveEvent?.Invoke(Vector2.zero);
        }
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            AimEvent?.Invoke(context.ReadValue<Vector2>());
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            AimEvent?.Invoke(Vector2.zero);
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            JumpBeginEvent?.Invoke();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            JumpReleaseEvent?.Invoke();
        }
    }


    public void OnAccept(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            OnValidateEvent?.Invoke();
        }
    }

    public void OnCancel(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            OnCancelEvent?.Invoke();
        }
    }

    public void EnablePlayerInputs()
    {
        _playerInputs.Player.Enable();
    }
    
    public void DisablePlayerInputs()
    {
        _playerInputs.Player.Disable();
    }

    public void OnBeforeSerialize()
    {
    }

    public void OnAfterDeserialize()
    {
        inputListeningRequested = false;
    }

    public void AddValidateAction(UnityAction action)
    {
        OnValidateEvent = null;
        OnValidateEvent += action;
    }
    
    public void AddCancelAction(UnityAction action)
    {
        OnCancelEvent = null;
        OnCancelEvent += action;
    }

    public void ClearValidateEvent()
    {
        OnValidateEvent = null;
    }
    
    public void ClearCancelEvent()
    {
        OnCancelEvent = null;
    }
    
}
