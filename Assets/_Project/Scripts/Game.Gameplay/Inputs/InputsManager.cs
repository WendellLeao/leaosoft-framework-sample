using UnityEngine.InputSystem;
using Leaosoft.Events;
using UnityEngine;
using Leaosoft;

namespace Game.Gameplay.Inputs
{
    public sealed class InputsManager : Manager
    {
        [Header("Services")]
        private IEventService _eventService;
        
        [Header("Input System")] 
        private PlayerInputs _playerInputs;
        private PlayerInputs.LandMapActions _landActions;

        [Header("Inputs")]
        private InputsData _inputsData;
        private Vector2 _movement;
        private bool _pressJump;

        public void Initialize(IEventService eventService)
        {
            _eventService = eventService;
            
            base.Initialize();
        }
        
        protected override void OnInitialize()
        {
            base.OnInitialize();
            
            _playerInputs = new PlayerInputs();

            _landActions = _playerInputs.LandMap;

            _playerInputs.Enable();

            SubscribeEvents();
        }

        protected override void OnDispose()
        {
            base.OnDispose();
            
            _playerInputs.Disable();

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
            _eventService.DispatchEvent(new ReadInputsEvent(_inputsData));
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
