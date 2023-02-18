using Game.Gameplay.Playing;
using Leaosoft.Services;
using Leaosoft.Events;
using Leaosoft.Audio;
using Leaosoft.Input;
using UnityEngine;

namespace Game.Gameplay
{
    public sealed class GameplaySystem : Leaosoft.System
    {
        [SerializeField] private CharacterManager _characterManager;

        [Header("Audio")] 
        [SerializeField] private AudioData _gameThemeAudio;
        
        protected override void OnInitialize()
        {
            base.OnInitialize();

            IInputService inputService = ServiceLocator.GetService<IInputService>();
            IEventService eventService = ServiceLocator.GetService<IEventService>();
            
            _characterManager.Initialize(inputService, eventService);

            PlayGameTheme();
        }

        protected override void OnDispose()
        {
            base.OnDispose();
            
            _characterManager.Dispose();
        }

        protected override void OnTick(float deltaTime)
        {
            base.OnTick(deltaTime);
            
            _characterManager.Tick(deltaTime);
        }

        protected override void OnFixedTick(float fixedDeltaTime)
        {
            base.OnFixedTick(fixedDeltaTime);
            
            _characterManager.FixedTick(fixedDeltaTime);
        }

        private void PlayGameTheme()
        {
            IAudioService audioService = ServiceLocator.GetService<IAudioService>();
            
            audioService.PlaySound(_gameThemeAudio.Id, Vector3.zero);
        }
    }
}
