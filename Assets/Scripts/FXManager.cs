using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXManager : MonoBehaviour
{
    ParticleSystem particuleSystem;
    float time = 0;

    private void Start()
    {
        particuleSystem = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        time+=Time.deltaTime;

        if (time > particuleSystem.duration)
        {
            Destroy(gameObject);
        }
    }
}
