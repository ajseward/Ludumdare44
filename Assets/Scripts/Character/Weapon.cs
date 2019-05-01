using Progression;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using Utility.Animation;
using Utility.Time;

namespace Character
{
    public class Weapon : SerializedMonoBehaviour
    {
        public GameObject Projectile;
        public Transform FirePoint;

        public Animator WeaponAnimator;
        
        [ReadOnly]
        public float Cooldown;
        public bool CanFire => _timeSinceFired >= Cooldown;
        public bool CanFireSoon => _timeSinceFired >= Cooldown - 0.5f;
        private TimeSince _timeSinceFired;

        private Rigidbody2D _rb;
        private Movement _movement;

        private ProgressionSystem _upgrades;
        
        
        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _movement = GetComponent<Movement>();
            _upgrades = ProgressionSystem.Instance;
            if (FirePoint == null)
            {
                FirePoint = transform;
                Debug.LogWarning("Fire Point is unset, is this intended?");
            }

             Cooldown = _movement.IsAIControlled
                ? _upgrades.GetEnemySettings().WeaponCooldown
                : _upgrades.WeaponCooldown;
            _timeSinceFired = Cooldown;
            
            if(WeaponAnimator != null && WeaponAnimator.HasParamter("CanFire"))
                WeaponAnimator.SetBool("CanFire", true);
                
        }

        private void FixedUpdate()
        {
            if(WeaponAnimator != null && WeaponAnimator.HasParamter("CanFire") && CanFire)
                WeaponAnimator.SetTrigger("CanFire");

        }

        public bool Fire()
        {
            Cooldown = _movement.IsAIControlled
                ? _upgrades.GetEnemySettings().WeaponCooldown
                : _upgrades.WeaponCooldown;
            if (!CanFire)
                return false;
            if(WeaponAnimator != null && WeaponAnimator.HasParamter("Cooldown"))
                WeaponAnimator.SetFloat("Cooldown", 1 / Cooldown);
            _timeSinceFired = 0;
            var projectile = Instantiate(Projectile, FirePoint.position, Quaternion.identity).GetComponent<WeaponProjectile>();
            var dir = 0f;

            if (Mathf.Approximately(_movement.InputDirection, 0))
            {
                dir = _movement.IsFlipped ? -1f : 1f;
            }
            else
            {
                dir = _movement.InputDirection;
            }
            projectile.Fire(_rb.velocity.x, dir, _movement.IsAIControlled);
            if(WeaponAnimator != null && WeaponAnimator.HasParamter("Cooldown"))
                WeaponAnimator.SetTrigger("Fired");
            return true;
        }
    }
}