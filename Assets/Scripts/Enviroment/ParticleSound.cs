using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSound : MonoBehaviour
{
    private ParticleSystem Particles;

    private int CurrentParticles;

    private AudioSource AudioManager;

    void Start()
    {
        Particles = GetComponent<ParticleSystem>();
        AudioManager = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Particles.particleCount > CurrentParticles)
        {
            AudioManager.Play();
        }

        CurrentParticles = Particles.particleCount;
    }

    void OnParticleTrigger()
    {
        AudioManager.Play();
    }
}
