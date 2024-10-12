using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ratcontroller : MonoBehaviour
{
    GameObject player;
    Vector3 directionofplayer;
    public float movementSpeed;
    player playerScript;
    float knockbackSpeed = 0.3f;
    float timer;
    bool isStunned;
    Animator anim;
    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        directionofplayer = new Vector3(player.transform.position.x - transform.position.x, player.transform.position.y + 1 - transform.position.y, player.transform.position.z - transform.position.z);
        if (!isStunned)
        {
            transform.up = directionofplayer;
        }
    }

    private void FixedUpdate()
    {
        if (!isStunned)
        {
            transform.Translate(0f, movementSpeed * Time.deltaTime, 0f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isStunned)
        {
            playerScript = collision.gameObject.GetComponent<player>();
            playerScript.health -= 1;
            Vector3 backwards = new Vector3(transform.position.x+2f, transform.position.y, transform.position.z);
            gameObject.transform.position = Vector3.Lerp(transform.position, backwards, knockbackSpeed);
            Stun(4);
        }
    }
    public void Stun(float duration)
    {
        StartCoroutine(StunCoroutine(duration));
    }

    private IEnumerator StunCoroutine(float duration)
    {
        isStunned = true;
        anim.Play("stunned");
        print(isStunned);

        yield return new WaitForSeconds(duration);

        isStunned = false;
        anim.Play("ratrun");
    }
}
