using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewArrowScript : MonoBehaviour
{
    public float flyingSpeed;
    public float damage;
    public bool canStun;
    public bool shieldBreak;
    public int enemyPenetration= 3;
    int hitCount = 0;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(flyingSpeed * Time.deltaTime, 0, 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("enemy") || other.gameObject.CompareTag("boss"))
        {
            EnemyHealth enemy = other.gameObject.GetComponent<EnemyHealth>();
            if (shieldBreak)
            {
                enemy.breakShield();
            }
            if (!enemy.shielded)
            {
                StartCoroutine(enemy.TakeDamage(damage));
                hitCount++;

                if (canStun)
                {
                    StartCoroutine(enemy.StunCoroutine(enemy.stunTime));
                }
            }
            else
            {
                StartCoroutine(enemy.BlockCoroutine());
                hitCount = 100;
            }
            
            if (hitCount >= enemyPenetration)
            {
                Destroy(gameObject); 
            }
        }
        else { Destroy(gameObject); }
    }
}
