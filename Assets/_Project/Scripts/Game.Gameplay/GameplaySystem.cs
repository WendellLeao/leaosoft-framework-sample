using Game.Gameplay.Playing;
using Game.Gameplay.Inputs;
using Leaosoft.Services;
using Leaosoft.Events;
using Leaosoft.Audio;
using UnityEngine;

namespace Game.Gameplay
{
    public sealed class GameplaySystem : Leaosoft.System
    {
        [SerializeField] private CharacterManager _characterManager;
        [SerializeField] private InputsManager _inputsManager;

        [Header("Audio")] 
        [SerializeField] private AudioData _gameThemeAudio;
        
        protected override void OnInitialize()
        {
            base.OnInitialize();

            IEventService eventService = ServiceLocator.GetService<IEventService>();
            
            _characterManager.Initialize(eventService);
            _inputsManager.Initialize(eventService);

            PlayGameTheme();
        }

        protected override void OnDispose()
        {
            base.OnDispose();
            
            _characterManager.Dispose();
            _inputsManager.Dispose();
        }

        protected override void OnTick(float deltaTime)
        {
            base.OnTick(deltaTime);
            
            _characterManager.Tick(deltaTime);
            _inputsManager.Tick(deltaTime);
        }

        protected override void OnFixedTick(float fixedDeltaTime)
        {
            base.OnFixedTick(fixedDeltaTime);
            
            _characterManager.FixedTick(fixedDeltaTime);
            _inputsManager.FixedTick(fixedDeltaTime);
        }

        private void PlayGameTheme()
        {
            IAudioService audioService = ServiceLocator.GetService<IAudioService>();
            
            audioService.PlaySound(_gameThemeAudio.Id, Vector3.zero);
        }
    }
}
