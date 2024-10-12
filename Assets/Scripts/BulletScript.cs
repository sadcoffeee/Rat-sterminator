using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BulletScript : MonoBehaviour
{
    public float speed = 10f;
    public int damage;
    private Vector3 direction;
    NewPlayer playerScript;
    public Animator animator;
    public BoxCollider2D myCollider;
    bool dead;

    public void SetDirection(Vector3 newDirection)
    {
        direction = newDirection;
    }

    void Update()
    {
        if(direction == Vector3.zero) { direction = Vector2.down; }
        
        if (!dead) transform.position += direction * speed * Time.deltaTime;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerScript = collision.gameObject.GetComponent<NewPlayer>();
            StartCoroutine(playerScript.hurtCoroutine(damage));
        }
        StartCoroutine(destroyProjectile());
    }
    IEnumerator destroyProjectile()
    {
        dead = true;
        if (animator != null)
        {
            Destroy(gameObject, 0.3f);
            myCollider.enabled = false;
            animator.SetTrigger("dead");
        } else Destroy(gameObject);
        yield return null;

    }
}
