using Progression;
using Singletons;
using Sirenix.OdinInspector;
using UI.WorldSpace;
using UnityEngine;

namespace Character
{
    public class Health : SerializedMonoBehaviour
    {
        public GameObject DeathCurrencyPrefab;
        public int CurrentHealth;
        public int MaxHealth;

        private bool _isAIControlled;
        private ProgressionSystem _upgrades;

        private void Awake()
        {
            _isAIControlled = GetComponent<Movement>().IsAIControlled;
            _upgrades = ProgressionSystem.Instance;
            MaxHealth = _isAIControlled ? _upgrades.GetEnemySettings().Health : _upgrades.MaxHealth;
            CurrentHealth = MaxHealth;
        }

        public void ApplyDamage(int damage)
        {
            CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0, MaxHealth);

            if (CurrentHealth <= 0)
            {
                if (!_isAIControlled)
                    StageSession.Instance.PlayerDied();

                else
                {
                    // death anim
                    var popup = Instantiate(DeathCurrencyPrefab, transform.position, Quaternion.identity).GetComponent<CatLifePopup>();
                    popup.Build(_upgrades.GetEnemySettings().Lives.ToString());
                    _upgrades.AwardLives();
                    Destroy(gameObject);
                }
            }
        }
    }
}