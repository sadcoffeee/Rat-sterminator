using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossbehaviour : MonoBehaviour
{
    public int bossHealth;
    public float bossSpeed;
    public float attackCooldown;
    public float superattackCooldown;
    public GameObject treasure;
    public GameObject bossattack;
    public GameObject bossattack2;
    SpriteRenderer spriteRenderer;
    player playerScript;
    float knockbackSpeed = 0.3f;
    float timer;
    private int lastAttackTime;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        timer = 0;
        lastAttackTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (bossHealth <= 0)
        {
            Instantiate (treasure, new Vector3(transform.position.x, transform.position.y, 0f), Quaternion.Euler(0, 0, 0));
            Destroy(gameObject);
        }

        timer = timer + Time.deltaTime;
        if (Mathf.FloorToInt(timer) % superattackCooldown == 0 && Mathf.FloorToInt(timer) > lastAttackTime)
        {
            Instantiate(bossattack2, new Vector3(transform.position.x - 2f, transform.position.y + 1f, 0f), Quaternion.Euler(0, 0, 0));
            lastAttackTime = Mathf.FloorToInt(timer);
        }
        if (Mathf.FloorToInt(timer) % attackCooldown == 0 && Mathf.FloorToInt(timer) > lastAttackTime)
        {
            Instantiate(bossattack, new Vector3(transform.position.x - 2f, transform.position.y + 1f, 0f), Quaternion.Euler(0, 0, 0));
            lastAttackTime = Mathf.FloorToInt(timer);
        }
    }
    private void FixedUpdate()
    {
        //walk at a regular pace
        transform.Translate(0f, bossSpeed * Time.deltaTime, 0f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //on player hit, reduce player health by one, knock player back
        if (collision.gameObject.CompareTag("Player")) 
        {
            playerScript = collision.gameObject.GetComponent<player>();
            playerScript.health --;
            Vector3 backwards = new Vector3(collision.gameObject.transform.position.x - 5f, collision.gameObject.transform.position.y, collision.gameObject.transform.position.z);
            collision.gameObject.transform.position = Vector3.Lerp(collision.gameObject.transform.position, backwards, knockbackSpeed);
        }
        //on arrow hit, reduce own hp by one
        else if (collision.gameObject.CompareTag("arrow"))
        {
            bossHealth--;
        }
        //on other collision, set the transform Up to the opposite of what it was and flip localscale y axis so your character stays upright
        else
        {
            if (transform.localScale == new Vector3(1,-1,1))
            {
                transform.localScale = new Vector3(1, 1, 1);
                transform.up = new Vector3(0f, 5f, 0f);
            }
            else
            {
                transform.localScale = new Vector3(1, -1, 1);
                transform.up = new Vector3(0f, -20f, 0f);
            }
         }
     }
 }
