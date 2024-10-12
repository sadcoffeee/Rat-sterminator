using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrow1script : MonoBehaviour
{
    GameObject player;
    player script;
    bool shootingLeft;
    bool shootingRight;
    bool shootingUp;
    bool shootingDown;
    public float flyingSpeed;
    GoalScript goalscript;

    // Start is called before the first frame update
    void Start()
    {
      //Realised all of the below code is useless because I rotate the arrow gameobject when I instantiate it and thus its "forward" direction matches positive x always, making it redundant for me to check player direction
        
        /*player = GameObject.FindWithTag("Player");
        script = player.GetComponent<player>();
        // Check what direction player is facing when this object gets instantiated, then flip the appropriate movement switch
        if (script.direction == global::player.Facing.Left)
        {
            shootingLeft = true;
            Debug.Log("shootingleft");
        }
        else if (script.direction == global::player.Facing.Right)
        {
            shootingRight = true;
            Debug.Log("shootingright");
        }
        else if (script.direction == global::player.Facing.Up)
        {
            shootingUp = true;
            Debug.Log("shootingup");
        }
        else if (script.direction == global::player.Facing.Down)
        {
            shootingDown = true;
            Debug.Log("shootingdown");
        }
        */
    }

    private void FixedUpdate()
    {
        transform.Translate(flyingSpeed*Time.deltaTime, 0, 0);
     }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("goal"))
        {
            goalscript = collision.gameObject.GetComponent<GoalScript>();
            if (!goalscript.hit)
            {
                goalscript.hit = true;
            } 
            else
            { 
                goalscript.hit = false; 
            }
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
