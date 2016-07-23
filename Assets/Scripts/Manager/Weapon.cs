using UnityEngine;
using System.Collections;

public enum WeaponType { Normal, Standing, Drop }

[System.Serializable]
public class Weapon {

    [Header("Universal")]
    public string name;
    public WeaponType weaponType;
    public Sprite icon;
    public string animationTrigger;

    [Header("Normal")]
    public Transform explosive;
    public Sprite arms;
    public Sprite armsAfter;
    public int offset;
    public bool drop;
    public float delay;

    public Weapon(string name, Sprite icon)
    {
        this.name = name;
        this.icon = icon;
    }
}