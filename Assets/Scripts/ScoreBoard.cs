using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class ScoreBoard : MonoBehaviour
{
    TextMeshProUGUI scoretext;
    TextMeshProUGUI timeText;
    int score = 0;
    float time = 0f;
    [SerializeField] GameObject scoreObject;
    [SerializeField] GameObject timeObject;

    public bool begin = false;
    
    public bool stopTracking;

    // Start is called before the first frame update
    void Awake()
    {
        stopTracking = false;

        if (SceneManager.GetActiveScene().name == "LevelOne")
        {
            PlayerPrefs.SetInt("score", 0);
            PlayerPrefs.SetInt("time", 0);
        }
        scoretext = scoreObject.GetComponent<TextMeshProUGUI>();
        timeText = timeObject.GetComponent<TextMeshProUGUI>();
        
        scoretext.text = score.ToString();
        timeText.text = ((int) time).ToString() + " s";
    }

    void Update()
    {
        if (score == 0 && !begin)
            return;

        scoretext.text = score.ToString();
        timeText.text = ((int) time).ToString() + " s";

        if (!stopTracking)
            time += Time.deltaTime;
    }

    public void AddToScore(int amt)
    {
        score += amt;
    }

    private void OnEnable()
    {
        score = PlayerPrefs.GetInt("score");
        time = PlayerPrefs.GetInt("time");
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt("score", score);
        PlayerPrefs.SetInt("time", (int) time);
    }
}
