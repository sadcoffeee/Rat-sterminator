using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewPlayer : MonoBehaviour
{
    Animator anim;
    SpriteRenderer spriteRenderer;
    public GameObject arrow1;
    public GameObject arrow2;
    public GameObject arrow3;
    public GameObject bow;
    public int health;
    public float walkingSpeed;
    public float attackCooldown;
    float attackCooldownTracker;
    public float bigAttackCooldown;
    float bigAttackCooldownTracker;
    Vector3 oldposition;
    float IdleTime;
    bool healing;
    bool currentlyHurting;
    float lastDamageTime = 0f;


    public int powerlevel = 0;

    public float ability1Cooldown;
    public float ability2Cooldown;
    public float ability3Cooldown;
    public float ability4Cooldown;
    public float ability1CooldownTracker;
    public float ability2CooldownTracker;
    public float ability3CooldownTracker;
    public float ability4CooldownTracker;
    public bool ability1;
    public bool ability2;
    public bool ability3;
    public bool ability4;
    bool doubleShot;
    public UIAnimHandler UiStuff;
    Animator bowAnim;
    public int score;
    public CursorScript cursorScript;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        oldposition = transform.position;
        bowAnim = bow.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!UiStuff.paused)
        {
            if (attackCooldownTracker <= attackCooldown) { attackCooldownTracker += Time.deltaTime; } 

            // Attacks when click
            if (Input.GetMouseButtonDown(0) && attackCooldownTracker > 0.05f && attackCooldown > attackCooldownTracker) { StartCoroutine(Attack(false)); } 
            else if ( Input.GetMouseButtonDown(0) && attackCooldownTracker >= attackCooldown ) { StartCoroutine(Attack(true)); }

            if (bigAttackCooldownTracker <= bigAttackCooldown) { bigAttackCooldownTracker += Time.deltaTime; }

            if (Input.GetMouseButtonDown(1) && bigAttackCooldownTracker >= bigAttackCooldown)
            {
                bigAttack();
            }

            if (ability1) { if (ability1Cooldown > ability1CooldownTracker) { ability1CooldownTracker += Time.deltaTime; } else if (Input.GetKey(KeyCode.LeftShift)) { dashAbility(); } }
            if (ability2) { if (ability2Cooldown > ability2CooldownTracker) { ability2CooldownTracker += Time.deltaTime; } else if (Input.GetKey(KeyCode.LeftControl)) { StartCoroutine(ghostAbility()); } }
            if (ability3) { if (ability3Cooldown > ability3CooldownTracker) { ability3CooldownTracker += Time.deltaTime; } else if (Input.GetKey(KeyCode.Q)) { StartCoroutine(waveAbility()); } }
            if (ability4) { if (ability4Cooldown > ability4CooldownTracker) { ability4CooldownTracker += Time.deltaTime; } else if (Input.GetKey(KeyCode.E)) { StartCoroutine(doubleAbility()); } }

            if (Input.GetKeyDown(KeyCode.Alpha1)) { powerlevel = 0; bowAnim.SetInteger("powerlevel", powerlevel);}
            if (Input.GetKeyDown(KeyCode.Alpha2)) { powerlevel = 1; bowAnim.SetInteger("powerlevel", powerlevel); }

            if (Input.GetKeyDown(KeyCode.Alpha3)) { powerlevel = 2; bowAnim.SetInteger("powerlevel", powerlevel); }



            // Idle animation when you're not moving
            if (transform.position != oldposition)
            {
                oldposition = transform.position;
                anim.SetBool("idle", false);
                IdleTime = 0;
            }
            else
            {
                anim.SetBool("idle", true);
                IdleTime += Time.deltaTime;
            }

            if (Time.time - lastDamageTime > 4f && health < 10 && !healing)
            {
                healing = true;
                StartCoroutine(regen());
            }

            // Sends to death scene when health is 0
            if (health <= 0) { UiStuff.EndGame(true); }

            // Movement
            float horizontalInput = 0.0f;
            float verticalInput = 0.0f;

            if (Input.GetKey(KeyCode.D))
            {
                horizontalInput += 1.0f;
                spriteRenderer.flipX = false;
            }
            if (Input.GetKey(KeyCode.A))
            {
                horizontalInput -= 1.0f;
                spriteRenderer.flipX = true;
            }
            if (Input.GetKey(KeyCode.W))
            {
                verticalInput += 1.0f;
            }
            if (Input.GetKey(KeyCode.S))
            {
                verticalInput -= 1.0f;
            }

            Vector3 movement = new Vector3(horizontalInput, verticalInput, 0).normalized * walkingSpeed;
            transform.Translate(movement);

            updateBow();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (UiStuff.paused) { UiStuff.unPauseGame(); } else { UiStuff.pauseGame(); };
        }
    }
    IEnumerator regen()
    {
        UiStuff.toggleRegen();
        while (healing)
        {
            if (health < 10)
                health += 1;
            UiStuff.updateHealth(health);
            yield return new WaitForSeconds(1);
        }
        UiStuff.toggleRegen();
    }
        IEnumerator Attack(bool offCooldown)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 instantiationPosition = transform.position + new Vector3(0, 1.5f, 0);
            Vector3 direction = (mousePos - instantiationPosition).normalized;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            GameObject arrowInstance;


            if (powerlevel == 0)
            {
                if (doubleShot)
                {
                    arrowInstance = Instantiate(arrow1, instantiationPosition, Quaternion.AngleAxis(angle, Vector3.forward));
                    if (!offCooldown) { arrowInstance.transform.DOScaleX(attackCooldownTracker * 1.9f, 0f); arrowInstance.transform.DOScaleY(attackCooldownTracker * 1.9f, 0f); arrowInstance.GetComponent<NewArrowScript>().damage = (float)Math.Round(arrowInstance.GetComponent<NewArrowScript>().damage * attackCooldownTracker, 2);  }
                yield return new WaitForSeconds(0.1f);
                }
                arrowInstance = Instantiate(arrow1, instantiationPosition, Quaternion.AngleAxis(angle, Vector3.forward));
                if (!offCooldown) { arrowInstance.transform.DOScaleX(attackCooldownTracker * 1.9f, 0f); arrowInstance.transform.DOScaleY(attackCooldownTracker * 1.9f, 0f); arrowInstance.GetComponent<NewArrowScript>().damage = (float)Math.Round(arrowInstance.GetComponent<NewArrowScript>().damage * attackCooldownTracker, 2); }
        }
        else if (powerlevel == 1)
            {
                if (doubleShot)
                {
                    arrowInstance = Instantiate(arrow2, instantiationPosition, Quaternion.AngleAxis(angle, Vector3.forward));
                    if (!offCooldown) { arrowInstance.transform.DOScaleX(attackCooldownTracker * 1.9f, 0f); arrowInstance.transform.DOScaleY(attackCooldownTracker * 1.9f, 0f); arrowInstance.GetComponent<NewArrowScript>().damage = (float)Math.Round(arrowInstance.GetComponent<NewArrowScript>().damage * attackCooldownTracker, 2); }
                yield return new WaitForSeconds(0.1f);
                }
                arrowInstance = Instantiate(arrow2, instantiationPosition, Quaternion.AngleAxis(angle, Vector3.forward));
                if (!offCooldown) { arrowInstance.transform.DOScaleX(attackCooldownTracker * 1.9f, 0f); arrowInstance.transform.DOScaleY(attackCooldownTracker * 1.9f, 0f); arrowInstance.GetComponent<NewArrowScript>().damage = (float)Math.Round(arrowInstance.GetComponent<NewArrowScript>().damage * attackCooldownTracker, 2); }
        }
        else if (powerlevel == 2)
            {
                if (doubleShot)
                {
                    arrowInstance = Instantiate(arrow3, instantiationPosition, Quaternion.AngleAxis(angle, Vector3.forward));
                    if (!offCooldown) { arrowInstance.transform.DOScaleX(attackCooldownTracker * 1.9f, 0f); arrowInstance.transform.DOScaleY(attackCooldownTracker * 1.9f, 0f); arrowInstance.GetComponent<NewArrowScript>().damage = (float)Math.Round(arrowInstance.GetComponent<NewArrowScript>().damage * attackCooldownTracker, 2); }

                yield return new WaitForSeconds(0.1f);
                }
                arrowInstance = Instantiate(arrow3, instantiationPosition, Quaternion.AngleAxis(angle, Vector3.forward));
                if (!offCooldown) { arrowInstance.transform.DOScaleX(attackCooldownTracker * 1.9f, 0f); arrowInstance.transform.DOScaleY(attackCooldownTracker * 1.9f, 0f); arrowInstance.GetComponent<NewArrowScript>().damage = (float)Math.Round(arrowInstance.GetComponent<NewArrowScript>().damage * attackCooldownTracker, 2); }

        }

        attackCooldownTracker = 0;
        cursorScript.resetCooldown();
        StartCoroutine(UiStuff.attack1Cooldown());
        }

    void bigAttack()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 instantiationPosition = transform.position + new Vector3(0, 1.5f, 0);
        Vector3 direction = (mousePos - instantiationPosition).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        GameObject arrowInstance;


        if (powerlevel == 0)
        {
            arrowInstance = Instantiate(arrow1, instantiationPosition, Quaternion.AngleAxis(angle, Vector3.forward));
            arrowInstance.transform.DOScaleX(1.7f, 0.1f); arrowInstance.transform.DOScaleY(1.7f, 0.1f); arrowInstance.GetComponent<NewArrowScript>().damage = (arrowInstance.GetComponent<NewArrowScript>().damage * 1.5f);
        }
        else if (powerlevel == 1)
        {
            arrowInstance = Instantiate(arrow2, instantiationPosition, Quaternion.AngleAxis(angle, Vector3.forward));
            arrowInstance.transform.DOScaleX(1.7f, 0.1f); arrowInstance.transform.DOScaleY(1.7f, 0.1f); arrowInstance.GetComponent<NewArrowScript>().damage = (arrowInstance.GetComponent<NewArrowScript>().damage * 1.5f);
        }
        else if (powerlevel == 2)
        {
            arrowInstance = Instantiate(arrow3, instantiationPosition, Quaternion.AngleAxis(angle, Vector3.forward));
            arrowInstance.transform.DOScaleX(1.7f, 0.1f); arrowInstance.transform.DOScaleY(1.7f, 0.1f); arrowInstance.GetComponent<NewArrowScript>().damage = (arrowInstance.GetComponent<NewArrowScript>().damage * 1.5f);

        }

        bigAttackCooldownTracker = 0;
        StartCoroutine(UiStuff.attack2Cooldown());
    }
    void dashAbility()
        {
            ability1CooldownTracker = 0;
            UiStuff.TriggerAbility(1, ability1Cooldown);
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 direction = (mousePos - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            float dashDistance = 4.0f;
            Vector3 dashVector = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0) * dashDistance;

            StartCoroutine(DashCoroutine(dashVector, 0.15f));
        }

        IEnumerator DashCoroutine(Vector3 dashVector, float duration)
        {
            float timeElapsed = 0f;
            Vector3 startingPos = transform.position;
            Vector3 targetPos = transform.position + dashVector;
            anim.SetTrigger("dash");

            while (timeElapsed < duration)
            {
                timeElapsed += Time.deltaTime;
                transform.position = Vector3.Lerp(startingPos, targetPos, timeElapsed / duration);
                yield return null;
            }

        }
        IEnumerator ghostAbility()
        {
            ability2CooldownTracker = 0;
            UiStuff.TriggerAbility(2, ability2Cooldown);

            Color originalColor = spriteRenderer.color;
            Color invulnerableColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0.6f);
            spriteRenderer.color = invulnerableColor;


            gameObject.layer = LayerMask.NameToLayer("Invulnerable");


            yield return new WaitForSeconds(2.0f);


            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);
            gameObject.layer = LayerMask.NameToLayer("player");
        }
        IEnumerator waveAbility()
        {
            ability3CooldownTracker = 0;
            UiStuff.TriggerAbility(3, ability3Cooldown);

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 instantiationPosition = transform.position + new Vector3(0, 1.5f, 0);
            Vector3 direction = (mousePos - instantiationPosition).normalized;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            float angleLeft = angle - 30f;
            float angleRight = angle + 30f;

            List<GameObject> arrows = new List<GameObject>();
            arrows.Add(arrow1);
            arrows.Add(arrow2);
            arrows.Add(arrow3);

            foreach (GameObject arrow in arrows)
            {
                Instantiate(arrow, instantiationPosition, Quaternion.AngleAxis(angle, Vector3.forward));
                Instantiate(arrow, instantiationPosition, Quaternion.AngleAxis(angleLeft, Vector3.forward));
                Instantiate(arrow, instantiationPosition, Quaternion.AngleAxis(angleRight, Vector3.forward));
                yield return new WaitForSeconds(0.1f);
            }

        }
        IEnumerator doubleAbility()
        {
            ability4CooldownTracker = 0;
            UiStuff.TriggerAbility(4, ability4Cooldown);
            doubleShot = true;
            bowAnim.SetBool("bigbow", true);
            yield return new WaitForSeconds(6.0f);
            doubleShot = false;
            bowAnim.SetBool("bigbow", false);
        }

        void updateBow()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 playerMiddle = transform.position + new Vector3(0, 1.3f, 0);
            Vector3 direction = (mousePos - playerMiddle).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;


            bow.transform.position = playerMiddle + direction * 1.6f;
            bow.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        }
        public IEnumerator hurtCoroutine(int howmuch)
        {
            if (!currentlyHurting)
            {
            currentlyHurting = true;
            if (score >= 100) { score -= 50; }
            lastDamageTime = Time.time;
            healing = false; 
            anim.SetTrigger("hurt");
            health -= howmuch;
            UiStuff.updateHealth(health);
            yield return new WaitForSeconds(0.1f);
            currentlyHurting = false;
            }
    }
}
