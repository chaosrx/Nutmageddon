using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Destructible2D;

public class Explosive : MonoBehaviour {

    public Sprite smoke;
    public float trailLife;
    public float damageCaused;
    public Texture2D stamp;
    public float explosionSize;
    public bool faceDirection = false;
    public float timer;
    public float timeDelta;
    public float particleFrequency;
    public float particleFrequencyDelta;
    ParticleManager particleManager;
    GameManager gameManager;

    void Start()
    {
        timeDelta = timer;
        particleFrequencyDelta = particleFrequency;
        particleManager = GameObject.Find("GameManager").GetComponent<ParticleManager>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (timer > 0)
            return;
        Debug.Log(collision.collider.transform.name);
        Detonate();
    }

    void Detonate()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        D2dDestructible.StampAll(transform.position, new Vector2(1, 1) * explosionSize, 0f, stamp, 1);
        List<Player> players = GameObject.Find("GameManager").GetComponent<GameManager>().Players;
        foreach (Player player in players)
        {
            if (Vector2.Distance(player.transform.position, transform.position) < 2f)
            {
                int damage = Mathf.RoundToInt((1f - (Vector2.Distance(player.transform.position, transform.position) / 2f)) * damageCaused);
                gameManager.RegisterDamage(player, damage);
                player.attacked = true;
                player.GetComponent<Animator>().SetTrigger("attacked");
                player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                Vector2 throwDirection = player.transform.position - transform.position;
                throwDirection.y = 1;
                player.GetComponent<Rigidbody2D>().velocity = throwDirection * (damage / 20f);
            }
        }
        Destroy(gameObject);
    }

    void FixedUpdate()
    {
        if (faceDirection)
        {
            Vector2 vel = GetComponent<Rigidbody2D>().velocity;
            GetComponent<Rigidbody2D>().rotation = Mathf.Atan2(vel.y, vel.x) * Mathf.Rad2Deg;
        }
        if (transform.position.y < -5)
            Destroy(gameObject);
        if (timer > 0)
        {
            timeDelta -= Time.fixedDeltaTime;
            if (timeDelta <= 0)
            {
                Detonate();
            }
        }
        if (particleFrequency > 0)
        {
            particleFrequencyDelta -= Time.fixedDeltaTime;
            if(particleFrequencyDelta <= 0)
            {
                particleManager.NewParticle(transform.position, smoke, trailLife, true);
                particleFrequencyDelta = particleFrequency;
            }
        }
    }
}
