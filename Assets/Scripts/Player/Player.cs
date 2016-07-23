using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour {

    public string charName;
    public int health = 100;
    public bool isTurn;
    public bool isDangerous;
    public Team team;
    public bool attacked;
    public WeaponWheel weaponWheel;
    public Weapon currentWeapon;

    public float angle;
    public GameObject explosive;
    public GameObject fired;
    public float power;

    public float moveSpeed = 6f;
    public float joystickThreshold = 0.15f;
    float horizontal;
    float vertical;

    Rigidbody2D rbody;
    Animator animator;
    GameManager gameManager;

    public delegate void Action();
    public event Action PreTurnEnd;
    public event Action TurnEnd;

    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        power = 0;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        currentWeapon = gameManager.weapons[0];
    }

    void Update()
    {
        if (health <= 0)
            GetComponent<Animator>().SetTrigger("die");
        if (health < 0)
            health = 0;

        horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
        vertical = CrossPlatformInputManager.GetAxis("Vertical");


        bool walking = Mathf.Abs(horizontal) > 0.4f;
        animator.SetBool("walking", walking && isTurn);
        animator.SetBool("aiming", !walking && isTurn && isDangerous && currentWeapon.arms != null);

        if (isTurn)
        {
            currentWeapon = gameManager.currentWeapon;
            // Handle Walking and Aiming.
            float tempAngle = angle;
            if (walking)
                HandleWalking();
            else if (Mathf.Abs(vertical) > joystickThreshold /2f)
                HandleAiming();
            else
                rbody.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;

            // Handle Shooting.
            if (CrossPlatformInputManager.GetButton("Fire") && isDangerous)
            {
                power += Time.deltaTime;
            }
            
            if ((CrossPlatformInputManager.GetButtonUp("Fire") || power >= 1f) && isDangerous)
            {
                StartCoroutine(Shoot());
            }

        }
        else
        {
            if (!attacked)
            {
                rbody.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
            }
               
        }
    }

    IEnumerator Shoot()
    {
        float p = power;
        power = 0f;
        if (currentWeapon.animationTrigger != "")
            animator.SetTrigger(currentWeapon.animationTrigger);
        yield return new WaitForSeconds(currentWeapon.delay);
        if (currentWeapon.weaponType == WeaponType.Normal)
        {
            Vector3 direction = (Quaternion.AngleAxis(angle, Vector3.forward) * new Vector3(1, 0));
            GameObject exp = currentWeapon.explosive.gameObject;
            GameObject go = (GameObject)Instantiate(exp, transform.position + direction, Quaternion.identity);
            go.GetComponent<Rigidbody2D>().velocity = currentWeapon.drop ? Vector3.zero : direction.normalized * 15f * p;
            fired = go;
            gameManager.GiveExplosive(go.GetComponent<Explosive>());
            EndTurn();
        }
        else
        {
            Debug.Log("fdsfg");
        }
        isTurn = currentWeapon.drop;
    }

    void HandleWalking()
    {
        EndPreturn();
        Vector2 velocity = rbody.velocity;
        velocity.x = horizontal * moveSpeed;
        rbody.velocity = velocity;
        transform.localScale = velocity.x >= 0 ? new Vector3(1, 1, 1) : new Vector3(-1, 1, 1);
        angle = transform.localScale.x == 1 ? 0 : -180;
        rbody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void HandleAiming()
    {
        if (transform.localScale.x == 1)
            angle = Mathf.Clamp(angle + CrossPlatformInputManager.GetAxis("Vertical") * 4f, -90, 90);
        else if (transform.localScale.x == -1)
            angle = Mathf.Clamp(angle - CrossPlatformInputManager.GetAxis("Vertical") * 4f, -270, -90);
        rbody.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
    }

    public void EndPreturn()
    {
        if (PreTurnEnd != null)
            PreTurnEnd();
    }

    public void EndTurn()
    {
        if (TurnEnd != null)
            TurnEnd();
    }
}
