using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basicRatBehavior : MonoBehaviour
{
    GameObject player;
    Vector3 directionofplayer;
    public float movementSpeed;
    bool isStunned;
    Animator anim;
    EnemyHealth healthScript;
    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        healthScript = GetComponent<EnemyHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!healthScript.stunned)
        {
            directionofplayer = new Vector3(player.transform.position.x - transform.position.x, player.transform.position.y + 1 - transform.position.y, player.transform.position.z - transform.position.z);
            if (directionofplayer.x > 0) { spriteRenderer.flipX = true; } else { spriteRenderer.flipX = false; }
            Vector3 normalizedDirection = directionofplayer.normalized;
            transform.Translate(normalizedDirection * movementSpeed * Time.deltaTime);
        }
    }
}
