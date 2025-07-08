using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
    public class MusicManager : MonoBehaviour
    {
        [SerializeField]
        private AudioSource audioSource;

        [SerializeField]
        private List<AudioClip> musicClips = new List<AudioClip>();


        private void Awake()
        {
            if (FindObjectsOfType<MusicManager>().Length > 1)
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);

            if (audioSource == null)
            {
                audioSource = GetComponent<AudioSource>();
            }
        }


        private void Start()
        {
            if (musicClips == null || musicClips.Count == 0 || audioSource == null)
            {
                return;
            }

            var clip = musicClips[Random.Range(0, musicClips.Count)];
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}