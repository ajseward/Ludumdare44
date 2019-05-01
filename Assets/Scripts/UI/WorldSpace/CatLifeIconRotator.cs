using Sirenix.OdinInspector;
using UnityEngine;
using Utility.Time;

namespace UI.WorldSpace
{
    public class CatLifeIconRotator : SerializedMonoBehaviour
    {
        public float RotationRate = 2f;

        private float _dt;
        private void Update()
        {
            var rotation = transform.rotation.eulerAngles;
            _dt += Time.deltaTime * RotationRate;
            rotation.z = Mathf.Sin(_dt) * 10f;
            transform.rotation = Quaternion.Euler(rotation);
        }
    }
}