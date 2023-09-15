using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;

        private AudioSource audioSource;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void PlayMainTheme()
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }

        public void StopMainTheme()
        {
            audioSource.Stop();
        }
    }
}
