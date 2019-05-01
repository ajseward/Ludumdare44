using System.Collections;
using Progression;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using Utility.Time;

namespace Character
{
    [RequireComponent(typeof(Camera))]
    public class CameraFollow : SerializedMonoBehaviour
    {
        public float XAngle = 5f;
        public float YAngle = 65f;
        
        public float MaxSpeed = 20f;
        
        public Vector3 Offset = new Vector3(-7f, 2.5f, -6f);

        private GameObject _player;

        private float _currentSpeed;
        private void Awake()
        {
            _player = GameObject.FindWithTag("Player");
            transform.rotation = Quaternion.Euler(XAngle, 5f, 0);
            ProgressionSystem.Instance.OnLevelChanged.AddListener(UpdateColors);
            UpdateColors();
            StartCoroutine(IntroCamera());
        }

        private IEnumerator IntroCamera()
        {
            var currentY = transform.rotation.eulerAngles.y;
            while (currentY != YAngle)
            {
                var y = Mathf.SmoothStep(currentY, YAngle, Time.deltaTime * MaxSpeed);
                currentY = y;
                transform.rotation = Quaternion.Euler(XAngle, currentY, 0);
                yield return null;
            }
        }


        private void UpdateColors()
        {
            var color = ProgressionSystem.Instance.GetCurrentColor();
            GetComponent<Camera>().backgroundColor = color;
            RenderSettings.fogColor = color;
        }

        private void FixedUpdate()
        {
            var targetPos = _player.transform.position;
            targetPos += Offset;
            transform.position = Vector3.Lerp(transform.position, targetPos, MaxSpeed * Time.fixedDeltaTime);
            
        }
    }
}