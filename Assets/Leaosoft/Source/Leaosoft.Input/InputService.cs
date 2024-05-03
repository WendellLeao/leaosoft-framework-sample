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
        private Vector2 _shoot;
        private bool _pressJump;

        protected override void RegisterService()
        {
            ServiceLocator.RegisterService<IInputService>(this);
        }

        protected override void UnregisterService()
        {
            ServiceLocator.UnregisterService<IInputService>();
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            
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

            _landActions.Shoot.performed += SetPlayerShoot;

            _landActions.Jump.performed += HandlePressJump;
            _landActions.Jump.canceled += HandlePressJump;
        }

        private void UnsubscribeEvents()
        {
            _landActions.Movement.performed -= SetPlayerMovement;
            
            _landActions.Shoot.performed -= SetPlayerShoot;
            
            _landActions.Jump.performed -= HandlePressJump;
            _landActions.Jump.canceled -= HandlePressJump;
        }

        private void UpdateInputsData()
        {
            _inputsData.Movement = _movement;
            _inputsData.Shoot = _shoot;
            
            _inputsData.PressJump = _pressJump;
        }
        
        private void DispatchInputs()
        {
            OnReadInputs?.Invoke(_inputsData);
        }
        
        private void ResetInputs()
        {
            _shoot = Vector2.zero;
            
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
        
        private void SetPlayerShoot(InputAction.CallbackContext action)
        {
            _shoot = action.ReadValue<Vector2>();
        }
    }
}
