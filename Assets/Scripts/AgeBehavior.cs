using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgeBehavior : MonoBehaviour
{
    public float currentHealth = 1f;
    public float maxHealth = 1f;
    public int defense;
    public float speed;
    public Age age;
    GameObject prefab;
    [SerializeField] HealthBar healthBar;
    [SerializeField] GameObject babyPrefab;
    [SerializeField] GameObject youngPrefab;
    [SerializeField] GameObject adultPrefab;
    [SerializeField] GameObject oldPrefab;
    [SerializeField] GameObject arrow;
    
    PlayerController playerController;
    GameObject weapon;
    float timer = 0.0f;
    float timeSinceLastAttack = 0.0f;
    
    void Awake()
    {
        age = new Age();
        prefab = GetCurrentPrefab();
        playerController = FindObjectOfType(typeof(PlayerController)) as PlayerController;
    }

    void Start()
    {
        healthBar.SetMaxHealth(maxHealth, true);
        UpdatePlayerStatistics();
    }

    // Update is called once per frame
    void Update()
    {
        // I don't like this and I'm sure you don't either, we'll both survive
        if (age.state == AgeEnum.Adult)
            if (timeSinceLastAttack > 0.4)
                weapon.GetComponent<BoxCollider2D>().enabled = false;
        timeSinceLastAttack += Time.deltaTime;

        if (currentHealth <= 0)
        {
            playerController.Death();
            Destroy(this);
        }

        ChangeState();
    }

    private void ChangeState()
    {
        timer += Time.deltaTime;
        if (timer > 10f)
        {
            timer = 0f;

            age.Next();

            prefab = GetCurrentPrefab();
            UpdatePlayerStatistics();
        }
    }

    private GameObject GetCurrentPrefab()
    {
        switch (age.state)
        {
            case AgeEnum.Baby:
                return babyPrefab;
            case AgeEnum.Young:
                return youngPrefab;
            case AgeEnum.Adult:
                return adultPrefab;
            case AgeEnum.Old:
                return oldPrefab;
            default:
                return null;
        }
    }

    public void TakeDamage(float amt)
    {
        currentHealth -= amt;
        healthBar.SetHealth(currentHealth);
    }

    private void UpdatePlayerStatistics()
    {
        // Make sure transform is correct size
        playerController.gameObject.transform.localScale = prefab.gameObject.transform.localScale;

        // Make sure collider is correct size
        playerController.GetComponent<CapsuleCollider2D>().size = prefab.gameObject.GetComponent<CapsuleCollider2D>().size;

        // Clean up previous attacks
        // FinishMapping();
        playerController.invincible = true;
        attacking = false;

        if (weapon != null)
            weapon.SetActive(false);

        // Update maxHealth and currentHealth
        float ratio = currentHealth / maxHealth; // Preserve the ratio of health had before transition
        
        this.maxHealth = prefab.GetComponent<AgeStats>().maxHealth;
        this.currentHealth = maxHealth * ratio;
        healthBar.SetMaxHealth(this.maxHealth);
        healthBar.SetHealth(this.currentHealth);
        
        // Update defense
        this.defense = prefab.GetComponent<AgeStats>().defense;

        // Update Speed
        this.speed = prefab.GetComponent<AgeStats>().speed;
        
        // Update animator
        GetComponent<Animator>().runtimeAnimatorController = prefab.GetComponent<Animator>().runtimeAnimatorController;

        // Update sprite
        GetComponent<SpriteRenderer>().sprite = prefab.GetComponent<Sprite>();

        // Add Weapon if they have one
        if (age.state == AgeEnum.Adult)
        {
            weapon = transform.GetChild(0).gameObject;
            weapon.SetActive(true);
            weapon.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    // Implement attacks with overloads?
    public void Attack()
    {
        switch (age.state)
        {
            case AgeEnum.Baby:
                BabyAttack();
                break;
            case AgeEnum.Young:
                YoungAttack();
                break;
            case AgeEnum.Adult:
                AdultAttack();
                break;
            case AgeEnum.Old:
                OldAttack();
                break;
        }
    }

    private void BabyAttack() {
        StartCoroutine(playerController.Dash());
    }

    bool attacking;

    private void YoungAttack() {
        if (timeSinceLastAttack < 0.2)
            return;
        else
            timeSinceLastAttack = 0.0f;

        // Shoot Bow
        Vector2 shotVelocity = new Vector2(20, 0);
        bool flip = false;
        
        if (!playerController.playerFacingRight)
        {
            shotVelocity *= -1f;
            flip = true;
        }

        GameObject arrow = Instantiate(this.arrow, new Vector2(transform.position.x + shotVelocity.x / 40f, transform.position.y), prefab.transform.rotation);
        arrow.transform.position = transform.position;
        playerController.GetComponent<Animator>().SetTrigger("Attack");

        arrow.GetComponent<Rigidbody2D>().velocity = shotVelocity;
        arrow.GetComponent<SpriteRenderer>().flipX = flip;
    }

    public void FinishYoungAttack()
    {
        attacking = false;
    }

    private void AdultAttack() 
    {
        
        if (timeSinceLastAttack < 0.5)
            return;
        else
            timeSinceLastAttack = 0.0f;

        // iFrames during attack
        playerController.invincible = true;

        // Play attack animation
        weapon.GetComponent<BoxCollider2D>().enabled = true;
        playerController.GetComponent<Animator>().SetTrigger("Attack");
    }

    public void FinishAdultAttack()
    {
        weapon.GetComponent<BoxCollider2D>().enabled = false;
        playerController.invincible = false;
    }

    private void OldAttack() {}


    public void FinishMapping()
    {
        switch (age.state)
        {
            case AgeEnum.Baby:
                break;
            case AgeEnum.Young:
                break;
            case AgeEnum.Adult:
                FinishAdultAttack();
                break;
            case AgeEnum.Old:
                break;
        }
    }
}
