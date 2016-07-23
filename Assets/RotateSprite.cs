using UnityEngine;
using System.Collections;

public class RotateSprite : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.touchCount == 1)
        {
            Touch t1 = Input.touches[0];
            if (t1.phase == TouchPhase.Moved)
            {
                transform.rotation = Quaternion.AngleAxis(Angle(t1.position, Camera.main.WorldToScreenPoint(transform.position)), Vector3.forward);
            }
        }
	}

    float Angle(Vector2 pos1, Vector2 pos2)
    {
        Vector2 from = pos2 - pos1;
        Vector2 to = new Vector2(1, 0);

        float result = Vector2.Angle(from, to);
        Vector3 cross = Vector3.Cross(from, to);

        if (cross.z > 0)
        {
            result = 360f - result;
        }

        return result;
    }
}
