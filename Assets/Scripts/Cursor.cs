using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CursorScript : MonoBehaviour
{
    public Texture2D cursor1, cursor2, cursor3, cursor1CD, cursor2CD, cursor3CD;
    Vector2 hotspot = new Vector2(32, 32);
    NewPlayer player;
    int lastPowerlevel;
    public bool Cooldown;
    float cooldownCounter;

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null) { player = GameObject.FindGameObjectWithTag("Player").GetComponent<NewPlayer>(); }
        else player = gameObject.AddComponent<NewPlayer>();
        Cursor.SetCursor(cursor1, hotspot, CursorMode.ForceSoftware);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Cooldown) 
        {
            cooldownCounter += Time.deltaTime;
            if (cooldownCounter > 0.5f ) { Cooldown = false; StartCoroutine(cooldownFinish()); }

        }
        if (player.powerlevel != lastPowerlevel) 
        { 
            lastPowerlevel = player.powerlevel;
            
            if (player.powerlevel == 0)
            {
                Cursor.SetCursor(cursor1, hotspot, CursorMode.ForceSoftware);
            }
            else if (player.powerlevel == 1) 
            {
                Cursor.SetCursor(cursor2, hotspot, CursorMode.ForceSoftware);
            }
            else if (player.powerlevel == 2)
            {
                Cursor.SetCursor(cursor3, hotspot, CursorMode.ForceSoftware);
            }
        }
    }
    public void resetCooldown()
    {
        cooldownCounter = 0;
        Cooldown = true;
    }
    IEnumerator cooldownFinish()
    {
        if (player.powerlevel == 0)
        {
            Cursor.SetCursor(cursor1CD, hotspot, CursorMode.ForceSoftware);
            yield return new WaitForSeconds(0.2f);
            Cursor.SetCursor(cursor1, hotspot, CursorMode.ForceSoftware);

        }
        else if (player.powerlevel == 1)
        {
            Cursor.SetCursor(cursor2CD, hotspot, CursorMode.ForceSoftware);
            yield return new WaitForSeconds(0.2f);
            Cursor.SetCursor(cursor2, hotspot, CursorMode.ForceSoftware);
        }
        else if (player.powerlevel == 2)
        {
            Cursor.SetCursor(cursor3CD, hotspot, CursorMode.ForceSoftware);
            yield return new WaitForSeconds(0.2f);
            Cursor.SetCursor(cursor3, hotspot, CursorMode.ForceSoftware);
        }

    }

}
