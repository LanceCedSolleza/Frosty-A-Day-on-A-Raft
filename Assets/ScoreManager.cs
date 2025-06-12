using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public int currentScore = 0;
    public int highScore = 0;
    public int dayCounter;

    [Header("Background Animator")]
    public Animator backgroundAnimator;

    public string[] backgroundTriggers; // Array of animation trigger names
    private int lastBackgroundIndex = -1;


    public SpriteRenderer waterSprite;
    public float colorTransitionDuration = 2f;

    private Color targetColor;
    private Color initialColor;
    private bool isTransitioning = false;
    private float transitionProgress = 0f;

    public GameObject heart1;
    public GameObject heart2;
    public GameObject heart3;
    public int health = 3;

    public bool isBossActive = false;
    public float bossDuration = 30f;
    private bool hasBossSpawnedTonight = false;
    public GameObject bossSprite;

    public GameObject krakenPanel;
    public Image timeBar;
    public Image timeBarFilled;
    public TextMeshProUGUI secondsText;
    public TextMeshProUGUI bossLifeText;

    public int bossLife = 10;

    public GameObject fishSpawner;
    public GameObject dynamiteSpawner;
    public GameObject mineSpawner;

    public FishSpawner fishSpawnerClass;
    public DynamiteSpawner dynamiteSpawnerClass;
    public MineSpawner mineSpawnerClass;
    public PowerUpSpawner powerUpSpawnerClass;

    public float scoreMultiplierDuration = 15f; 
    public int scoreMultiplier = 1; 
    private Coroutine multiplierCoroutine;

    public GameObject floatingPoint;
    public Transform harpoonOrigin;

    public AudioManager audioManager;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Load high score
            highScore = PlayerPrefs.GetInt("HighScore", 0);
        }
        else
        {
            Destroy(gameObject);
        }

        if (waterSprite != null)
        {
            initialColor = waterSprite.color; // Initialize the starting color
            targetColor = initialColor;       // Set the first target color
        }

        UpdateBackground();

        bossLife = 10;
     }

    void Update()
    {
        if (isTransitioning && waterSprite != null)
        {
            transitionProgress += Time.deltaTime / colorTransitionDuration;
            waterSprite.color = Color.Lerp(initialColor, targetColor, transitionProgress);

            if (transitionProgress >= 1f)
            {
                isTransitioning = false;
                transitionProgress = 0f;
            }
        }

        if (health > 3) {
            health = 3;
        }
        if (health < 0) {
            health = 0;
        }

        switch (health) { 
            case 0:  
                heart1.gameObject.SetActive(false);
                heart2.gameObject.SetActive(false);
                heart3.gameObject.SetActive(false);
                break ;
            case 1:
                heart1.gameObject.SetActive(true);
                heart2.gameObject.SetActive(false);
                heart3.gameObject.SetActive(false);
                break ;
            case 2:
                heart1.gameObject.SetActive(true);
                heart2.gameObject.SetActive(true);
                heart3.gameObject.SetActive(false);
                break ;
            case 3:
                heart1.gameObject.SetActive(true);
                heart2.gameObject.SetActive(true);
                heart3.gameObject.SetActive(true);
                break ;
            default:
                heart1.gameObject.SetActive(true);
                heart2.gameObject.SetActive(true);
                heart3.gameObject.SetActive(true);
                break;


        }

        if (isBossActive) {
            Vector3 position = bossSprite.transform.position;
            position.y = Mathf.MoveTowards(position.y, 0.01f, 5f * Time.deltaTime);
            bossSprite.transform.position = position;

            bossLifeText.text = bossLife.ToString();
            
        }

        if (!isBossActive)
        {
            Vector3 position = bossSprite.transform.position;
            position.y = Mathf.MoveTowards(position.y, -7.62f, 3f * Time.deltaTime);
            bossSprite.transform.position = position;
            
        }

        if (isBossActive && bossLife == 0) {
            audioManager.KrakenWin();
            EndBossFight();
        }

    }
    public void AddScore(int amount)
    {
        
        int finalScore = amount * scoreMultiplier; // Apply the multiplier
        currentScore += finalScore;

        if (finalScore != 0)
        {
            GameObject pointUI = Instantiate(floatingPoint, new Vector3(harpoonOrigin.position.x + 1f, harpoonOrigin.position.y + 1f, harpoonOrigin.position.z), Quaternion.identity) as GameObject;
            pointUI.transform.GetComponentInChildren<TextMeshPro>().text = "" + finalScore;

            Destroy(pointUI, 1f);
        }


        // Update high score
        if (currentScore > highScore)
        {
            highScore = currentScore;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }

        UpdateBackground();
        
    }

    public void ApplyScoreMultiplier(int multiplierValue)
    {

        // Stop any active multiplier coroutine
        if (multiplierCoroutine != null)
        {
            StopCoroutine(multiplierCoroutine);
        }

        // Update the multiplier value
        scoreMultiplier = multiplierValue;
        xValue = multiplierValue;
        // Start the timer to reset the multiplier
        multiplierCoroutine = StartCoroutine(ResetMultiplierAfterDuration());
    }
    private int xValue;

    public TextMeshProUGUI multiplierTimerText;
    private IEnumerator ResetMultiplierAfterDuration()
    {
        float elapsedTime = 0f;

        while (elapsedTime < scoreMultiplierDuration)
        {

            if (xValue == 2)
            {
                multiplierTimerText.text = "x2: " + (scoreMultiplierDuration - elapsedTime).ToString("F1") + "s";
            }

            if (xValue == 4)
            {
                multiplierTimerText.text = "x4: " + (scoreMultiplierDuration - elapsedTime).ToString("F1") + "s";
            }

            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        scoreMultiplier = 1;
        multiplierTimerText.text = "";

        Debug.Log("Score multiplier reset to 1.");
    }

    private void TrySpawnBoss()
    {

        if (!isBossActive && !hasBossSpawnedTonight)
        {   
            hasBossSpawnedTonight = true; // Mark as attempted
            System.Random bossRandom = new System.Random();
            int bossChance = bossRandom.Next(0, 10);
            Debug.Log("BOSS CHANCE " + bossChance);
            if (bossChance <= 5f)
            {
                audioManager.KrakenSpawn();
                audioManager.PlayKrakenBG();
                StartBossFight();

                return;
            }
        }
    }


    private void StartBossFight()
    {
        isBossActive = true;
        hasBossSpawnedTonight = true;
        Debug.Log("Boss has appeared!");
        bossLife = 10;

        fishSpawnerClass.StopSpawningFish();
        dynamiteSpawnerClass.StopSpawningDynamite();
        mineSpawnerClass.StartSpawningMines();
        fishSpawnerClass.DestroyAllFishes();
        dynamiteSpawnerClass.DestroyAllDynamite();
        powerUpSpawnerClass.DestroyAllPowers();
        powerUpSpawnerClass.StopSpawningPower();

        krakenPanel.gameObject.SetActive(true);
        timeBar.gameObject.SetActive(true);
        timeBarFilled.gameObject.SetActive(true);

        StartCoroutine(BossTimer());
        
    
    }

    private void EndBossFight()
    {
        isBossActive = false;

        if (bossLife > 0)
        {
            audioManager.KrakenLose();
            audioManager.PlayNightBG();
        }

        UpdateBackground();
        Debug.Log("Boss has disappeared!");

        fishSpawnerClass.StartSpawningFish();
        dynamiteSpawnerClass.StartSpawningDynamite();
        mineSpawnerClass.StopSpawningMines();
        mineSpawnerClass.DestroyAllMines();
        powerUpSpawnerClass.StartSpawningPower();

        krakenPanel.gameObject.SetActive(false);
        timeBar.gameObject.SetActive(false);
        timeBarFilled.gameObject.SetActive(false);
    }

    private System.Collections.IEnumerator BossTimer()
    {
        float elapsedTime = 0f;

        // Initialize the time bar at full
        timeBarFilled.fillAmount = 1f;

        // Drain the time bar over the bossDuration
        while (elapsedTime < bossDuration)
        {
            elapsedTime += Time.deltaTime;
            float remainingTime = bossDuration - elapsedTime;
            secondsText.text = Mathf.CeilToInt(remainingTime).ToString() + "s";
            timeBarFilled.fillAmount = 1f - (elapsedTime / bossDuration); // Update the fill amount

            yield return null; // Wait for the next frame
        }

        // End the boss fight
        EndBossFight();
    }

    private void UpdateBackground()
    {
        Debug.Log("Has Boss Spawned or nah: " + hasBossSpawnedTonight);
        // Calculate the new background index based on score

        int calculatedIndex = (currentScore / 20);

        // Clamp to ensure it doesn't go below 0
        if (calculatedIndex < 0)
        {
            calculatedIndex = 0;
        }

        // Wrap around to cycle through backgrounds
        int backgroundIndex = calculatedIndex % backgroundTriggers.Length;
        Debug.Log("BackG Index: " + backgroundIndex);
        // Only update if there's a new background index
        if (backgroundIndex != lastBackgroundIndex)
        {
            if (lastBackgroundIndex != -1)
            {
                backgroundAnimator.ResetTrigger(backgroundTriggers[lastBackgroundIndex]);
            }


            if (backgroundTriggers[backgroundIndex] == "Day")
            {
                hasBossSpawnedTonight = false;
                audioManager.PlayMorningBG();
            }

            if (backgroundTriggers[backgroundIndex] == "Sunset")
            {
                
                audioManager.PlaySunsetBG();
            }

            if (backgroundTriggers[backgroundIndex] == "Night")
            {   
                audioManager.PlayNightBG();
            }


            ChangeWaterColor(backgroundIndex);


            audioManager.PlayNextScene();
            backgroundAnimator.SetTrigger(backgroundTriggers[backgroundIndex]);
            lastBackgroundIndex = backgroundIndex;

            Debug.Log(backgroundTriggers[backgroundIndex]);
            if (backgroundTriggers[backgroundIndex] == "Night")
            {
                TrySpawnBoss();
            }

            Debug.Log("Background changed to: " + backgroundTriggers[backgroundIndex]);
        }
    }

    private void ChangeWaterColor(int backgroundIndex)
    {
        // Set water color based on the current background trigger
        switch (backgroundTriggers[backgroundIndex])
        {
            case "Day":
                
                initialColor = waterSprite.color;
                targetColor = new Color(1f, 1f, 1f); // White
                isTransitioning = true;
                Debug.Log("Water Day");
                break;

            case "Sunset":
                
                initialColor = waterSprite.color;
                targetColor = new Color(1f, 0.5330188f, 0.8227551f); // Sunset color
                isTransitioning = true;
                Debug.Log("Water Sunset");
                break;

            case "Night":
                
                initialColor = waterSprite.color;
                targetColor = new Color(0.8613143f, 0.5395603f, 0.9150943f); // Night color
                isTransitioning = true;
                Debug.Log("Water Night");
                break;

            default:
                Debug.LogWarning("Unknown background trigger: " + backgroundTriggers[backgroundIndex]);
                break;
        }
    }

}
