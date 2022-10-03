using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    [SerializeField] float maxHealth;
    [SerializeField] int points;
    Score score;
    float currentHealth;
    Animator animator;


    void Start()
    {
        score = FindObjectOfType(typeof(Score)) as Score;
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        CheckHealth();
    }

    private void CheckHealth()
    {
        if (currentHealth <= 0)
        {
            animator.SetTrigger("Dead");
        }
    }

    public void OnDestroy()
    {
        score.AddToScore(points);
    }

    public void TakeDamage(float amt)
    {
        animator.SetTrigger("Hit");
        currentHealth -= amt;
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<Damage>() != null)
        {
            TakeDamage(col.GetComponent<Damage>().points);
        }
    }
}
