using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] private int health = 10;
    [SerializeField] private TextMeshProUGUI myTextElement;

    private void Update() 
    {
        ButtonPress();
    }

    public void ButtonPress()
    {
        myTextElement.text = "HEALTH : " + health;

        if(Input.GetKeyDown(KeyCode.Space))
        {
            health--;
        }
    }
}
