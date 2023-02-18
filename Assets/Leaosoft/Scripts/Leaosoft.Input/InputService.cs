using UnityEngine.InputSystem;
using Leaosoft.Services;
using UnityEngine;
using System;

namespace Leaosoft.Input
{
    /// <summary>
    /// The InputService provides the abstraction <see cref="IInputService"/> to expose all the players inputs.
    /// <seealso cref="ServiceLocator"/>
    /// </summary>

    [DisallowMultipleComponent]
    public sealed class InputService : GameService, IInputService
    {
        public event Action<InputsData> OnReadInputs;

        [Header("Input System")] 
        private Inputs _inputs;
        private Inputs.LandMapActions _landActions;
        private InputsData _inputsData;

        [Header("Inputs")] 
        private Vector2 _movement;
        private bool _pressJump;

        public void RegisterService()
        {
            ServiceLocator.RegisterService<IInputService>(this);
        }

        public void UnregisterService()
        {
            ServiceLocator.DeregisterService<IInputService>();
        }

        public void Initialize()
        {
            _inputs = new Inputs();

            _landActions = _inputs.LandMap;

            _inputs.Enable();

            SubscribeEvents();
        }

        protected override void OnDispose()
        {
            base.OnDispose();
            
            _inputs.Disable();

            UnsubscribeEvents();
        }

        protected override void OnTick(float deltaTime)
        {
            base.OnTick(deltaTime);
            
            UpdateInputsData();

            DispatchInputs();

            ResetInputs();
        }

        private void SubscribeEvents()
        {
            _landActions.Movement.performed += SetPlayerMovement;

            _landActions.Jump.performed += HandlePressJump;
            _landActions.Jump.canceled += HandlePressJump;
        }

        private void UnsubscribeEvents()
        {
            _landActions.Movement.performed -= SetPlayerMovement;
            
            _landActions.Jump.performed -= HandlePressJump;
            _landActions.Jump.canceled -= HandlePressJump;
        }

        private void UpdateInputsData()
        {
            _inputsData.Movement = _movement;
            
            _inputsData.PressJump = _pressJump;
        }
        
        private void DispatchInputs()
        {
            OnReadInputs?.Invoke(_inputsData);
        }
        
        private void ResetInputs()
        {
            _pressJump = false;
        }

        private void HandlePressJump(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Performed:
                {
                    _pressJump = true;
                    
                    break;
                }
                case InputActionPhase.Canceled:
                {
                    _pressJump = false;
                    
                    break;
                }
            }
        }

        private void SetPlayerMovement(InputAction.CallbackContext action)
        {
            _movement = action.ReadValue<Vector2>();
        }
    }
}
