using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RotateCircle : MonoBehaviour {

    private Quaternion originalRotation;
    private float startAngle = 0;
    private bool held = false;

    public float distance = 1f;
    public Sprite[] weapons;
    public Transform current;
    Transform[] parts;

    public void Start()
    {
        originalRotation = this.transform.rotation;
        parts = new Transform[weapons.Length];
        for (int i = 0; i < weapons.Length; i++)
        {
            GameObject go = new GameObject(i.ToString());
            go.AddComponent<Image>().sprite = weapons[i];
            go.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1f);
            go.transform.parent = transform;
            parts[i] = go.transform;
        }
    }

    void Update()
    {
        

        
        for (int i = 0; i < weapons.Length; i++)
        {
            parts[i].localPosition = (Quaternion.AngleAxis(i * (360 / weapons.Length), Vector3.forward) * new Vector2(1, 0)).normalized * distance;
            Vector2 dir = transform.rotation * new Vector2(1, 0);
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            parts[i].localRotation = Quaternion.AngleAxis(-angle, Vector3.forward);
            parts[i].localScale = new Vector3(0.35f, 0.35f, 1);
            float actualAngle = Mathf.Round((angle / (360 / weapons.Length)) + 0.4999f) * (360 / weapons.Length);
        }
        if (!held)
        {
            Vector2 dir = transform.rotation * new Vector2(1, 0);
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            float targetAngle = Mathf.Round((angle / (360 / weapons.Length)) - 0.4999f) * (360 / weapons.Length);
            float nextAngle = Mathf.LerpAngle(angle, targetAngle + (360 / weapons.Length) / 2f, 0.9f);
            transform.rotation = Quaternion.AngleAxis(nextAngle, Vector3.forward);
            
            Vector2 dirr = transform.rotation * new Vector2(1, 0);
            float anglee = Mathf.Atan2(dirr.y, dirr.x) * Mathf.Rad2Deg;
            float antiAngle = Mathf.Floor((-angle / (360 / weapons.Length))) * (360 / weapons.Length);
            int number = (int)((antiAngle / 18f) - 12.5f);
            number = InBounds(0, 20, number);
            parts[number].localScale = new Vector3(1f, 1f, 1f);
            parts[number].SetAsLastSibling();
        }
    }

    int InBounds(int min, int max, int number)
    {
        while (number < min)
            number += max;
        while (number >= max)
            number -= min;
        return number;
    }

    public void InputIsDown()
    {
        held = true;
        originalRotation = this.transform.rotation;
        Vector3 screenPos = transform.position;
        Vector3 vector = Input.mousePosition - screenPos;
        startAngle = Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
    }

    public void InputIsHeld()
    {
        Vector3 screenPos = transform.position;
        Vector3 vector = Input.mousePosition - screenPos;
        float angle = Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
        Quaternion newRotation = Quaternion.AngleAxis(angle - startAngle , this.transform.forward);
        newRotation.y = 0; //see comment from above 
        newRotation.eulerAngles = new Vector3(0,0,newRotation.eulerAngles.z);
        this.transform.rotation = originalRotation *  newRotation;
    }

    public void InputIsUp()
    {
        held = false;
    }
}
