using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
    private void Awake()
    {
        ScoreBoard score = FindObjectOfType(typeof(ScoreBoard)) as ScoreBoard;
        score.stopTracking = true;
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        gameObject.SetActive(false);
    }

    public void ReturnToTitle()
    {
        SceneManager.LoadScene("Title");
    }
}
