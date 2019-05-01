using Character;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Progression
{
    public class EndTrigger : SerializedMonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            var movement = other.GetComponent<Movement>();

            if (movement == null)
                return;
            
            var isAI = movement.IsAIControlled;

            if (isAI)
                return;
            
            Camera.main.GetComponent<CameraFollow>().MaxSpeed = 0f;
            
            movement.PlayerInput.Disable();
            movement.SetMovementDirection(new Vector2(1f, 0f));
            
            ProgressionSystem.Instance.StartGoToUpgrades();
        }
    }
}