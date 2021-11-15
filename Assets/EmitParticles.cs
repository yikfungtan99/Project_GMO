using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmitParticles : MonoBehaviour
{
    public List<ParticleSystem> particles = new List<ParticleSystem>();

    public void Emit()
    {
        foreach (ParticleSystem particle in particles)
        {
            particle.Play();
        }
    }
}
