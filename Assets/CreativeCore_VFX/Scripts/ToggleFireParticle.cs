using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]

public class ToggleFireParticle : MonoBehaviour
{
    private ParticleSystem fireParticle;
    public ParticleSystem igniteParticle;
    public ParticleSystem extinguishParticle;
    public GameObject pointLight;

    private void Start()
    {
        fireParticle = GetComponent<ParticleSystem>();
    }

    public void ToggleFire(bool isOn)
    {
        if (!isOn)
        {
            fireParticle?.Stop();
            pointLight?.SetActive(false);
            if (extinguishParticle != null)
                extinguishParticle.Play();
        } 
        else
        {
            fireParticle?.Play();
            pointLight?.SetActive(true);
            if (igniteParticle != null)
                igniteParticle.Play();
        }
    }
}
