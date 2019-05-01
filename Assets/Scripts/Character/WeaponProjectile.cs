using System.Collections;
using System.Xml.Schema;
using Progression;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UnityEngine;
using Utility.Time;
using Random = System.Random;

namespace Character
{
    public class WeaponProjectile : SerializedMonoBehaviour
    {
        public AudioClip[] FireSounds;
        
        private Collider2D _collider;

        private ProgressionSystem _upgrades;

        private Rigidbody2D _rb;

        private TimeSince _timeSinceSpawn;

        private bool _aiControlled;

        private float _lifetime = 2f;
        private float _colliderActivateTime = 0.5f;
        private float _scaleSpeed = 0.25f;
        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
            _upgrades = ProgressionSystem.Instance;
            _rb = GetComponent<Rigidbody2D>();
        }
        
        private void Start()
        {
            var rand = new Random();
            var clip = rand.Next(0, FireSounds.Length);

            var audioSource = GetComponent<AudioSource>();
            audioSource.clip = FireSounds[clip];
            audioSource.Play();
            _timeSinceSpawn = 0;
        }

        private IEnumerator ScaleUp(float toScale)
        {
            var dt = 0f;
            while (transform.localScale.x < toScale)
            {
                transform.localScale = Vector3.Slerp(Vector3.one, Vector3.one * toScale, dt);
                dt += Time.deltaTime / _scaleSpeed;
                yield return null;
            }
        }

        public void Fire(float movementVelocityX, float dir, bool aiControlled)
        {
            _aiControlled = aiControlled;
            if (Mathf.Approximately(dir, 0))
                dir = 1f;

            var scale = _aiControlled ? 1f : _upgrades.ProjectileScale;
            StartCoroutine(ScaleUp(scale));
            
            var gravScale = _aiControlled
                ? _upgrades.GetEnemySettings().ProjectileGravityScale
                : _upgrades.ProjectileGravityScale;

            _rb.gravityScale = gravScale;
            
            var speed = _aiControlled ? _upgrades.GetEnemySettings().ProjectileSpeed : _upgrades.ProjectileSpeed;
            _rb.AddForce(new Vector2((_upgrades.ProjectileSpeed + movementVelocityX) * dir, 1.5f), ForceMode2D.Impulse);
            _rb.AddTorque(speed * -dir);
        }

        private void FixedUpdate()
        {
            if (_timeSinceSpawn > _lifetime)
            {
                Destroy(gameObject);
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            
            var health = other.gameObject.GetComponent<Health>();
            var hasHealth = health != null;

            if (_timeSinceSpawn > _colliderActivateTime || hasHealth)
            {
                Explode();
            }
            
            if (!hasHealth) 
                return;
            
            var dmg = 0;
            dmg = _aiControlled ? _upgrades.GetEnemySettings().Damage : _upgrades.ProjectileDamage;
            health.ApplyDamage(dmg);
        }

        private void Explode()
        {
            var explosion = transform.GetChild(0);
            transform.DetachChildren();
            Destroy(gameObject);
            explosion.gameObject.SetActive(true);
        }
        private void OnCollisionStay2D(Collision2D other)
        {
            
            if (_timeSinceSpawn > _colliderActivateTime)
            {
                Explode();
            }
        }
    }
}