using Game.Gameplay.Inputs;
using Leaosoft.Events;
using UnityEngine;
using Leaosoft;

namespace Game.Gameplay.Playing
{
    public sealed class CharacterMovement : EntityComponent
    {
        [SerializeField] private float _moveSpeed;
        
        private IEventService _eventService;
        private Rigidbody2D _rigidBody;
        private Vector2 _movement;

        public void Begin(IEventService eventService, Rigidbody2D rigidBody)
        {
            _eventService = eventService;
            _rigidBody = rigidBody;
            
            base.Begin();
        }

        protected override void OnBegin()
        {
            base.OnBegin();
            
            _eventService.AddEventListener<ReadInputsEvent>(HandleReadInputs);
        }

        protected override void OnStop()
        {
            base.OnStop();
            
            _eventService.RemoveEventListener<ReadInputsEvent>(HandleReadInputs);
        }

        protected override void OnFixedTick(float fixedDeltaTime)
        {
            base.OnFixedTick(fixedDeltaTime);
            
            float horizontalMovement = _movement.x * _moveSpeed;
            float verticalMovement = _rigidBody.velocity.y;
            
            _rigidBody.velocity = new Vector2(horizontalMovement, verticalMovement);
        }

        private void HandleReadInputs(ServiceEvent serviceEvent)
        {
            if (serviceEvent is ReadInputsEvent readInputsEvent)
            {
                InputsData inputsData = readInputsEvent.InputsData;
                
                _movement = inputsData.Movement;
            }
        }
    }
}
