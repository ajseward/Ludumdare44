using Progression;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using Utility.Time;

namespace UI.WorldSpace
{
    public class CatLifePopup : SerializedMonoBehaviour
    {
        public TMP_Text Text;
        public float FloatSpeed = 2f;
        private void Update()
        {
            var position = transform.position;
            position.y += Time.deltaTime * FloatSpeed;
            transform.position = position;
        }

        public void Build(string amountObtained)
        {
            Text.SetText($"+{amountObtained}");
            Destroy(gameObject, 4f);
        }
    }
}