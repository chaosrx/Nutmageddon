using UnityEngine;
using System.Collections;

public class ItemBob : MonoBehaviour {

    private float initialY;
    public float variance;

    void Start()
    {
        initialY = transform.position.y;
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x, initialY + Mathf.Sin(Time.time) * variance);
    }
}
