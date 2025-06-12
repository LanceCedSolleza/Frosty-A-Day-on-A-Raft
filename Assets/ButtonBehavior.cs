using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ButtonBehavior : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Button mainButton;
    //harpoon
    public GameObject arrowSprite;
    public GameObject harpoonSprite;

    public Transform arrow;            // Reference to the arrow
    public Transform harpoon;          // Reference to the harpoon
    public Transform harpoonOrigin;    // Starting point of the harpoon (below the raft)
    public Transform harpoonPivot;    // Starting point of the harpoon (below the raft)
    public float swingSpeed;     // Speed of the arrow's swing
    public float swingAngle;     // Maximum angle of the swing
    public float harpoonSpeed;   // Speed of the harpoon
    public float harpoonPullSpeed;

    private bool isPressing = false;   // Track if the button is being pressed
    private bool isHarpoonLaunched = false; // Track if the harpoon is launched
    private float currentAngle = 0f;   // Current angle of the arrow

    private LineRenderer ropeRenderer;  // Reference to the LineRenderer
    public Transform ropeStartPoint;

    private HarpoonCollisionHandler harpoonCollisionHandler;
    private AnimationBehavior animationBehavior;

    private GameObject hookedFish = null;

    public Image progressBar;          // UI element for the progress bar
    public Image progressBarFilled;          // UI element for the progress bar
    public GameObject smashSprite;
    public float drainSpeed = 0.5f;    // Speed at which the progress bar drains
    public float fillAmountPerClick = 0.1f; // Amount filled per button press
    private bool isSecured = false;
    private bool isStruggling = false;
    public float currentProgress;

    public float buttonCooldown = 3f; // Cooldown duration in seconds
    public float cooldownTimer = 0f;   // Timer to track cooldown
    private bool canPressButton = true;

    public float struggleChance;
    public float struggleRandom;

    public AudioManager audioManager;

    public int score = 0;
    void Start()
    {
        arrowSprite.gameObject.SetActive(false);
        harpoonSprite.gameObject.SetActive(false);

        ropeRenderer = harpoonSprite.GetComponent<LineRenderer>();
        ropeRenderer.positionCount = 2; // Two points: Start and End

        isPullingBack = false;

        harpoonCollisionHandler = harpoon.GetComponent<HarpoonCollisionHandler>();
        animationBehavior = FindObjectOfType<AnimationBehavior>();

        progressBar.gameObject.SetActive(false);
        progressBarFilled.gameObject.SetActive(false);
        smashSprite.gameObject.SetActive(false);
    }

    // Update is called once per frame

    private float distanceTraveled = 0f;
    private bool isPullingBack;
    void Update()
    {

        if (!canPressButton)
        {
            cooldownTimer -= Time.deltaTime;
            mainButton.interactable = false;
            if (cooldownTimer <= 0f)
            {
                canPressButton = true; // Allow button press again
                mainButton.interactable = true;
            }
        }

        if (isPressing && !isStruggling)
        {
            // Swing the arrow while the button is held down
            currentAngle = Mathf.Sin(Time.time * swingSpeed) * swingAngle;
            arrow.localRotation = Quaternion.Euler(0, 0, -currentAngle); // Negative to swing downward
            arrowSprite.gameObject.SetActive(true);


        }
        else if (isHarpoonLaunched)
        {

            if (harpoonCollisionHandler.HasHookedFish())
            {

                hookedFish = harpoonCollisionHandler.hookedFish;

                if (hookedFish.name == "DYNAMITE(Clone)") {
                    PullFish();
                    return;
                }

                if (hookedFish.name == "ONEUP(Clone)")
                {
                    PullFish();
                    return;
                }
                if (hookedFish.name == "X2(Clone)")
                {
                    PullFish();
                    return;
                }
                if (hookedFish.name == "X4(Clone)")
                {
                    PullFish();
                    return;
                }

                if (hookedFish.name == "MINE(Clone)")
                {
                    HandleHarpoonPullBack();
                    GameObject explosionInstance = Instantiate(explosion, new Vector3(hookedFish.transform.position.x, hookedFish.transform.position.y, hookedFish.transform.position.z), Quaternion.identity);
                    Destroy(explosionInstance, 2f);
                    Destroy(hookedFish);
                    hookedFish = null;

                    ScoreManager.Instance.bossLife -= 1;
                    if (ScoreManager.Instance.bossLife == 0)
                    {
                        ScoreManager.Instance.AddScore(20);
                        animationBehavior.PlayKrakenDie();
                    }
                    if (ScoreManager.Instance.bossLife > 0)
                    {
                        animationBehavior.PlayKrakenHurt();
                        audioManager.FrostyDie();
                    }


                    return;
                }


                if (!isStruggling && !isSecured)
                {   
                    System.Random randomizer = new System.Random();
                    struggleRandom = randomizer.Next(0, 10);

                    if (struggleRandom < struggleChance)
                    {
                        StartStruggle();
                        return;
                    }
                    else { 
                        isStruggling = false;
                        isSecured = true;
                        PullFish();
                    }
                }

                if (isStruggling)
                {
                    animationBehavior.FrostyStruggle();
                    HandleStruggle();
                }

                if (!isStruggling && isSecured) {
                    PullFish();
                }

            }
            if (harpoonCollisionHandler.HasHookedFish() == false)
            {
                HandleHarpoonMovement();
            }
        }

        if (!isHarpoonLaunched && isPullingBack) {
           HandleHarpoonPullBack();
        }

    }

    private void StartStruggle()
    {
        audioManager.PlayStruggle();
        currentProgress = 0.5f;
        isStruggling = true;
        isSecured = false;
        progressBarFilled.fillAmount = currentProgress;
        progressBar.gameObject.SetActive(true);
        progressBarFilled.gameObject.SetActive(true);
        smashSprite.gameObject.SetActive(true);
    }

    private void HandleStruggle()
    {   
        mainButton.interactable = true;
        hookedFish.transform.position = harpoonPivot.position;
        currentProgress -= drainSpeed * Time.deltaTime;
        progressBarFilled.fillAmount = currentProgress;
        smashSprite.gameObject.SetActive(true);


        if (currentProgress <= 0f)
        {
            LoseFish();
            canPressButton = false;
            cooldownTimer = buttonCooldown;
        }

        if (currentProgress >= 0.95f) {
            progressBar.gameObject.SetActive(false);
            progressBarFilled.gameObject.SetActive(false);
            smashSprite.gameObject.SetActive(false);
            isStruggling = false;
            isSecured = true;
            PullFish();
            canPressButton = false;
            cooldownTimer = buttonCooldown;
        }
    }

    private void PullFish() {

        Vector3 pullDirection = (harpoonOrigin.position - harpoonPivot.position).normalized;
        harpoonPivot.position += pullDirection * harpoonPullSpeed * Time.deltaTime;

        hookedFish.transform.position = harpoonPivot.position;

        // Update rope position
        ropeRenderer.SetPosition(0, ropeStartPoint.position); // Rope start (origin)
        ropeRenderer.SetPosition(1, harpoonPivot.position);   // Rope end (harpoon)


        if (harpoonCollisionHandler.HasHookedFish() && Vector2.Distance(hookedFish.transform.position, harpoonOrigin.position) < 0.1f)
        {
            Debug.Log("lezgo");
            isStruggling = false;
            isSecured = false;
            ScoreFish(); // Add score and remove fish
            ResetHarpoon(); // Reset the harpoon for the next use
        }
    }

    private void LoseFish()
    {
        isStruggling = false;
        progressBar.gameObject.SetActive(false);
        progressBarFilled.gameObject.SetActive(false);
        smashSprite.gameObject.SetActive(false);

        Destroy(hookedFish);
        hookedFish = null;

        ScoreManager.Instance.health -= 1;
        GameObject pointUI = Instantiate(floatingPoint, new Vector3(harpoonOrigin.position.x + 1.5f, harpoonOrigin.position.y + 1f, harpoonOrigin.position.z), Quaternion.identity) as GameObject;
        pointUI.transform.GetComponentInChildren<TextMeshPro>().text = "-1 Life";
        Destroy(pointUI, 1f);

        ResetHarpoon() ;
    }

    public GameObject floatingPoint;
    public GameObject explosion;
    private void ScoreFish()
    {
        if (hookedFish != null)
        {
            int points; // Define the points for catching a fish

            switch (hookedFish.name) {
                case "FISH 10(Clone)": points = 2; animationBehavior.PlayScoreGainedAnimation();
                    audioManager.CaughtFish();
                    
                    break;
                case "DYNAMITE(Clone)": points = 0;
                                        animationBehavior.FrostyDied();
                                        GameObject explosionInstance = Instantiate(explosion, new Vector3(harpoonOrigin.position.x, harpoonOrigin.position.y, harpoonOrigin.position.z), Quaternion.identity); 
                                        Destroy(explosionInstance, 2f);
                                        GameObject pointUI = Instantiate(floatingPoint, new Vector3(harpoonOrigin.position.x + 1.5f, harpoonOrigin.position.y + 1f, harpoonOrigin.position.z), Quaternion.identity) as GameObject;
                                        pointUI.transform.GetComponentInChildren<TextMeshPro>().text = "-1 Life";
                                        Destroy(pointUI, 1f);
                                        ScoreManager.Instance.health -=1 ;
                                        audioManager.FrostyDie();                
                    break;

                case "ONEUP(Clone)": points = 0;
                                     animationBehavior.PlayScoreGainedAnimation();
                                     GameObject lifeUI = Instantiate(floatingPoint, new Vector3(harpoonOrigin.position.x + 1.5f, harpoonOrigin.position.y + 1f, harpoonOrigin.position.z), Quaternion.identity) as GameObject;
                                     lifeUI.transform.GetComponentInChildren<TextMeshPro>().text = "+Life";
                    ScoreManager.Instance.health += 1;
                    if (ScoreManager.Instance.health > 3)
                    {
                        ScoreManager.Instance.health = 3;
                    }

                    Destroy(lifeUI, 1f);
                    audioManager.PowerUp(); 
                    break;
                case "X2(Clone)":   points = 0;
                                    animationBehavior.PlayScoreGainedAnimation(); audioManager.PowerUp();
                    break;
                case "X4(Clone)": points= 0;
                                  animationBehavior.PlayScoreGainedAnimation(); audioManager.PowerUp();
                    break;

                default: points = 1 ; animationBehavior.PlayScoreGainedAnimation(); audioManager.CaughtFish(); break;
            
            }

            // Add score using the ScoreManager
            
            ScoreManager.Instance.AddScore(points);


            Destroy(hookedFish);
            hookedFish = null;
        }
    }

    private void HandleHarpoonMovement()
    {
        distanceTraveled = Vector2.Distance(harpoonPivot.position, harpoonOrigin.position);

        // Move the harpoon downward based on the arrow's angle
        harpoonPivot.position += harpoonPivot.up * -harpoonSpeed * Time.deltaTime; // Negative for downward movement
        arrowSprite.gameObject.SetActive(false);
        // Optionally, stop the harpoon after it moves far enough

        ropeRenderer.SetPosition(0, ropeStartPoint.position); // Rope start (origin)
        ropeRenderer.SetPosition(1, harpoonPivot.position);   // Rope end (harpoon)

        if (distanceTraveled > 7f)
        {
            isHarpoonLaunched = false;
            isPullingBack = true;
        }
    }

    private void HandleHarpoonPullBack()
    {
        harpoon.GetComponent<Collider2D>().enabled = false;

        distanceTraveled = Vector2.Distance(harpoonPivot.position, harpoonOrigin.position);

        if (distanceTraveled < 0.05f)
        {
            harpoon.gameObject.SetActive(false);
            ResetHarpoon();
        }
        //active kase!
        Vector3 pullDirection = (harpoonOrigin.position - harpoonPivot.position).normalized;
        harpoonPivot.position += pullDirection * harpoonPullSpeed * Time.deltaTime;

        ropeRenderer.SetPosition(0, ropeStartPoint.position); // Rope start (origin)
        ropeRenderer.SetPosition(1, harpoonPivot.position);   // Rope end (harpoon)
    }


    private bool isHarpoonReady;
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!mainButton.interactable || !canPressButton)
        {
            isHarpoonReady = false;
            return;
        }

        if (isStruggling)
        {
            isPressing = false;
            mainButton.interactable = true;
            currentProgress += fillAmountPerClick;
            currentProgress = Mathf.Clamp(currentProgress, 0f, 1f);
            progressBarFilled.fillAmount = currentProgress;
        }

        if (!isStruggling && mainButton.interactable) {
            isPressing = true;
            isHarpoonLaunched = false;
            isHarpoonReady = true;
        }

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isHarpoonReady)
        {
            return;
        }

        if (isStruggling)
        {
            mainButton.interactable = true;
        }

        if (!isStruggling && canPressButton) {
            isPressing = false;
            isHarpoonLaunched = true;

            mainButton.interactable = false;

            // Set harpoon position and rotation
            harpoonPivot.rotation = arrow.rotation;

            harpoonPivot.position = harpoonOrigin.position;
            harpoon.gameObject.SetActive(true);

            harpoon.GetComponent<Collider2D>().enabled = true;
        }
    }
    private void ResetHarpoon()
    {
        // Reset the harpoon after it finishes moving
        harpoon.gameObject.SetActive(false);
        isHarpoonLaunched = false;

        ropeRenderer.SetPosition(0, Vector3.zero);
        ropeRenderer.SetPosition(1, Vector3.zero);

        mainButton.interactable = true;

        mainButton.enabled = true;
        isPullingBack = false;
        harpoon.GetComponent<Collider2D>().enabled = true;
        progressBar.gameObject.SetActive(false);
        progressBarFilled.gameObject.SetActive(false);
        smashSprite.gameObject.SetActive(false);

        isStruggling = false;
        isSecured = false;

        Debug.Log("reset");

    }


}
