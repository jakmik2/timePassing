using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    [SerializeField] float maxHealth;
    [SerializeField] int points;
    [SerializeField] string controllerName;
    ScoreBoard score;
    float currentHealth;
    Animator animator;
    bool scored = false;


    void Start()
    {
        score = FindObjectOfType(typeof(ScoreBoard)) as ScoreBoard;
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        CheckHealth();
    }

    private void CheckHealth()
    {
        if (currentHealth <= 0 && !scored)
        {
            scored = true;
            animator.SetTrigger("Dead");
            score.AddToScore(points);
        }
    }

    public void TakeDamage(float amt)
    {
        animator.SetTrigger("Hit");
        currentHealth -= amt;
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<Damageable>() != null)
            if (col.GetComponent<Damageable>().controllerName == this.controllerName)
                return;
                
        if (col.GetComponent<Damage>() != null)
        {
            TakeDamage(col.GetComponent<Damage>().points);
        }
    }
}
