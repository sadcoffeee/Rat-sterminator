using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playercontroller : MonoBehaviour
{
    bool WalkingLeft;
    bool WalkingUp;
    bool WalkingRight;
    bool WalkingDown;
    Animator anim;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    public float walkingSpeed;
    public float dashSpeed;
    private DashDirection dashDirection;
    public float dashDuration;
    public float dashTimer;
    bool isDashing;



    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        dashDirection = DashDirection.NoDirection;
        isDashing = false;
    }

    // Update is called once per frame
    void Update()
    {
       //Dash when space is pressed if you arent already dashing
        if (Input.GetKeyDown(KeyCode.Space) && !isDashing)
        {
            isDashing = true;
        }
       
        //Get walking key inputs, set walking to true and play walking animation
        if (Input.GetKeyDown(KeyCode.D))
        {
            WalkingRight = true;
            spriteRenderer.flipX = false;
            anim.Play("Walk");
            dashDirection = DashDirection.Right;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            WalkingLeft = true;
            spriteRenderer.flipX = true;
            anim.Play("Walk");
            dashDirection = DashDirection.Left;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            WalkingUp = true;
            anim.Play("Walk");
            dashDirection = DashDirection.Up;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            WalkingDown = true;
            anim.Play("Walk");
            dashDirection = DashDirection.Down;
        }

        //Get key up, play idle animation and stop walking
        if (Input.GetKeyUp(KeyCode.D))
        {
            WalkingRight = false;
            anim.Play("Idle");
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            WalkingLeft = false;
            anim.Play("Idle");
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            WalkingUp = false;
            anim.Play("Idle");
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            WalkingDown = false;
            anim.Play("Idle");
        }
    }

    private void FixedUpdate()
    {
       rb.velocity = Vector2.zero;
        if(WalkingRight)
        {
            rb.velocity =  new Vector2(rb.velocity.x + walkingSpeed, rb.velocity.y);
        }
        if (WalkingLeft)
        {
            rb.velocity = new Vector2(rb.velocity.x - walkingSpeed, rb.velocity.y);
        }
        if (WalkingUp)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + walkingSpeed);
        }
        if (WalkingDown)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y - walkingSpeed);
        }

        if (isDashing) 
        {
            if (dashTimer >= dashDuration)
            {
                isDashing = false;
                dashTimer = 0;
                rb.velocity = Vector2.zero;
            }
            else
            {
                dashTimer += Time.deltaTime;
                if (dashDirection == DashDirection.Left)
                {
                    rb.velocity = Vector2.left * dashSpeed;
                }

                if (dashDirection == DashDirection.Right)
                {
                    rb.velocity = Vector2.right * dashSpeed;
                }

                if (dashDirection == DashDirection.Up)
                {
                    rb.velocity = Vector2.up * dashSpeed;
                }
                if (dashDirection == DashDirection.Down)
                {
                    rb.velocity = Vector2.down * dashSpeed;
                }
                Debug.Log(dashDirection);
            }
        }
    }
        enum DashDirection
    {
        Left,
        Right,
        Up,
        Down,
        NoDirection
    }
}
