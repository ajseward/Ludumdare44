using System;
using System.Linq;
using Input;
using Progression;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEditor;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Experimental.Input;
using Utility.Animation;

namespace Character 
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Sprite), typeof(Weapon))]
    public class Movement : SerializedMonoBehaviour
    {
        public LayerMask GroundLayers;
        
        [HideInInspector]
        [NonSerialized]
        public DefaultInput PlayerInput;
        
        [BoxGroup("Settings")]
        public bool IsAIControlled = false;

        [BoxGroup("Movement Values")] 
        [Range(0f, 1f)]
        public float MovementDamping = 0.5f;

        [BoxGroup("Movement Values")] 
        public bool CanAirControl = true;
        
        [BoxGroup("Debug Values")]
        [ReadOnly]
        public bool IsGrounded;
        
        [BoxGroup("Debug Values")]
        [ReadOnly]
        public bool IsFlipped;
        
        [BoxGroup("Debug Values")]
        [ReadOnly]
        public float InputDirection;
        
        [BoxGroup("Debug Values")]
        [ReadOnly]
        public bool ShouldJump;
        
        [BoxGroup("Debug Values")]
        public bool DrawDebug = false;

        

        private Rigidbody2D _rb;
        private SpriteRenderer _sprite;
        private Animator _animator;
        
        private Vector2 _currentVelocity;

        private Weapon _weapon;

        private ProgressionSystem _upgrades;

        

        private void Awake()
        {
            _upgrades = ProgressionSystem.Instance;
            
            _sprite = GetComponent<SpriteRenderer>();
            _rb = GetComponent<Rigidbody2D>();
            _weapon = GetComponent<Weapon>();
            _animator = GetComponent<Animator>();

            if (!IsAIControlled)
            {
                PlayerInput = new DefaultInput();
                PlayerInput.Enable();
                PlayerInput.Player.Movement.performed += MovementPerformed;
                PlayerInput.Player.Movement.cancelled += MovementPerformed;
                PlayerInput.Player.Shoot.performed += ShootPerformed;
            }
        }

        private void ShootPerformed(InputAction.CallbackContext _)
        {
            _weapon.Fire();
        }

        private void MovementPerformed(InputAction.CallbackContext action)
        {
            var dir = action.ReadValue<Vector2>();
            SetMovementDirection(dir);
        }

        public void SetMovementDirection(Vector2 dir)
        {
            ShouldJump = dir.y > 0 && IsGrounded;

            InputDirection = dir.x;
        }

        private void Move(float move, bool jump)
        {
            if (IsGrounded || CanAirControl)
            {
                var tarVelocity = new Vector2(move * 10f, _rb.velocity.y);
                _rb.velocity = Vector2.SmoothDamp(_rb.velocity, tarVelocity, ref _currentVelocity, MovementDamping);
                
                if(_animator.HasParamter("MoveSpeed"))
                    _animator.SetFloat("MoveSpeed", _rb.velocity.x);
                if (move > 0 && IsFlipped)
                {
                    Flip();
                }
                else if (move < 0 && !IsFlipped)
                {
                    Flip();
                }
            }
            if(_animator.HasParamter("IsGrounded"))
                _animator.SetBool("IsGrounded", IsGrounded);
            
            if (IsGrounded && jump)
            {
                IsGrounded = false;
                var jumpImpulse = IsAIControlled ? _upgrades.GetEnemySettings().JumpImpulse : _upgrades.JumpImpulse;
                _rb.AddForce(new Vector2(0f, jumpImpulse));
            }
            
            if(_animator.HasParamter("IsMoving"))
                _animator.SetBool("IsMoving", !Mathf.Approximately(InputDirection, 0));
        }

        private void FixedUpdate()
        {
            var speed = IsAIControlled ? _upgrades.GetEnemySettings().MovementSpeed : _upgrades.MovementSpeed;
            Move(InputDirection * speed * Time.fixedDeltaTime, ShouldJump);
            IsGrounded = false;

            // get sprite current bounds
            var bounds = _sprite.bounds;
            // get half the size of the bounds
            var extentsY = bounds.extents.y;
            // set the ground position to the transform - half the bounds. (should be center of object)
            var groundCheckPosition = transform.position;
            groundCheckPosition.y -= extentsY;
            
            var colliders = new Collider2D[2];
            var size = Physics2D.OverlapCircleNonAlloc(groundCheckPosition, 0.2f, colliders, GroundLayers);

            if (size > 0)
            {
                IsGrounded = true;
            }
        }

        private void Flip()
        {
            IsFlipped = !IsFlipped;
            _sprite.flipX = IsFlipped;

            var childSprites = GetComponentsInChildren<SpriteRenderer>();
            for (var i = 0; i < childSprites.Length; ++i)
            {
                childSprites[i].flipX = IsFlipped;
            }
        }

        private void OnDrawGizmos()
        {
            if (!DrawDebug || _sprite == null)
                return;
            
            ////
            // Copied from fixed update
            ////
            // get sprite current bounds
            var bounds = _sprite.bounds;
            // get half the size of the bounds
            var extentsY = bounds.extents.y;
            // set the ground position to the transform - half the bounds. (should be center of object)
            var groundCheckPosition = transform.position;
            groundCheckPosition.y -= extentsY;
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(groundCheckPosition, 0.2f);
        }
    }
}
