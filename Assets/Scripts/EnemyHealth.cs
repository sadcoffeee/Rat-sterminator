using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class EnemyHealth : MonoBehaviour
{
    public float health;
    public int damageDealing;
    public bool stunned;
    public bool shielded;
    public float stunTime;
    public GameObject damageNumber;
    NewPlayer playerScript;
    bool dead;
    public int score;
    BoxCollider2D myCollider;


    public Animator anim;


    void Start()
    {
        anim = GetComponent<Animator>();
        myCollider = GetComponent<BoxCollider2D>();
    }
    void Update()
    {
        if (health <= 0 && !dead) { StartCoroutine(death()); }

    }
    public IEnumerator TakeDamage(float damage)
    {
        GameObject dmgNum = Instantiate(damageNumber, transform);

        health -= damage;
        TextMeshPro text = dmgNum.GetComponent<TextMeshPro>();
        text.text = damage.ToString();

        dmgNum.SetActive(true);
        yield return new WaitForSeconds(1);
        if (dmgNum != null) { Destroy(dmgNum); }
    }
    public IEnumerator death()
    {
        dead = true;
        myCollider.enabled = false;
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<NewPlayer>();
        playerScript.score += score;
        StartCoroutine(StunCoroutine(1.2f));
        SpriteRenderer guh = GetComponent<SpriteRenderer>();
        guh.color = new Color(1f, 0.6f, 0.6f, 0.6f);

        yield return new WaitForSeconds(0.6f);
        guh.color = new Color(1f, 0.5f, 0.5f, 0.3f);

        yield return new WaitForSeconds(0.6f);
        Destroy(gameObject);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // if collision is player, deal damage & stun this 
        if (collision.gameObject.CompareTag("Player") && !stunned)
        {
            StartCoroutine(StunCoroutine(stunTime));
            playerScript = collision.gameObject.GetComponent<NewPlayer>();
            StartCoroutine(playerScript.hurtCoroutine(damageDealing));
            print("hit player");
        }

        
    }
    public IEnumerator StunCoroutine(float duration)
    {
        stunned = true;
        anim.SetBool("stunned", true);

        yield return new WaitForSeconds(duration);

        stunned = false;

        if (anim != null)
        {
           anim.SetBool("stunned", false);
        }
    }
    public IEnumerator BlockCoroutine()
    {
        anim.SetTrigger("block");

        yield return new WaitForSeconds(0.1f);
       
    }

    public void breakShield() 
    {
        if (shielded)
        {
            anim.SetBool("shielded", false );
            shielded = false;
        }
    }
}
