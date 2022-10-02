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
    [SerializeField] GameObject babyPrefab;
    [SerializeField] GameObject youngPrefab;
    [SerializeField] GameObject adultPrefab;
    [SerializeField] GameObject oldPrefab;
    [SerializeField] GameObject arrow;
    
    PlayerController playerController;
    GameObject weapon;
    float timer = 0.0f;
    

    // Start is called before the first frame update
    void Start()
    {
        age = new Age();
        prefab = GetCurrentPrefab();
        UpdatePlayerStatistics();
        playerController = FindObjectOfType(typeof(PlayerController)) as PlayerController;
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(currentHealth);
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

    private void UpdatePlayerStatistics()
    {
        if (weapon != null)
            weapon.SetActive(false);

        // Update maxHealth and currentHealth
        float ratio = currentHealth / maxHealth; // Preserve the ratio of health had before transition
        
        this.maxHealth = prefab.GetComponent<AgeStats>().maxHealth;
        this.currentHealth = maxHealth * ratio;
        
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
            weapon = transform.GetChild(0).GetChild(0).gameObject;
            weapon.SetActive(true);
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
        // Dodge
        int storeDefense = this.defense;
        this.defense = 100;
        // Call the dodge animation
        this.defense = storeDefense;
    }

    private void YoungAttack() {
        // Shoot Bow
        GameObject arrow = Instantiate(this.arrow);
        arrow.transform.position = transform.position;
        playerController.GetComponent<Animator>().SetTrigger("Attack");
        if (playerController.playerFacingRight)
            arrow.GetComponent<Rigidbody2D>().velocity = new Vector2(20, 0);
        else
        {
            arrow.GetComponent<SpriteRenderer>().flipX = true;
            arrow.GetComponent<Rigidbody2D>().velocity = new Vector2(-20, 0);
        }
    }

    private void AdultAttack() 
    {
        // Play attack animation        
        weapon.GetComponent<Animator>().SetTrigger("Attack");
    }

    private void OldAttack() {}
}
