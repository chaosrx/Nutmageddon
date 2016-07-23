using UnityEngine;
using System.Collections;

public class Aim : MonoBehaviour {

    public Transform root;
    public Transform weaponArms;
    public float angle;
    public float distance;

    void Update()
    {
        if (root != null)
        {
            angle = root.GetComponent<Player>().angle;
            transform.position = root.position + (Quaternion.AngleAxis(angle, Vector3.forward) * new Vector3(1, 0));
            weaponArms.position = root.position + new Vector3(0.1f * root.localScale.x, -0.05f);
            weaponArms.rotation = Quaternion.AngleAxis(angle + (root.localScale.x == 1 ? -32 : 32), Vector3.forward);
            weaponArms.GetComponent<SpriteRenderer>().flipY = root.localScale.x == -1;
            weaponArms.gameObject.SetActive(!root.GetComponent<Animator>().GetBool("walking"));
        }
    }
}
