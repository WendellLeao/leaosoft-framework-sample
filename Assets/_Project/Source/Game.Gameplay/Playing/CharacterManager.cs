using Leaosoft.Events;
using Leaosoft.Input;
using UnityEngine;
using Leaosoft;

namespace Game.Gameplay.Playing
{
    public sealed class CharacterManager : Manager
    {
        [SerializeField] private Character _character;
        
        private IInputService _inputService;
        private IEventService _eventService;

        public void Initialize(IInputService inputService, IEventService eventService)
        {
            _inputService = inputService;
            _eventService = eventService;
            
            base.Initialize();
        }
        
        protected override void OnInitialize()
        {
            base.OnInitialize();

            _character.Begin(_inputService);
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
