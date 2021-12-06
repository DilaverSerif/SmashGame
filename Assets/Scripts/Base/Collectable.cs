using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;


public abstract class Collectable : MonoBehaviour, ICollectable
{
    protected Collider baseCollider;
    [SerializeField] protected Sounds sound;
    [SerializeField] protected Particles _particle;
    [SerializeField] protected bool canDestory;
    [SerializeField] protected int value;
    protected GameObject target;
    
    public virtual void Awake()
    {
        baseCollider = GetComponent<Collider>();
    }

    protected abstract bool condition(GameObject contant);

    private void OnTriggerEnter(Collider other)
    {
        if (!condition(other.gameObject)) return;
        target = other.gameObject;
        Effect();
        Contact(other.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!condition(collision.gameObject)) return;
        target = collision.gameObject;

        var points = collision.contacts;
        Effect(points[0].point);
        Contact(collision.gameObject);
    }

    public abstract void Contact(GameObject target);

    public virtual void Effect([Optional]Vector3 pos)
    {
        GameBase.Dilaver.SoundSystem.PlaySound(sound);
        if (pos == Vector3.zero) pos = transform.position;
        GameBase.Dilaver.ParticlePlaySystem.PlayParticle(_particle,pos);

        if (canDestory)
        {
            Destroy(gameObject);
        }
    }
}