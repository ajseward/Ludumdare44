using System;
using System.Collections;
using Singletons;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Events;

namespace Progression {
    public class ProgressionSystem : Singleton<ProgressionSystem>
    {
        public struct EnemySettings
        {
            public float MovementSpeed;
            public float JumpImpulse;

            public float WeaponCooldown;
            public float ProjectileSpeed;
            public float ProjectileGravityScale;
            public int Damage;

            public float FireDistance;

            public int Health;
            public int Lives;
        }
        
        [ShowInInspector]
        [PropertyRange(0, 9)]
        public int CurrentLevel
        {
            get => _currentLevel;
            set
            {
                _currentLevel = value;
                OnLevelChanged.Invoke();
            }
        }

        public int CurrentLives;

        private const int BASE_UPGRADE_COST = 9;

        private int GetCost(int level)
        {
            return Mathf.FloorToInt(BASE_UPGRADE_COST * Mathf.Pow(1.1f,level));
        }

        public bool Upgrade(ref int level, ref float value, float increment)
        {
            var cost = GetCost(level);
            if (CurrentLives < cost)
                return false;

            CurrentLives -= cost;

            value += increment;
            ++level;
            return true;
        }

        public bool Upgrade(ref int level, ref int value, int increment)
        {
            var cost = GetCost(level);
            if (CurrentLives < cost)
                return false;

            CurrentLives -= cost;

            value += increment;
            ++level;
            return true;
        }
        
        public enum UpgradeType
        {
            MovementSpeed,
            JumpImpulse,
            WeaponCooldown,
            ProjectileScale,
            ProjectileGravity,
            ProjectileDamage,
            MaxHealth
        }
        
        [TabGroup("Movement Upgrades")] public int MovementSpeedLevel;
        [HideInInspector]public float MovementSpeed = 20f;
        public float MovementSpeedIncrement = 2f;

        [TabGroup("Movement Upgrades")] public int JumpImpulseLevel;
        [HideInInspector] public float JumpImpulse = 40f;
        [HideInInspector] public float JumpImpulseIncrement = 2f;

        [TabGroup("Weapon Upgrades")] public int WeaponCooldownLevel;
        [HideInInspector] public float WeaponCooldown = 2f;
        [HideInInspector] public float WeaponCooldownIncrement = -0.05f;

        [TabGroup("Weapon Upgrades")] public int ProjectileScaleLevel;
        [HideInInspector] public float ProjectileScale = 1f;
        [HideInInspector] public float ProjectileScaleIncrement = 0.25f;

        [TabGroup("Weapon Upgrades")] public int ProjectileSpeedLevel;
        [HideInInspector] public float ProjectileSpeed = 10f;
        [HideInInspector] public float ProjectileSpeedIncrement = 2.5f;

        [TabGroup("Weapon Upgrades")] public int ProjectileGravityScaleLevel;
        [HideInInspector]public float ProjectileGravityScale = 1f;
        [HideInInspector] public float ProjectileGravityIncrement = -0.02f;

        [TabGroup("Weapon Upgrades")] public int ProjectileDamageLevel;
        [HideInInspector] public int ProjectileDamage = 5;
        [HideInInspector] public int ProjectileDamageIncrement = 5;

        [TabGroup("Player Upgrades")] public int MaxHealthLevel;
        [HideInInspector] public int MaxHealth = 10;
        [HideInInspector] public int MaxHealthIncrement = 5;

        public UnityEvent OnLevelChanged = new UnityEvent();
        private int _currentLevel;

        private readonly Color[] _difficultyColors = new[]
        {
            // level 1
            new Color(0.16f, 0.17f, 0.17f),
            // level 2
            new Color(0.78f, 0.75f, 0.76f),
            // level 3
            new Color(0.69f, 0.45f, 0.59f),
            // level 4
            new Color(0.51f, 0.4f, 0.6f),
            // level 5
            new Color(0.27f, 0.33f, 0.55f),
            // level 6
            new Color(0.4f, 0.63f, 0.65f),
            //level 7
            new Color(0.54f, 0.73f, 0.54f),
            // level 8
            new Color(1f, 0.94f, 0.6f),
            // level 9
            new Color(0.69f, 0.45f, 0.16f),
            // level 10
            new Color(0.47f, 0f, 0.09f),
        };

        private readonly EnemySettings[] _enemySettings = new[]
        {
            // level 1
            new EnemySettings()
            {
                MovementSpeed = 5f, 
                JumpImpulse = 40f, 
                WeaponCooldown = 4f, 
                ProjectileSpeed = 2f, 
                ProjectileGravityScale = 1f, 
                Damage = 1,
                FireDistance = 4f,
                Health = 3,
                Lives = 9,
            },
            // level 2
            new EnemySettings()
            {
            MovementSpeed = 7f, 
            JumpImpulse = 60f, 
            WeaponCooldown = 3.5f, 
            ProjectileSpeed = 4f, 
            ProjectileGravityScale = 0.8f, 
            Damage = 3,
            FireDistance = 5f,
            Health = 4,
            Lives = 14,
            },
            // level 3
            new EnemySettings()
            {
                MovementSpeed = 10f, 
                JumpImpulse = 70f, 
                WeaponCooldown = 3f, 
                ProjectileSpeed = 7f, 
                ProjectileGravityScale = 0.6f, 
                Damage = 6,
                FireDistance = 7f,
                Health = 6,
                Lives = 20,
            },
            // level 4
            new EnemySettings()
            {
                MovementSpeed = 13f, 
                JumpImpulse = 75f, 
                WeaponCooldown = 2.5f, 
                ProjectileSpeed = 9f, 
                ProjectileGravityScale = 0.5f, 
                Damage = 10,
                FireDistance = 10f,
                Health = 12,
                Lives = 30,
            },
            // level 5
            new EnemySettings()
            {
                MovementSpeed = 17f, 
                JumpImpulse = 80f, 
                WeaponCooldown = 2.2f, 
                ProjectileSpeed = 12f, 
                ProjectileGravityScale = 0.45f, 
                Damage = 12,
                FireDistance = 12f,
                Health = 20,
                Lives = 45,
            },
            // level 6
            new EnemySettings()
            {
                MovementSpeed = 21f, 
                JumpImpulse = 84f, 
                WeaponCooldown = 2f, 
                ProjectileSpeed = 15f, 
                ProjectileGravityScale = 0.4f, 
                Damage = 14,
                FireDistance = 15f,
                Health = 35,
                Lives = 68,
                
            },
            // level 7
            new EnemySettings()
            {
                MovementSpeed = 28f, 
                JumpImpulse = 89f, 
                WeaponCooldown = 1.7f, 
                ProjectileSpeed = 19f, 
                ProjectileGravityScale = 0.37f, 
                Damage = 20,
                FireDistance = 18f,
                Health = 50,
                Lives = 102,
            },
            // level 8
            new EnemySettings()
            {
                MovementSpeed = 32f, 
                JumpImpulse = 95f, 
                WeaponCooldown = 1.4f, 
                ProjectileSpeed = 20f, 
                ProjectileGravityScale = 0.33f, 
                Damage = 25,
                FireDistance = 20f,
                Health = 70,
                Lives = 153,
            },
            // level 9
            new EnemySettings()
            {
                MovementSpeed = 36f, 
                JumpImpulse = 100f, 
                WeaponCooldown = 1.1f, 
                ProjectileSpeed = 24f, 
                ProjectileGravityScale = 0.3f, 
                Damage = 20,
                FireDistance = 20f,
                Health = 90,
                Lives = 230,
            },
            // level 10
            new EnemySettings()
            {
                MovementSpeed = 40f, 
                JumpImpulse = 120f, 
                WeaponCooldown = 0.8f, 
                ProjectileSpeed = 30f, 
                ProjectileGravityScale = 0.2f, 
                Damage = 40,
                FireDistance = 20f,
                Health = 150,
                Lives = 500,
            },
        };

        public Color GetCurrentColor()
        {
            return _difficultyColors[CurrentLevel];
        }

        public EnemySettings GetEnemySettings()
        {
            return _enemySettings[CurrentLevel];
        }

        public void AwardLives()
        {
           CurrentLives += GetEnemySettings().Lives;
        }

        private IEnumerator GoToUpgrades()
        {
            
            yield return null;
        }

        public void StartGoToUpgrades()
        {
            
        }
    }
}
