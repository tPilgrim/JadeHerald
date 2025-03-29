using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    private GameObject Particles;
    private GameObject Light;

    void Start()
    {
        Particles = GameObject.Find("Particles");
        Light = GameObject.Find("Mushroom Light");
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ParticlesButton()
    {
        if(Particles.activeSelf == true)
        {
            Particles.SetActive(false);
        }
        else
        {
            Particles.SetActive(true);
        }
    }

    public void LightButton()
    {
        if (Light.activeSelf == true)
        {
            Light.SetActive(false);
        }
        else
        {
            Light.SetActive(true);
        }
    }
}
