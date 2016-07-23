using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParticleManager : MonoBehaviour {

    public List<Particle> particles;
    public Transform particleParent;

    void Start()
    {
        particles = new List<Particle>();
        particleParent = new GameObject("ParticleParent").transform;
        particleParent.parent = transform;
        FillPool(20);
    }

    void FillPool(int size)
    {
        for (int i = 0; i < size; i++)
        {
            particles.Add(NewParticle());
        }
    }

    public void NewParticle(Vector2 position, Sprite sprite, float span, bool spin = false)
    {
        Particle p = null;
        foreach (Particle part in particles)
        {
            if (!part.gameObject.activeSelf)
            {
                p = part;
                break;
            }
        }

        if (p == null)
        {
            p = NewParticle();
            p.gameObject.SetActive(true);
            particles.Add(p);
        }
        p.Init(position, sprite, span, spin);
    }

    Particle NewParticle()
    {
        GameObject particleObject = new GameObject("Particle");
        particleObject.AddComponent<SpriteRenderer>().sortingOrder = 1000000;
        particleObject.AddComponent<Rigidbody2D>();
        particleObject.AddComponent<Particle>();
        particleObject.transform.parent = particleParent;
        particleObject.SetActive(false);
        return particleObject.GetComponent<Particle>();
    }
}
