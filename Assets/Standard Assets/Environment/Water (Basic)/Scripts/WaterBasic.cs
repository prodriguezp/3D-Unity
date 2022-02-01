using System;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

namespace UnityStandardAssets.Water
{
    [ExecuteInEditMode]
    public class WaterBasic : MonoBehaviour
    {
        private FirstPersonController player;
        public AudioClip chapoteo;
        AudioSource chapoteoSource;
        void Update()
        {
            Renderer r = GetComponent<Renderer>();
            if (!r)
            {
                return;
            }
            Material mat = r.sharedMaterial;
            if (!mat)
            {
                return;
            }

            Vector4 waveSpeed = mat.GetVector("WaveSpeed");
            float waveScale = mat.GetFloat("_WaveScale");
            float t = Time.time / 20.0f;

            Vector4 offset4 = waveSpeed * (t * waveScale);
            Vector4 offsetClamped = new Vector4(Mathf.Repeat(offset4.x, 1.0f), Mathf.Repeat(offset4.y, 1.0f),
                Mathf.Repeat(offset4.z, 1.0f), Mathf.Repeat(offset4.w, 1.0f));
            mat.SetVector("_WaveOffset", offsetClamped);
        }
        
        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>();
            chapoteoSource = GetComponent<AudioSource>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                player.m_AudioSource.volume = 0;
                player.m_RunSpeed = 2.5f;
                player.m_WalkSpeed = 3f;
                chapoteoSource.clip = chapoteo;
                chapoteoSource.Play();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            player.m_AudioSource.volume = 0.5f;
            chapoteoSource.Stop();
            player.m_RunSpeed = 10f;
            player.m_WalkSpeed = 5f;
        }
        
    }
}