using Sirenix.OdinInspector;
using UnityEngine;
using Random = System.Random;

namespace Character
{
    public class Explosion : SerializedMonoBehaviour
    {
        public AudioClip[] AudioClips;
        public AnimationClip ExplodeAnim;

        private void OnEnable()
        {
            var rand = new Random();
            var clip = rand.Next(0, AudioClips.Length);
            var audioSource = GetComponent<AudioSource>();
            audioSource.clip = AudioClips[clip];
            audioSource.Play();
            Destroy(gameObject, ExplodeAnim.length);
        }
    }
}