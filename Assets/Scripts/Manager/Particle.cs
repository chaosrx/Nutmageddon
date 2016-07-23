using UnityEngine;
using System.Collections;

public class Particle : MonoBehaviour {

    public float lifespan;
    Rigidbody2D rbody;

    void Start()
    {
    }

    public void Init(Vector2 position, Sprite sprite, float span, bool randomRotation)
    {
        gameObject.SetActive(true);
        rbody = GetComponent<Rigidbody2D>();
        transform.position = position;
        rbody.gravityScale = 0f;
        rbody.angularDrag = 0f;
        if (randomRotation)
        {
            rbody.angularVelocity = Random.Range(-60, 60);
            rbody.velocity = new Vector2(0.3f, 0);
        }
        GetComponent<SpriteRenderer>().sprite = sprite;
        lifespan = span;
        StartCoroutine(Disable());
    }

    IEnumerator Disable()
    {
        SpriteRenderer sp = GetComponent<SpriteRenderer>();
        yield return new WaitForSeconds(lifespan);
        while (sp.color.a > 0f)
        {
            sp.color = new Color(1, 1, 1, sp.color.a - 1 / 256f);
            yield return new WaitForSeconds(1 / 256f);
        }
        gameObject.SetActive(false);
        sp.color = new Color(1, 1, 1, 1);
    }
}
