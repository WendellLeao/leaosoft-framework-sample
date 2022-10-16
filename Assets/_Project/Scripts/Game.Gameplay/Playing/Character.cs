using Leaosoft.Events;
using UnityEngine;
using Leaosoft;

namespace Game.Gameplay.Playing
{
    public sealed class Character : Entity
    {
        [Header("Physics")]
        [SerializeField] private Rigidbody2D _rigidBody;
        [SerializeField] private BoxCollider2D _boxCollider;
        
        [Header("Components")]
        [SerializeField] private CharacterMovement _characterMovement;
        [SerializeField] private CharacterJump _characterJump;

        [Header("View")] 
        [SerializeField] private CharacterView _characterView;

        private IEventService _eventService;

        public void Begin(IEventService eventService)
        {
            _eventService = eventService;

            base.Begin();
        }

        protected override void OnBegin()
        {
            base.OnBegin();
            
            _characterView.Setup();
            
            _characterMovement.Begin(_eventService, _rigidBody);
            _characterJump.Begin(_eventService, _rigidBody, _boxCollider);
        }

        protected override void OnStop()
        {
            base.OnStop();
            
            _characterView.Dispose();
            
            _characterMovement.Stop();
            _characterJump.Stop();
        }

        protected override void OnTick(float deltaTime)
        {
            base.OnTick(deltaTime);
            
            _characterView.Tick(deltaTime);
            
            _characterMovement.Tick(deltaTime);
            _characterJump.Tick(deltaTime);
        }

        protected override void OnFixedTick(float fixedDeltaTime)
        {
            base.OnFixedTick(fixedDeltaTime);
            
            _characterMovement.FixedTick(fixedDeltaTime);
            _characterJump.FixedTick(fixedDeltaTime);
        }
    }
}
