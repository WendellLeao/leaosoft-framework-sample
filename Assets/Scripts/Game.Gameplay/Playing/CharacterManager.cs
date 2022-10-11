using Leaosoft.Services;
using Leaosoft.Input;
using UnityEngine;
using Leaosoft;

namespace Game.Gameplay.Playing
{
    public sealed class CharacterManager : Manager
    {
        [SerializeField] private Character _character;
        
        protected override void OnInitialize()
        {
            base.OnInitialize();

            IInputService inputService = ServiceLocator.GetService<IInputService>();

            _character.Begin(inputService);
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
