using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newboss : MonoBehaviour
{
    public float bossSpeed;
    public float attackCooldown;
    public float superattackCooldown;
    public GameObject bossattack;
    public GameObject bossattack2;
    SpriteRenderer spriteRenderer;
    EnemyHealth health;

    float timer;
    private int lastAttackTime;

    // Start is called before the first frame update
    void Start()
    {
        health = GetComponent<EnemyHealth>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        timer = 0;
        lastAttackTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!health.stunned)
        {
            timer = timer + Time.deltaTime;
            if (Mathf.FloorToInt(timer) % superattackCooldown == 0 && Mathf.FloorToInt(timer) > lastAttackTime)
            {
                Instantiate(bossattack2, new Vector3(transform.position.x, transform.position.y - 1f, 0f), Quaternion.Euler(0, 0, 0));
                lastAttackTime = Mathf.FloorToInt(timer);
            }
            if (Mathf.FloorToInt(timer) % attackCooldown == 0 && Mathf.FloorToInt(timer) > lastAttackTime)
            {
                Instantiate(bossattack, new Vector3(transform.position.x, transform.position.y - 1f, 0f), Quaternion.Euler(0, 0, 0));
                lastAttackTime = Mathf.FloorToInt(timer);
            }

            //walk at a regular pace
            transform.Translate(bossSpeed * Time.deltaTime, 0f, 0f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("bosswall"))

        {
            if (transform.localScale == new Vector3(-1, 1, 1))
            {
                transform.localScale = new Vector3(1, 1, 1);
                transform.right = new Vector3(1, 0, 0);
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 1);
                transform.right = new Vector3(-1, 0, 0);
            }
        }
    }
}
