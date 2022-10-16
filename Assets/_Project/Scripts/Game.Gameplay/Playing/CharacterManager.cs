using Leaosoft.Events;
using UnityEngine;
using Leaosoft;

namespace Game.Gameplay.Playing
{
    public sealed class CharacterManager : Manager
    {
        [SerializeField] private Character _character;
        
        private IEventService _eventService;

        public void Initialize(IEventService eventService)
        {
            _eventService = eventService;
            
            base.Initialize();
        }
        
        protected override void OnInitialize()
        {
            base.OnInitialize();

            _character.Begin(_eventService);
        }

        protected override void OnDispose()
        {
            base.OnDispose();
            
            _character.Stop();
        }

        protected override void OnTick(float deltaTime)
        {
            base.OnTick(deltaTime);
            
            _character.Tick(deltaTime);
        }

        protected override void OnFixedTick(float fixedDeltaTime)
        {
            base.OnFixedTick(fixedDeltaTime);
            
            _character.FixedTick(fixedDeltaTime);
        }
    }
}
