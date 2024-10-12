using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    List<GameObject> spawnPoints;
    List<WaveItem> waves;
    bool enemiesLeft;
    public TextMeshProUGUI waveTextObject;
    public GameObject spawnPointHolder;
    public GameObject bossSpawn;
    public GameObject basicEnemy;
    public GameObject shieldEnemy;
    public GameObject gunEnemy;
    public GameObject bossEnemy;
    public int howmanywaves;
    Animator crystal;
    public UIAnimHandler UIStuff;
    public bool currentlySelectingAbility;

    public int wave1Score;
    public int wave2Score;
    public int wave3Score;
    public int wave4Score;
    public int wave5Score;
    public int totalScore;
    public NewPlayer player;

    public void Start()
    {
        spawnPoints = new List<GameObject>();
        foreach (Transform child in spawnPointHolder.transform)
        {
            spawnPoints.Add(child.gameObject);
        }

        waves = new List<WaveItem>();
        makeWaves(howmanywaves);

        crystal = GetComponent<Animator>();

        StartCoroutine(goTime());
        
    }

    public class WaveItem
    {
        public int basicEnemies;
        public int shieldEnemies;
        public int shootEnemies;
        public int bossEnemies;

        public int waveTime;
        public string waveText;
    }
    /*
    public void WaveSpawner(WaveItem waveItem)
    {
        // maybe replace this with a doTween animation?
        waveTextObject.text = waveItem.waveText;
        enemiesLeft = true;

        float coroutineTime = waveItem.waveTime / 10;
        int coroutineBasic = Mathf.RoundToInt(waveItem.basicEnemies / 10);
        int coroutineShield = Mathf.RoundToInt(waveItem.shieldEnemies / 10);
        int coroutineGun = Mathf.RoundToInt(waveItem.shootEnemies / 10);
        int coroutineBoss = waveItem.bossEnemies;
        int coroutineCounter = 0;

        while (coroutineCounter < 9)
        {
            SpawnCoroutine(coroutineTime, coroutineBasic, coroutineShield, coroutineGun, 0);
            coroutineCounter++;
            while (coroutineCounter < 10)
            {
                SpawnCoroutine(coroutineTime, coroutineBasic, coroutineShield, coroutineGun, coroutineBoss);
                coroutineCounter++;
            }
        }
        while (enemiesLeft)
        {
            CheckCoroutine();
        }
    }
    private IEnumerator SpawnCoroutine(float duration, int basic, int shield, int gun, int boss)
    {
        while (basic > 0) { Instantiate(basicEnemy, spawnPoints[Random.Range(0,6)].transform); basic--; }
        yield return new WaitForSeconds(duration / 8);

        while (shield > 0) { Instantiate(shieldEnemy, spawnPoints[Random.Range(0, 6)].transform); shield--; }
        yield return new WaitForSeconds(duration / 4);

        while (gun > 0) { Instantiate(gunEnemy, spawnPoints[Random.Range(0, 6)].transform); gun--; }
        yield return new WaitForSeconds(duration / 2);

        //instantiate boss
        yield return new WaitForSeconds(duration / 8);

    }
    private IEnumerator CheckCoroutine()
    {
        yield return new WaitForSeconds(5);
        enemiesLeft = checkForEnemies();
    }
    public bool checkForEnemies() 
    {
        if (GameObject.FindGameObjectsWithTag("Enemy") != null) { return true; } return false;
    }
    */
    IEnumerator goTime()
    {
        int waveCounter = 1;
        foreach (WaveItem wave in waves) 
        {
            StartCoroutine(UIStuff.setWaveText(wave.waveText));
            crystal.SetTrigger("slightAngry");
            yield return new WaitForSeconds(10);
            enemiesLeft = true;
            yield return StartCoroutine(SpawnWave(wave));
            if (waveCounter == 1) { wave1Score = player.score; totalScore += wave1Score; }
            else if (waveCounter == 2) { wave2Score = player.score - totalScore; totalScore += wave2Score; }
            else if (waveCounter == 3) {  wave3Score = player.score - totalScore; totalScore += wave3Score; }
            else if (waveCounter == 4) {  wave4Score = player.score - totalScore; totalScore += wave4Score; }
            else if (waveCounter == 5) { wave5Score = player.score - totalScore; totalScore += wave5Score; }
            waveCounter++;
            if (waveCounter == 2 || waveCounter == 4) { UIStuff.showPanel(); yield return StartCoroutine(CheckAbilitySelected()); }
        }
        crystal.SetTrigger("dead");
        UIStuff.wavesCompleted();
    }
    private IEnumerator SpawnWave(WaveItem waveItem)
    {
        float coroutineTime = waveItem.waveTime / 10;
        int coroutineBasic = Mathf.RoundToInt(waveItem.basicEnemies / 10);
        int coroutineShield = Mathf.RoundToInt(waveItem.shieldEnemies / 10);
        int coroutineGun = Mathf.RoundToInt(waveItem.shootEnemies / 10);
        int coroutineBoss = waveItem.bossEnemies;
        int coroutineCounter = 0;
        crystal.SetTrigger("bigAngry");

        yield return StartCoroutine(SpawnCoroutine(coroutineTime, coroutineBasic, coroutineShield, coroutineGun, coroutineBoss));

        while (coroutineCounter < 9)
        {
            yield return StartCoroutine(SpawnCoroutine(coroutineTime, coroutineBasic, coroutineShield, coroutineGun, 0));
            coroutineCounter++;
        }
        yield return StartCoroutine(CheckEnemies());
    }

    private IEnumerator SpawnCoroutine(float duration, int basic, int shield, int gun, int boss)
    {
        if (boss > 0)
        {
            Instantiate(bossEnemy, bossSpawn.transform);
        }

        while (basic > 0)
        {
            Instantiate(basicEnemy, spawnPoints[Random.Range(0, 5)].transform);
            basic--;
            //yield return new WaitForSeconds(duration / 8);
        }

        yield return new WaitForSeconds(duration / 4);

        while (shield > 0)
        {
            Instantiate(shieldEnemy, spawnPoints[Random.Range(0, 5)].transform);
            shield--;
            //yield return new WaitForSeconds(duration / 4);
        }

        yield return new WaitForSeconds(duration / 2);

        while (gun > 0)
        {
            Instantiate(gunEnemy, spawnPoints[Random.Range(0, 5)].transform);
            gun--;
            //yield return new WaitForSeconds(duration / 4);
        }
        yield return new WaitForSeconds(duration / 4);
    }
    private IEnumerator CheckAbilitySelected()
    {
        currentlySelectingAbility = true;

        while (currentlySelectingAbility)
        {
            yield return new WaitForSeconds(2);
        }
    }

    private IEnumerator CheckEnemies()
    {
        while (enemiesLeft)
        {
            yield return new WaitForSeconds(5);
            enemiesLeft = checkForEnemies();
        }
    }

    public bool checkForEnemies()
    {
        if (GameObject.FindGameObjectsWithTag("boss").Length > 0) { return true; }
        
        else
        {
            return GameObject.FindGameObjectsWithTag("enemy").Length > 0;
        }
    }
    public void startInfiniteMode()
    {
        UIStuff.paused = false;
        Time.timeScale = 1f;
        for (int i = 1; i < 5; i++)
        {
            UIStuff.currentlySelectedAbility = i;
            UIStuff.activateAbility();
        }
        StartCoroutine(infiniteMode());
    }
    private IEnumerator infiniteMode()
    {
        bool urDoomed = true;
        float difficulty = 1.0f;

        while (urDoomed) 
        {
            difficulty += 0.1f;
            if (GameObject.FindGameObjectsWithTag("boss").Length > 0)
            {
                yield return StartCoroutine(SpawnCoroutine(5f, (int)(difficulty * Random.Range(2, 5)), (int)(difficulty * Random.Range(1, 3)), (int)difficulty * Random.Range(1, 3), 0));
            }
            else
            {
                yield return StartCoroutine(SpawnCoroutine(20f, (int)(difficulty * Random.Range(2, 8)), (int)(difficulty * Random.Range(5, 10)), (int)difficulty * Random.Range(5, 10), 1));
            } 

            yield return new WaitForSeconds(5);
        }
    }

    void makeWaves(int howmany)
    {
        if (howmany > 0) 
        {
            waves.Add(new WaveItem
            {
                basicEnemies = 30,
                shieldEnemies = 0,
                shootEnemies = 0,
                bossEnemies = 0,
                waveTime = 40,
                waveText = "Wave 1: Basic Enemies"
            });
        }
        if (howmany > 1)
        {
            waves.Add(new WaveItem
            {
                basicEnemies = 20,
                shieldEnemies = 10,
                shootEnemies = 0,
                bossEnemies = 0,
                waveTime = 40,
                waveText = "Wave 2: Shield Enemies"
            });
        }
        if (howmany > 2)
        {
            waves.Add(new WaveItem
            {
                basicEnemies = 20,
                shieldEnemies = 10,
                shootEnemies = 10,
                bossEnemies = 0,
                waveTime = 60,
                waveText = "Wave 3: Watch Out For Bullets"
            });
        }
        if (howmany > 3)
        {
            waves.Add(new WaveItem
            {
                basicEnemies = 30,
                shieldEnemies = 20,
                shootEnemies = 20,
                bossEnemies = 0,
                waveTime = 80,
                waveText = "Wave 4: The Horde"
            });
        }
        if (howmany > 4)
        {
            waves.Add(new WaveItem
            {
                basicEnemies = 20,
                shieldEnemies = 20,
                shootEnemies = 20,
                bossEnemies = 1,
                waveTime = 80,
                waveText = "Wave 5: Boss Time"
            });
        }
    }
}
