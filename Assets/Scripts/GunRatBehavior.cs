using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRatBehavior : MonoBehaviour
{
    float moveTimer;
    float moveThreshold;
    bool moving;
    public float moveSpeed;
    GameObject player;
    Vector3 directionofplayer;
    EnemyHealth healthScript;
    Animator anim;
    public GameObject bullet;
    bool doingSomething;
    SpriteRenderer spriteRenderer;

    private void Start()
    {
        anim = GetComponent<Animator>();
        healthScript = GetComponent<EnemyHealth>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        moveThreshold = 3f;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        StartCoroutine(whatDo());
    }

    IEnumerator whatDo() 
    {
        if (healthScript.stunned) { yield return null; }
        else if (!doingSomething)
        {
            if (moveTimer < moveThreshold) { moveTimer += Time.deltaTime; }
            else
            {
                float coinflip = Random.Range(0, 2);
                if (coinflip < 0.5f) { doingSomething = true; yield return StartCoroutine(moveCoroutine()); }
                else { doingSomething = true; StartCoroutine(shootCoroutine()); }
            }
        }
    }

    IEnumerator moveCoroutine()
    {
        directionofplayer = new Vector3(player.transform.position.x - transform.position.x, player.transform.position.y + 1 - transform.position.y, player.transform.position.z - transform.position.z).normalized;
        Quaternion notWalkingStraightAhead = Quaternion.Euler(0, 0, (Random.Range(-60f, 60f) * Mathf.Deg2Rad));
        Vector3 skewedDirection = notWalkingStraightAhead * directionofplayer;

        float timeElapsed = 0f;
        float duration = 1f;
        Vector3 startingPos = transform.position;
        //or if directionofplayer vector is shorter than like 5  units make it do the same move calculation but then reverse it by 180 (need to do it before normalizing the vector)
        Vector3 targetPos = transform.position + skewedDirection * moveSpeed; // multiply movespeed by a randomrange of 0.8 to 1.2 for some variance in movement length?
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
        moveThreshold = Random.Range(2f, 4f);
        moveTimer = 0f;
        doingSomething = false;
    }
    IEnumerator shootCoroutine()
    {
        anim.SetTrigger("shoot");
        yield return new WaitForSeconds(0.2f);
        Vector3 directionOfPlayer = new Vector3(player.transform.position.x - transform.position.x, player.transform.position.y + 1 - transform.position.y, player.transform.position.z - transform.position.z).normalized;

        GameObject bulletInstance = Instantiate(bullet, transform.position, Quaternion.identity);

        BulletScript bulletScript = bulletInstance.GetComponent<BulletScript>();
        if (bulletScript != null)
        {
            bulletScript.SetDirection(directionOfPlayer);
        }
        
        moveThreshold = Random.Range(1f, 3f);
        moveTimer = 0f;
        doingSomething = false;
     }
}
