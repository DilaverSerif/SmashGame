using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanBrekable : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var check = FindObjectOfType<DragAndShoot>();
        
        if (check != null)
        {
            GameBase.Dilaver.ParticlePlaySystem.SetScale(Vector3.one * 2).PlayParticle(Particles.smoke,transform.position);
            Finisher.Check.Invoke(transform);
            Destroy(gameObject);
        }
    }
}
