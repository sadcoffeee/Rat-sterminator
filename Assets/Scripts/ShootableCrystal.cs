using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootableCrystal : MonoBehaviour
{
    bool shot;
    private void Start()
    {
        shot = false;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("redarrow") || collision.gameObject.CompareTag("yellowarrow") || collision.gameObject.CompareTag("greenarrow"))
        {
            if (!shot)
            {
                shot = true;
                GetComponent<WaveManager>().Start();
            }
        }
    }
}
