using Leaosoft.Input;
using UnityEngine;
using Leaosoft;

namespace Game.Gameplay.Playing
{
    public sealed class CharacterMovement : EntityComponent
    {
        [SerializeField]
        private float _moveSpeed;
        
        private IInputService _inputService;
        private Rigidbody2D _rigidBody;
        private Vector2 _movement;
        private bool _isFacingRight = true;

        public void Begin(IInputService inputService, Rigidbody2D rigidBody)
        {
            _inputService = inputService;

            _rigidBody = rigidBody;

            base.Begin();
        }

        protected override void OnBegin()
        {
            base.OnBegin();

            _inputService.OnReadInputs += HandleReadInputs;
        }

        protected override void OnStop()
        {
            base.OnStop();
            
            _inputService.OnReadInputs -= HandleReadInputs;
        }
        
        protected override void OnFixedTick(float fixedDeltaTime)
        {
            base.OnFixedTick(fixedDeltaTime);

            MoveCharacter(_movement);
        }

        private void HandleReadInputs(InputsData inputsData)
        {
            _movement = inputsData.Movement;

            if (ShouldFlip(_movement))
            {
                FlipCharacter();
            }
        }
        
        private void FlipCharacter()
        {
            _isFacingRight = !_isFacingRight;

            Transform characterTransform = transform;
                
            Vector3 localScale = characterTransform.localScale;

            localScale.x *= -1f;

            characterTransform.localScale = localScale;
        }
        
        private void MoveCharacter(Vector2 movement)
        {
            float horizontalMovement = movement.x * _moveSpeed;
            float verticalMovement = _rigidBody.velocity.y;

            _rigidBody.velocity = new Vector2(horizontalMovement, verticalMovement);
        }

        private bool ShouldFlip(Vector2 movement)
        {
            if (_isFacingRight && movement.x < 0)
            {
                return true;
            }
            
            if (!_isFacingRight && movement.x > 0)
            {
                return true;
            }

            return false;
        }
    }
}
