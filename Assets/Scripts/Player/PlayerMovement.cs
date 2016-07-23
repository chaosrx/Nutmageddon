using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerMovement : MonoBehaviour {

    Player player;
    Rigidbody2D rbody;
    Animator animator;

    void Start()
    {
        player = GetComponent<Player>();
        rbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (player.isTurn)
        {
            bool walking = Mathf.Abs(CrossPlatformInputManager.GetAxis("Horizontal")) > 0.15f;
                animator.SetBool("walking", walking);
            if (walking)
            {
                player.EndPreturn();
                Vector2 velocity = rbody.velocity;
                velocity.x = CrossPlatformInputManager.GetAxis("Horizontal") * 6f;
                rbody.velocity = velocity;
                transform.localScale = velocity.x >= 0 ? new Vector3(1, 1, 1) : new Vector3(-1, 1, 1);
                player.angle = transform.localScale.x == 1 ? 0 : -180;
                rbody.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
            else if (Mathf.Abs(CrossPlatformInputManager.GetAxis("Vertical")) > 0.15f)
            {
                if (transform.localScale.x == 1)
                    player.angle = Mathf.Clamp(player.angle + CrossPlatformInputManager.GetAxis("Vertical") * 4f, -90, 90);
                else  if (transform.localScale.x == -1)
                    player.angle = Mathf.Clamp(player.angle - CrossPlatformInputManager.GetAxis("Vertical") * 4f, -270, -90);
                rbody.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
            }
            else
            {
                rbody.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
            }
        }
        else
        {
            rbody.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
        }
    }
}
