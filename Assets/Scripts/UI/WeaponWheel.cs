using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WeaponWheel : MonoBehaviour {

    public Sprite iconBackSelected;
    public Sprite iconBack;

    Quaternion originalRotation;
    float startAngle = 0;
    bool held = false;
    float distance = 195f;
    GameManager gameManager;
    Transform[] partsBack;
    Transform[] partsFront;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        originalRotation = this.transform.rotation;

        partsBack = new Transform[gameManager.weapons.Count];
        for (int i = 0; i < gameManager.weapons.Count; i++)
        {
            GameObject go = new GameObject(i.ToString());
            go.AddComponent<Image>().sprite = iconBack;
            go.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
            go.transform.parent = transform;
            partsBack[i] = go.transform;
        }

        partsFront = new Transform[gameManager.weapons.Count];
        for (int i = 0; i < gameManager.weapons.Count; i++)
        {
            GameObject go = new GameObject(i.ToString());
            go.AddComponent<Image>().sprite = gameManager.weapons[i].icon;
            go.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
            go.transform.parent = transform;
            partsFront[i] = go.transform;
        }
    }

    void Update()
    {
        Weapon[] weapons = gameManager.weapons.ToArray();

        for (int i = 0; i < weapons.Length; i++)
        {
            partsBack[i].localPosition = (Quaternion.AngleAxis(i * (360 / weapons.Length), Vector3.forward) * new Vector2(1, 0)).normalized * distance;
            Vector2 dir = transform.rotation * new Vector2(1, 0);
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            partsBack[i].localRotation = Quaternion.AngleAxis(-angle, Vector3.forward);
            partsBack[i].localScale = new Vector3(0.6f, 0.6f, 1);
            partsBack[i].GetComponent<Image>().sprite = iconBack;
            float actualAngle = Mathf.Round((angle / (360 / weapons.Length)) + 0.4999f) * (360 / weapons.Length);

            partsFront[i].localPosition = (Quaternion.AngleAxis(i * (360 / weapons.Length), Vector3.forward) * new Vector2(1, 0)).normalized * distance;
            partsFront[i].localRotation = Quaternion.AngleAxis(-angle, Vector3.forward);
            partsFront[i].localScale = new Vector3(0.3f, 0.3f, 1);;
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

            partsBack[InBounds(0, 20, number)].GetComponent<Image>().sprite = iconBackSelected;
            partsBack[InBounds(0, 20, number)].localScale = new Vector3(1f, 1f, 1f);
            partsBack[InBounds(0, 20, number + 1)].localScale = new Vector3(0.7f, 0.7f, 1f);
            partsBack[InBounds(0, 20, number - 1)].localScale = new Vector3(0.7f, 0.7f, 1f);

            partsFront[InBounds(0, 20, number)].localScale = new Vector3(0.55f, 0.55f, 1f);
            partsFront[InBounds(0, 20, number + 1)].localScale = new Vector3(0.35f, 0.35f, 1f);
            partsFront[InBounds(0, 20, number - 1)].localScale = new Vector3(0.35f, 0.35f, 1f);

            partsBack[InBounds(0, 20, number + 2)].SetAsLastSibling();
            partsBack[InBounds(0, 20, number - 2)].SetAsLastSibling();
            partsFront[InBounds(0, 20, number + 2)].SetAsLastSibling();
            partsFront[InBounds(0, 20, number - 2)].SetAsLastSibling();

            partsBack[InBounds(0, 20, number + 1)].SetAsLastSibling();
            partsBack[InBounds(0, 20, number - 1)].SetAsLastSibling();
            partsFront[InBounds(0, 20, number + 1)].SetAsLastSibling();
            partsFront[InBounds(0, 20, number - 1)].SetAsLastSibling();

            partsBack[InBounds(0, 20, number)].SetAsLastSibling();
            partsFront[InBounds(0, 20, number)].SetAsLastSibling();
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

    public Weapon SelectedWeapon
    {
        get
        {
            Weapon[] weapons = gameManager.weapons.ToArray();
            Vector2 dir = transform.rotation * new Vector2(1, 0);
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            float antiAngle = Mathf.Floor((-angle / (360 / weapons.Length))) * (360 / weapons.Length);
            int number = (int)((antiAngle / 18f) - 12.5f);
            return weapons[InBounds(0, 20, number)];
        }
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
        Quaternion newRotation = Quaternion.AngleAxis(angle - startAngle, this.transform.forward);
        newRotation.y = 0; //see comment from above 
        newRotation.eulerAngles = new Vector3(0, 0, newRotation.eulerAngles.z);
        this.transform.rotation = originalRotation * newRotation;
    }

    public void InputIsUp()
    {
        held = false;
    }
}
