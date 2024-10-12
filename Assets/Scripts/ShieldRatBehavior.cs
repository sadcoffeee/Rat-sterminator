using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldRatBehavior : MonoBehaviour
{

    float moveTimer;
    float moveThreshold;
    public float moveSpeed;
    GameObject player;
    Vector3 directionofplayer;
    EnemyHealth healthScript;
    Animator anim;
    bool doingSomething;
    SpriteRenderer spriteRenderer;


    private void Start()
    {
        anim = GetComponent<Animator>();
        healthScript = GetComponent<EnemyHealth>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        moveThreshold = 2f;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        StartCoroutine(whatDo());
    }

    IEnumerator whatDo()
    {
        if(healthScript.stunned) { yield return null; } 
        else if (!doingSomething)
        {
            if (moveTimer > moveThreshold)
            {
                yield return StartCoroutine(moveCoroutine());
            }
            else
            {
                moveTimer += Time.deltaTime;
            }
        }
    }
    IEnumerator moveCoroutine()
    {
        directionofplayer = new Vector3(player.transform.position.x - transform.position.x, player.transform.position.y + 1 - transform.position.y, player.transform.position.z - transform.position.z).normalized;

        float timeElapsed = 0f;
        float duration = 1.5f;
        Vector3 startingPos = transform.position;
        Vector3 targetPos = transform.position + directionofplayer * moveSpeed; // multiply movespeed by a randomrange of 0.8 to 1.2 for some variance in movement length?

        if (directionofplayer.x < 0)
        {
            spriteRenderer.flipX = false;
        }
        if (directionofplayer.x > 0)
        {
            spriteRenderer.flipX = true;
        }
        while (timeElapsed < duration)
        {
            anim.SetBool("moving", true);
            timeElapsed += Time.deltaTime;
            transform.position = Vector3.Lerp(startingPos, targetPos, timeElapsed / duration);
            yield return null;
        }
        anim.SetBool("moving", false);
        moveTimer = 0f;
        doingSomething = false;
        moveThreshold = Random.Range(1f, 3f);
    }
}
