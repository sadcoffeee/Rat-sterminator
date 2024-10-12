using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class player : MonoBehaviour
{
    Animator anim;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    public float walkingSpeed;
    public Facing direction;
    public GameObject arrow1;
    public GameObject arrow2;
    public GameObject arrow3;
    public int health;

    Vector3 oldposition;
    

    int powerlevel = 0;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        health = 5;
        oldposition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (transform.position != oldposition)
        {
            oldposition = transform.position;
            anim.SetBool("idle", false);
        }
        else
        {
            anim.SetBool("idle",true);
        }
        */

        if (health <= 0) { SceneManager.LoadScene("youdie"); }
        
        //Increase or decrease powerlevel
        if (Input.GetKeyDown(KeyCode.UpArrow) && powerlevel < 2)
        {
            powerlevel++;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) && powerlevel > 0)
        {
            powerlevel--;
        }

        //Control the direction we're facing based on the latest keypress
        if (Input.GetKey(KeyCode.W))
        {
            direction = Facing.Up;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction = Facing.Down;
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction = Facing.Left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction = Facing.Right;
        }

        //Attack!  First check which powerlevel we are, then check which direction, then spawn arrow at one of 4 predetermined locations and rotations offset from where the current player transform is
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Base shooting
            if (powerlevel == 0)
            {
                if (direction == Facing.Left)
                {
                    Instantiate(arrow1, new Vector3(transform.position.x - 1.7f, transform.position.y + 1.68f, 0f), Quaternion.Euler(0, 0, 180));
                }
                if (direction == Facing.Right)
                {
                    Instantiate(arrow1, new Vector3(transform.position.x + 1.7f, transform.position.y + 1.3f, 0f), Quaternion.Euler(0, 0, 0));
                }
                if (direction == Facing.Up)
                {
                    Instantiate(arrow1, new Vector3(transform.position.x + 0.16f, transform.position.y + 3.08f, 0f), Quaternion.Euler(0, 0, 90));
                }
                if (direction == Facing.Down)
                {
                    Instantiate(arrow1, new Vector3(transform.position.x - 0.19f, transform.position.y - 0.18f, 0f), Quaternion.Euler(0, 0, 270));
                }
            }
            //Power up
            if (powerlevel == 1)
            {
                if (direction == Facing.Left)
                {
                    Instantiate(arrow2, new Vector3(transform.position.x -1.7f, transform.position.y + 1.68f, 0f), Quaternion.Euler(0,0,180));
                }
                if (direction == Facing.Right)
                {
                    Instantiate(arrow2, new Vector3(transform.position.x + 1.7f, transform.position.y + 1.3f, 0f), Quaternion.Euler(0, 0, 0));
                }
                if (direction == Facing.Up)
                {
                    Instantiate(arrow2, new Vector3(transform.position.x + 0.16f, transform.position.y + 3.08f, 0f), Quaternion.Euler(0, 0, 90));
                }
                if (direction == Facing.Down)
                {
                  Instantiate(arrow2, new Vector3(transform.position.x - 0.19f, transform.position.y - 0.18f, 0f), Quaternion.Euler(0,0,270));
                }
            }
            //Speediest shot
            if (powerlevel == 2)
            {
                if (direction == Facing.Left)
                {
                    Instantiate(arrow3, new Vector3(transform.position.x - 1.7f, transform.position.y + 1.68f, 0f), Quaternion.Euler(0, 0, 180));
                }
                if (direction == Facing.Right)
                {
                    Instantiate(arrow3, new Vector3(transform.position.x + 1.7f, transform.position.y + 1.3f, 0f), Quaternion.Euler(0, 0, 0));
                }
                if (direction == Facing.Up)
                {
                    Instantiate(arrow3, new Vector3(transform.position.x + 0.16f, transform.position.y + 3.08f, 0f), Quaternion.Euler(0, 0, 90));
                }
                if (direction == Facing.Down)
                {
                    Instantiate(arrow3, new Vector3(transform.position.x - 0.19f, transform.position.y - 0.18f, 0f), Quaternion.Euler(0, 0, 270));
                }
            }
        }
    }

    private void FixedUpdate()
    {
        //Powerlevel 0 movement
        if (powerlevel == 0)
        {
            if (direction == Facing.Right && Input.GetKey(KeyCode.D))
            {
                transform.Translate(walkingSpeed, 0.0f, 0f);
                anim.Play("walk1");
                spriteRenderer.flipX = false;
            }
            else if (direction == Facing.Left && Input.GetKey(KeyCode.A))
            {
                transform.Translate(-walkingSpeed, 0.0f, 0f);
                anim.Play("walk1");
                spriteRenderer.flipX = true;
            }
            else if (direction == Facing.Down && Input.GetKey(KeyCode.S))
            {
                transform.Translate(0.0f, -walkingSpeed, 0f);
                anim.Play("walk1");
            }
            else if (direction == Facing.Up && Input.GetKey(KeyCode.W))
            {
                transform.Translate(0.0f, walkingSpeed, 0f);
                anim.Play("walk1");
            }
            else
            { 
                anim.Play("idle1");
            }
        }

        //Powerlevel 1 movement
        if (powerlevel == 1)
        {
            if (direction == Facing.Right && Input.GetKey(KeyCode.D))
            {
                transform.Translate(walkingSpeed, 0.0f, 0f);
                anim.Play("walk2");
                spriteRenderer.flipX = false;
            }
            else if (direction == Facing.Left && Input.GetKey(KeyCode.A))
            {
                transform.Translate(-walkingSpeed, 0.0f, 0f);
                anim.Play("walk2");
                spriteRenderer.flipX = true;
            }
            else if (direction == Facing.Down && Input.GetKey(KeyCode.S))
            {
                transform.Translate(0.0f, -walkingSpeed, 0f);
                anim.Play("walk2");
            }
            else if (direction == Facing.Up && Input.GetKey(KeyCode.W))
            {
                transform.Translate(0.0f, walkingSpeed, 0f);
                anim.Play("walk2");
            }
            else
            {
                anim.Play("idle2");
            }
        }

        //Powerlevel 2 movement
        if (powerlevel == 2)
        {
            if (direction == Facing.Right && Input.GetKey(KeyCode.D))
            {
                transform.Translate(walkingSpeed, 0.0f, 0f);
                anim.Play("walk3");
                
                spriteRenderer.flipX = false;
            }
            else if (direction == Facing.Left && Input.GetKey(KeyCode.A))
            {
                transform.Translate(-walkingSpeed, 0.0f, 0f);
                anim.Play("walk3");
                spriteRenderer.flipX = true;
            }
            else if (direction == Facing.Down && Input.GetKey(KeyCode.S))
            {
                transform.Translate(0.0f, -walkingSpeed, 0f);
                anim.Play("walk3");
            }
            else if (direction == Facing.Up && Input.GetKey(KeyCode.W))
            {
                transform.Translate(0.0f, walkingSpeed, 0f);
                anim.Play("walk3");
            }
            else
            {
                anim.Play("idle3");
            }
        }
    }
    public enum Facing
    {
        Left,
        Right,
        Up,
        Down,
    }
}

