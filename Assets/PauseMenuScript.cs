using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseMenuScript : MonoBehaviour
{
    public GameObject pauseMenuUI; // Reference to the pause menu UI panel
    public GameObject confirmDialogUI; // Reference to the confirmation dialog UI panel

    public GameObject buttonDock;
    public GameObject pauseButton;
    public Button mainButton;

    public GameObject checkButton;
    public GameObject xButton;

    public GameObject warning;

    private bool isPaused = false;


    void Start()
    {
        confirmDialogUI.SetActive(false);
        xButton.SetActive(false);
        checkButton.SetActive(false);
        pauseMenuUI.SetActive(false);
        warning.SetActive(false);
        showConfirm = false;
    }
    void Update()
    {
        // Check for the Pause key (Escape)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        if (showConfirm) {
            Vector3 confirmBoxPos = confirmDialogUI.transform.position;
            confirmBoxPos.y = Mathf.MoveTowards(confirmBoxPos.y, -2.5f, 10f * Time.unscaledDeltaTime);

            Vector3 checkPos = checkButton.transform.position;
            checkPos.y = Mathf.MoveTowards(checkPos.y, 161f, 850f * Time.unscaledDeltaTime);

            Vector3 xPos = xButton.transform.position;
            xPos.y = Mathf.MoveTowards(xPos.y, 161f, 850f * Time.unscaledDeltaTime);

            confirmDialogUI.transform.position = confirmBoxPos;
            checkButton.transform.position = checkPos;
            xButton.transform.position = xPos;

            confirmDialogUI.SetActive(true);

            if (checkButton.transform.position.y < 162f) {
                xButton.SetActive(true);
                checkButton.SetActive(true);
                warning.SetActive(true);
            }
        }

        if (!showConfirm) {
  
            Vector3 bconfirmBoxPos = confirmDialogUI.transform.position;
            bconfirmBoxPos.y = Mathf.MoveTowards(bconfirmBoxPos.y, 0.2f, 10f * Time.unscaledDeltaTime);
            confirmDialogUI.transform.position = bconfirmBoxPos;

            Vector3 bcheckPos = checkButton.transform.position;
            bcheckPos.y = Mathf.MoveTowards(bcheckPos.y, 420f, 850f * Time.unscaledDeltaTime);

            Vector3 bxPos = xButton.transform.position;
            bxPos.y = Mathf.MoveTowards(bxPos.y, 420f, 850f * Time.unscaledDeltaTime);

            checkButton.transform.position = bcheckPos;
            xButton.transform.position = bxPos;


                checkButton.SetActive(false);
                xButton.SetActive(false);
                warning.SetActive(false);


            if (confirmDialogUI.transform.position.y > 0.19f)
            {
                confirmDialogUI.SetActive(false);
            }

        }

    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        buttonDock.SetActive(true);
        pauseButton.SetActive(true);
        mainButton.gameObject.SetActive(true);
        mainButton.interactable = true;
        mainButton.enabled = true;
        Time.timeScale = 1f; // Resume game time
        isPaused = false;
        showConfirm = false;

    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        buttonDock.SetActive(false);
        pauseButton.SetActive(false);
        mainButton.gameObject.SetActive(false);
        mainButton.interactable = false;
        mainButton.enabled = false;
        //Time.timeScale = 0f; // Pause game time
        isPaused = true;
    }

    public void OpenSettings()
    {
        // Handle opening settings (e.g., display settings UI)
        Debug.Log("Settings opened");
        showConfirm = false;
    }

    private bool showConfirm = false;
    public void ShowMainMenuConfirmation()
    {
        showConfirm = true;
    }

    public void ConfirmMainMenu()
    {
        // Return to the main menu
        Time.timeScale = 1f; // Ensure time is running before changing scenes
        if (ScoreManager.Instance != null)
        {
            Destroy(ScoreManager.Instance.gameObject);
        }
        SceneManager.LoadScene("TitleScreen");
        showConfirm = false;
    }

    public void CancelMainMenu()
    {
        // Close the confirmation dialog
        showConfirm = false;
    }

}
