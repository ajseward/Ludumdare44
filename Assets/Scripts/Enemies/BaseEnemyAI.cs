using Character;
using Progression;
using UnityEngine;
using Utility.Time;

namespace Enemies 
{
    public class BaseEnemyAI : MonoBehaviour
    {
        public bool CanMove = true;
        public bool CanJump = false;
        public bool IsFlying = false;

        private Movement _movement;

        private GameObject _player;
        private Movement _playerMovement;
        private Weapon _weapon;

        private bool _hasRolledJump;

        private float _xDir = -1f;
        private float _yDir;

        private ProgressionSystem _upgrades;

        private TimeSince _timeSinceFired;
        private void Awake()
        {
            _movement = GetComponent<Movement>();
            _weapon = GetComponent<Weapon>();
            _player = GameObject.FindWithTag("Player");
            _playerMovement = _player.GetComponent<Movement>();
            _upgrades = ProgressionSystem.Instance;
            _timeSinceFired = 0f;
        }

        private void FixedUpdate()
        {
            if (CanMove)
            {
                if (CanJump)
                {
                    if (!_playerMovement.IsGrounded && !_hasRolledJump)
                    {
                        _hasRolledJump = true;
                        var jumpChance = Random.Range(0f, 1f);
                        if (jumpChance > 0.6f)
                        {
                            _yDir = 1f;
                        }
                    }
                    else if (_playerMovement.IsGrounded && _hasRolledJump)
                    {
                        _yDir = 0f;
                        _hasRolledJump = false;
                    }
                }
                _movement.SetMovementDirection(new Vector2(_xDir, _yDir));
            }

            var playerDir = (_player.transform.position.x < transform.position.x) ? -1f : 1f;
            var dist = Vector3.Distance(transform.position, _player.transform.position);
            var extraDist = IsFlying ? 1.5f : 1f;
            if (dist <= _upgrades.GetEnemySettings().FireDistance * extraDist)
            {
                if (!Mathf.Approximately(_xDir, playerDir))
                    _xDir = playerDir;

                if (_weapon.Fire())
                {
                    _timeSinceFired = 0f;
                }
                
                if (dist <= _upgrades.GetEnemySettings().FireDistance * extraDist && !_weapon.CanFireSoon && _timeSinceFired > _weapon.Cooldown * 0.3f)
                {
                    _xDir = -playerDir;
                }
            }
            else
            {
                if (!Mathf.Approximately(_xDir, playerDir))
                    _xDir = playerDir;
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                var otherRb = other.gameObject.GetComponent<Rigidbody2D>();
                otherRb.AddForce(new Vector2(-otherRb.velocity.x * 2f, 0), ForceMode2D.Impulse);
            }
        }
    }
}
