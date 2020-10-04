using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailChangeSmoke : MonoBehaviour
{
    ParticleSystem particles;
    public static RailChangeSmoke Instance;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        particles = GetComponent<ParticleSystem>();
    }

    public void Play(Vector3 pos)
    {
        Play(pos, 15);
    }

    public void Play(Vector3 pos, int particlesCount)
    {
        transform.position = pos;
        particles.Emit(particlesCount);
    }
}
