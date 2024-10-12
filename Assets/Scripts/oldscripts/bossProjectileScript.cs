using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossProjectileScript : MonoBehaviour
{
    public float travelSpeed;
    player playerScript;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(-travelSpeed * Time.deltaTime, 0f, 0f);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("boss")) 
        {
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            playerScript = collision.gameObject.GetComponent<player>();
            playerScript.health -= 1;
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
