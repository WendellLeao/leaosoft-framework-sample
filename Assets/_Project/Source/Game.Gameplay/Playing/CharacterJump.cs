﻿using Leaosoft.Services;
using Leaosoft.Input;
using Leaosoft.Audio;
using UnityEngine;
using Leaosoft;

namespace Game.Gameplay.Playing
{
    public sealed class CharacterJump : EntityComponent
    {
        [SerializeField]
        private float _jumpForce;
        [SerializeField]
        private float _minimumDistance = 0.06f;
        [SerializeField]
        private LayerMask _groundLayers;

        [Header("Audio")] 
        [SerializeField]
        private AudioData _jumpAudio;
        
        private IInputService _inputService;
        private IAudioService _audioService;
        private BoxCollider2D _boxCollider;
        private Rigidbody2D _rigidBody;
        private bool _isGrounded;
        private bool _isJumping;

        public void Begin(IInputService inputService, Rigidbody2D rigidBody, BoxCollider2D boxCollider)
        {
            _inputService = inputService;
            
            _audioService = ServiceLocator.GetService<IAudioService>();
            
            _boxCollider = boxCollider;
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

        protected override void OnTick(float deltaTime)
        {
            base.OnTick(deltaTime);

            _isGrounded = IsGrounded();
        }

        protected override void OnFixedTick(float fixedDeltaTime)
        {
            base.OnFixedTick(fixedDeltaTime);

            if (!CanJump())
            {
                return;
            }
            
            _rigidBody.AddForce(Vector2.up * _jumpForce);

            _audioService.PlaySound(_jumpAudio.Id, Vector3.zero);
            
            _isJumping = false;
        }

        private void HandleReadInputs(InputsData inputsData)
        {
            if (!inputsData.PressJump)
            {
                return;
            }
        
            if (_isJumping)
            {
                return;
            }
                
            _isJumping = true;
        }

        private bool CanJump()
        {
            if (!_isJumping)
            {
                return false;
            }

            if (!_isGrounded)
            {
                _isJumping = false;
                
                return false;
            }

            return true;
        }

        private bool IsGrounded()
        {
            Bounds bounds = _boxCollider.bounds;
            
            Vector3 origin = bounds.center;

            Vector3 boundsSize = bounds.size;

            RaycastHit2D raycastHit = Physics2D.BoxCast(origin, boundsSize, 0f, Vector2.down, _minimumDistance, _groundLayers);

            DrawGroundDetection(raycastHit, bounds, _minimumDistance);
            
            return raycastHit.collider != null;
        }

        private void DrawGroundDetection(RaycastHit2D raycastHit, Bounds bounds, float minimumDistance)
        {
            bool hitGround = raycastHit.collider != null;
            
            Color rayColor = hitGround ? Color.green : Color.red;

            Debug.DrawRay(bounds.center + new Vector3(bounds.extents.x, 0), Vector2.down * (bounds.extents.y + minimumDistance), rayColor);
            Debug.DrawRay(bounds.center - new Vector3(bounds.extents.x, 0), Vector2.down * (bounds.extents.y + minimumDistance), rayColor);
            Debug.DrawRay(bounds.center - new Vector3(bounds.extents.x, bounds.extents.y + minimumDistance), Vector2.right * (bounds.extents.x * 2f), rayColor);
        }
    }
}
