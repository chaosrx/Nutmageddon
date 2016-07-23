using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameCamera : MonoBehaviour {

    public Transform bg1;
    public Transform bg2;
    public Transform bg3;

    public List<Transform> targets;
    public bool waitForDestroy = false;

    public delegate void Action();
    public event Action TargetDestroyed;

    void Start()
    {
        
    }

    void Update()
    {
        bg1.position = new Vector2(-transform.position.x, 0) / 2;
        bg2.position = new Vector2(-transform.position.x, 0) / 3;
        bg3.position = new Vector2(-transform.position.x, 0) / 4;
        targets.RemoveAll(x => x == null);

        if (targets.Count > 0)
        {
            Vector3 targetPos = Vector2.zero;
            foreach (Transform target in targets)
                targetPos += (Vector3)target.position;
            targetPos /= targets.Count;
            if (targetPos.y < 0)
                targetPos.y = 0;
            targetPos.z = -10f;
            transform.position = Vector3.Lerp(transform.position, targetPos, 0.5f);
        }
        
        if (waitForDestroy)
        {
            if (targets.Count == 0)
            {
                waitForDestroy = false;
                if (TargetDestroyed != null)
                    TargetDestroyed();
            }
        }
    }

    public void SetTarget(Transform target)
    {
        targets = new List<Transform>();
        targets.Add(target);
    }
}
