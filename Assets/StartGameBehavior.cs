using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartGameBehavior : MonoBehaviour
{
    public GameObject waterSprite;
    public float waterMoveSpeed = 1f; 
    public float targetWaterY;

    public GameObject raftSprite;
    public float raftMoveSpeed = 1f;
    public float targetRaftX;

    public Button mainButton;
    public Button pauseButton;

    private bool waterMove = false;
    private bool raftMove = false;

    public bool hideGui;

    public GameObject cloudLeft;
    public GameObject cloudRight;

    public GameObject buttonDock;

    public GameObject fishSpawner;
    public GameObject dynamiteSpawner;
    public GameObject powerSpawner;
    public GameObject pauseMenuScript;

    public GameObject gameOverPanel;

    public AudioManager audioManager;
   

    void Start()
    {
        waterMove = true;
        raftMove = true;
        mainButton.gameObject.SetActive(false);
        mainButton.interactable = true;
        pauseButton.gameObject.SetActive(false);
        fishSpawner.SetActive(false);
        powerSpawner.SetActive(false);
        dynamiteSpawner.SetActive(false);
        pauseMenuScript.SetActive(false);
        gameOverPanel.SetActive(false);
        ScoreManager.Instance.health = 3;
        Time.timeScale = 1f;
        buttonDock.SetActive(true);

        ScoreManager.Instance.health = 3;
    }

    private void Update()
    {

        if (ScoreManager.Instance != null)
        {
            if (ScoreManager.Instance.health == 0)
            {
                Time.timeScale = 0f;
                gameOverPanel.SetActive(true);
                buttonDock.SetActive(false);
                mainButton.gameObject.SetActive(false);
                mainButton.interactable = false;
                pauseButton.gameObject.SetActive(false);
            }
        }


        if (hideGui) {
            Color buttonOp = mainButton.GetComponent<Image>().color;
            buttonOp.a = 0f;
            mainButton.GetComponent<Image>().color = buttonOp;
        }

        if (!hideGui)
        {
            Color buttonOp = mainButton.GetComponent<Image>().color;
            buttonOp.a = 1f;
            mainButton.GetComponent<Image>().color = buttonOp;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ScoreManager.Instance.AddScore(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ScoreManager.Instance.AddScore(5);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ScoreManager.Instance.AddScore(5);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            ScoreManager.Instance.highScore = 0;
        }



        if (waterMove && waterSprite != null)
        {
            Vector3 position = waterSprite.transform.position;
            position.y = Mathf.MoveTowards(position.y, targetWaterY, waterMoveSpeed * Time.deltaTime);
            waterSprite.transform.position = position;

            if (Mathf.Approximately(position.y, targetWaterY))
            {   
                fishSpawner.gameObject.SetActive(true);
                dynamiteSpawner.SetActive(true);
                waterMove = false;
                powerSpawner.SetActive(true);

            }
        }

        if (raftMove) {

            Vector3 raftPosition = raftSprite.transform.position;
            raftPosition.x = Mathf.MoveTowards(raftPosition.x, targetRaftX, raftMoveSpeed * Time.deltaTime);
            raftSprite.transform.position = raftPosition;

            Vector3 cloudLeftPosition = cloudLeft.transform.position;
            cloudLeftPosition.x = Mathf.MoveTowards(cloudLeftPosition.x, -9.22f, 4f * Time.deltaTime);
            cloudLeft.transform.position = cloudLeftPosition;

            Vector3 cloudRightPosition = cloudRight.transform.position;
            cloudRightPosition.x = Mathf.MoveTowards(cloudRightPosition.x, 9.07f, 4f * Time.deltaTime);
            cloudRight.transform.position = cloudRightPosition;

            Vector3 dockPosition = buttonDock.transform.position;
            dockPosition.y = Mathf.MoveTowards(dockPosition.y, -4.43f, 4f * Time.deltaTime);
            buttonDock.transform.position = dockPosition;

            if (Mathf.Approximately(raftPosition.x, targetRaftX))
            {
                raftMove = false;
                mainButton.gameObject.SetActive(true);
                pauseButton.gameObject.SetActive(true);
                pauseMenuScript.gameObject.SetActive(true);
            }

        }

        

    }

    public void YesPlayAgain() {
        if (ScoreManager.Instance != null)
        {
            Destroy(ScoreManager.Instance.gameObject);
        }
        Time.timeScale = 1f;
        audioManager.PowerUp();
        SceneManager.LoadScene("GameStart");
    }

    public void NoIdontWannaPlayAgain() {
        if (ScoreManager.Instance != null)
        {   
            audioManager.FrostyDie();
            Destroy(ScoreManager.Instance.gameObject);
        }
        Time.timeScale = 1f;
        SceneManager.LoadScene("TitleScreen");
    }

}
