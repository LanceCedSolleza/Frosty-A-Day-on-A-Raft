using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleScreenBehavior : MonoBehaviour
{
    public string sceneName;
    public GameObject infoPanel;
    public GameObject guidePanel;

    public Button tapToPlay;
    public Button guideButton;
    public Button infoButton;
    public Button exitButton;


    void Start()
    {
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeScene() {
        SceneManager.LoadScene(sceneName);
    }

    public void OpenInfo() { 
        infoPanel.SetActive(true);
        tapToPlay.gameObject.SetActive(false);
        tapToPlay.interactable = false;
        guideButton.gameObject.SetActive(false);
        infoButton.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);
    
    }

    public void CloseInfo()
    {
        infoPanel.SetActive(false);
        tapToPlay.gameObject.SetActive(true);
        tapToPlay.interactable = true;
        guideButton.gameObject.SetActive(true);
        infoButton.gameObject.SetActive(true);
        exitButton.gameObject.SetActive(true);
    }

    public void openGuide() {
        guidePanel.SetActive(true);
        tapToPlay.gameObject.SetActive(false);
        tapToPlay.interactable = false;
        guideButton.gameObject.SetActive(false);
        infoButton.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);
    }
    public void closeGuide() {
        guidePanel.SetActive(false);
        tapToPlay.gameObject.SetActive(true);
        tapToPlay.interactable = true;
        guideButton.gameObject.SetActive(true);
        infoButton.gameObject.SetActive(true);
        exitButton.gameObject.SetActive(true);

    }

    public void ExitApplication()
    {
        Debug.Log("Exiting application...");
        Application.Quit();

        // For the editor, simulate application quit
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

}
