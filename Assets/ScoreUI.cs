using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    public TextMeshProUGUI currentScoreText;
    public TextMeshProUGUI highScoreText;

    void Update()
    {
        currentScoreText.text = "Score: " + ScoreManager.Instance.currentScore;
        highScoreText.text = "High Score: " + ScoreManager.Instance.highScore;
    }
}
