using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Transactions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIAnimHandler : MonoBehaviour
{
    public GameObject ability1;
    public GameObject ability2;
    public GameObject ability3;
    public GameObject ability4;
    Animator ability1Anim;
    Animator ability2Anim;
    Animator ability3Anim;
    Animator ability4Anim;
    public GameObject regenStuff;
    public GameObject WaveText;
    public TextMeshProUGUI WaveTextChild;
    public Image healthbar;
    bool regenning;
    List<Sprite> regenImages;
    public NewPlayer playerScript;
    public WaveManager waveManager;

    List<Sprite> abilityImages;
    public GameObject selectionPanel;
    public Image PanelImage1;
    public Image PanelImage2;
    public TextMeshProUGUI PanelTitle;
    public TextMeshProUGUI PanelDesc;
    public int currentlySelectedAbility;
    bool doneOnce;

    public GameObject pauseMenu;
    public bool paused;
    public GameObject uWonMenu;
    public GameObject scoreMenu;

    public GameObject score1;
    public GameObject score2;
    public GameObject score3;
    public GameObject score4;
    public GameObject score5;
    public GameObject score6;

    public GameObject attack1CD;
    public GameObject attack2CD;

    private void Start()
    {
        doneOnce = false;
        paused = false;
        Time.timeScale = 1f;
        regenImages = new List<Sprite>();
        foreach (Transform child in regenStuff.transform)
        {
            regenImages.Add(child.GetComponent<Image>().sprite);
        }
        abilityImages = new List<Sprite>();
        foreach (Transform child in PanelTitle.transform)
        {
            abilityImages.Add(child.GetComponent<Image>().sprite);
        }
        ability1Anim = ability1.GetComponent<Animator>();
        ability2Anim = ability2.GetComponent<Animator>();
        ability3Anim = ability3.GetComponent<Animator>();
        ability4Anim = ability4.GetComponent<Animator>();
        StartCoroutine(attack2Cooldown());
    }
    public void activateAbility()
    {
        waveManager.currentlySelectingAbility = false;
        if (!doneOnce) doneOnce = true;
        if (currentlySelectedAbility == 1) { ability1.SetActive(true);  playerScript.ability1 = true; playerScript.ability1CooldownTracker = playerScript.ability1Cooldown; }
        if (currentlySelectedAbility == 2) { ability2.SetActive(true); playerScript.ability2 = true; playerScript.ability2CooldownTracker = playerScript.ability2Cooldown; }
        if (currentlySelectedAbility == 3) { ability3.SetActive(true); playerScript.ability3 = true; playerScript.ability3CooldownTracker = playerScript.ability3Cooldown; }
        if (currentlySelectedAbility == 4) { ability4.SetActive(true); playerScript.ability4 = true; playerScript.ability4CooldownTracker = playerScript.ability4Cooldown; }
    }

    public void TriggerAbility(int abilityNumber, float cooldown)
    {
        if (abilityNumber == 1) { ability1Anim.SetFloat("animspeed", 1 / cooldown); ability1Anim.SetTrigger("used"); }
        if (abilityNumber == 2) { ability2Anim.SetFloat("animspeed", 1 / cooldown); ability2Anim.SetTrigger("used"); }
        if (abilityNumber == 3) { ability3Anim.SetFloat("animspeed", 1 / cooldown); ability3Anim.SetTrigger("used"); }
        if (abilityNumber == 4) { ability4Anim.SetFloat("animspeed", 1 / cooldown); ability4Anim.SetTrigger("used"); }
    }

    public void toggleRegen() 
    {
        if (!regenning)
        {
            regenning = true;
            regenStuff.SetActive(true);
        }
        else
        {
            regenning = false;
            regenStuff.SetActive(false);
        }
    }

    public void updateHealth(int hp)
    {
        if (hp < 0) { healthbar.sprite = regenImages[10]; }
        else
        {
            healthbar.sprite = regenImages[10 - hp];
        }
    }

    public IEnumerator setWaveText(string text)
    {
        WaveTextChild.text = text;
        print(text);
        yield return new WaitForSeconds(0.1f);
        WaveText.SetActive(true);
    }
    public void showPanel()
    {
        if (doneOnce)
        {
            //set it with the movement abilities
            PanelTitle.text = "Movement Abilities";
            PanelDesc.text = "Pick a movement ability to unlock";
            PanelImage1.sprite = abilityImages[0];
            PanelImage2.sprite = abilityImages[1];
            selectionPanel.SetActive(true);
            
        }
        else
        {
            //set it with the dmg abilities
            PanelTitle.text = "Damage Abilities";
            PanelDesc.text = "Pick a damage ability to unlock";
            PanelImage1.sprite = abilityImages[2];
            PanelImage2.sprite = abilityImages[3];
            selectionPanel.SetActive(true);
        }
    }
    public void clickPanelAbility(int whichSide)
    {
        if (doneOnce)
        {
            if (whichSide == 0)
            {
                PanelImage1.color = new Color(0.8f, 0.8f, 0.8f);
                PanelImage2.color = Color.white;
                PanelTitle.text = "Dash";
                PanelDesc.text = "Click 'Left Shift' to perform a quick dash in your mouse direction";
                currentlySelectedAbility = 1;
            }
            if (whichSide == 1)
            {
                PanelImage1.color = Color.white;
                PanelImage2.color = new Color(0.8f, 0.8f, 0.8f);
                PanelTitle.text = "Ghost";
                PanelDesc.text = "Click 'Left Control' to become invulnerable for a short duration";
                currentlySelectedAbility = 2;
            }
        }
        else
        {
            if (whichSide == 0)
            {
                PanelImage1.color = new Color(0.8f, 0.8f, 0.8f);
                PanelImage2.color = Color.white;
                PanelTitle.text = "Wave Shot";
                PanelDesc.text = "Click 'Q' to rapidly shoot a wave of different arrows in your mouse direction";
                currentlySelectedAbility = 3;
            }
            if (whichSide == 1)
            {
                PanelImage1.color = Color.white;
                PanelImage2.color = new Color(0.8f, 0.8f, 0.8f);
                PanelTitle.text = "Double Shot";
                PanelDesc.text = "Click 'E' to temporarily empower your bow, allowing it to shoot twice as much";
                currentlySelectedAbility = 4;
            }
        }
    }
    public void pauseGame()
    {
        Time.timeScale = 0f;
        paused = true;
        pauseMenu.SetActive(true);
    }
    public void unPauseGame()
    {
        pauseMenu.SetActive(false);
        paused = false;
        Time.timeScale = 1f;
    }
    public void wavesCompleted()
    {
        uWonMenu.SetActive(true);
        Time.timeScale = 0f;
        paused = true;
    }
    public void EndGame(bool died)
    {
        if (died) { scoreMenu.GetComponent<Image>().color = new Color(1.0f, 0.7f, 0.7f); }
        score1.GetComponent<TextMeshProUGUI>().text = waveManager.wave1Score.ToString();
        score2.GetComponent<TextMeshProUGUI>().text = waveManager.wave2Score.ToString();
        score3.GetComponent<TextMeshProUGUI>().text = waveManager.wave3Score.ToString();
        score4.GetComponent<TextMeshProUGUI>().text = waveManager.wave4Score.ToString();
        score5.GetComponent<TextMeshProUGUI>().text = waveManager.wave5Score.ToString();
        score6.GetComponent<TextMeshProUGUI>().text = (playerScript.score - (waveManager.wave1Score + waveManager.wave2Score + waveManager.wave3Score + waveManager.wave4Score + waveManager.wave5Score)).ToString();

        scoreMenu.SetActive(true);
        Time.timeScale = 0f;
        paused = true;
    }
    public IEnumerator attack1Cooldown()
    {
        float counter = 0;

        while (counter < playerScript.attackCooldown) 
        {
            counter += Time.deltaTime;
            attack1CD.GetComponent<Slider>().value = 1 - (counter / playerScript.attackCooldown);
            yield return null;
        }
        attack1CD.GetComponent<Slider>().value = 0;
    }
    public IEnumerator attack2Cooldown()
    {
        float counter = 0;

        while (counter < playerScript.bigAttackCooldown)
        {
            counter += Time.deltaTime;
            attack2CD.GetComponent<Slider>().value = 1 - (counter / playerScript.bigAttackCooldown);
            yield return null;
        }
        attack2CD.GetComponent<Slider>().value = 0;
    }
}
