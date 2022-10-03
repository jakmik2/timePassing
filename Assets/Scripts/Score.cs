using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    TextMeshProUGUI textMesh;
    int score;
    
    // Start is called before the first frame update
    void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        score = 0;
    }

    void Update()
    {
        textMesh.text = score.ToString();
    }

    public void AddToScore(int amt)
    {
        score += amt;
    }
}
