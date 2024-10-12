using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalScript : MonoBehaviour
{
    public GameObject turnoff;
    public GameObject turnon;
    public bool hit;
    // Start is called before the first frame update
    void Start()
    {
        hit = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (hit)
        {
            for (int i = 0; i < turnoff.transform.childCount; i++)
            {
                turnoff.transform.GetChild(i).gameObject.SetActive(false);
            }
            for (int i = 0; i < turnon.transform.childCount; i++)
            {
                turnon.transform.GetChild(i).gameObject.SetActive(true);
            }

        }
        if (!hit)
        {
            for (int i = 0; i < turnoff.transform.childCount; i++)
            {
                turnoff.transform.GetChild(i).gameObject.SetActive(true);
            }
            for (int i = 0; i < turnon.transform.childCount; i++)
            {
                turnon.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
