using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rigidbody2D;
    SpriteRenderer sprite;
    Transform transform;
    Animator animator;
    AgeBehavior ageBehavior;
    [SerializeField] float jumpVelocity;
    [SerializeField] float velocityAugment;
    public bool canDash = true;
    private bool isDashing;
    private float dashingPower = 24f;
    private float dashingTime = 0.3f;
    private float dashingCooldown = 2f;
    
    [SerializeField] GameObject deathScreen;
    public float speed;
    public bool playerFacingRight;
    AgeStats currentAge;
    Vector2 startPos;
    public bool invincible = false;
    float timer = 0f;
    float jumpWait = 0f;
    bool disable = false;

    // Start is called before the first frame update
    void Start()
    {
        Physics2D.queriesStartInColliders = false;
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        transform = GetComponent<Transform>();
        sprite = GetComponent<SpriteRenderer>();
        speed = GetComponent<AgeStats>().speed;
        ageBehavior = GetComponent<AgeBehavior>();
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDashing)
            return;
        if (disable)
            return;
        speed = ageBehavior.speed;

        if (rigidbody2D.velocity.y < 0)
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, rigidbody2D.velocity.y * velocityAugment);

        InvincibleTimer();
        if (jumpWait > 0.1f)
            GetInput();
        else
            jumpWait += Time.deltaTime;
            
        UpdateAnimation();
    }

    Vector2 position;

    private void GetInput()
    {
        if (TouchingWall())
        {
            rigidbody2D.velocity = new Vector2(0f, 0f);
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            Move();
        if (Input.GetKey(KeyCode.W))
            Jump();
        if (Input.GetMouseButtonDown(0))
            Attack();
    }

    private void InvincibleTimer()
    {
        if (invincible)
        {
            timer += Time.deltaTime;
            if (timer > 1f)
                invincible = false;
        }
        else
            timer = 0f;
    }

    private void Move()
    {
        float dirX = Input.GetAxis("Horizontal");
        if (dirX > 0)
            playerFacingRight = true;
        else if (dirX < 0)
            playerFacingRight = false;
        
        rigidbody2D.velocity = new Vector2(dirX * speed, rigidbody2D.velocity.y);
    }

    private void UpdateAnimation()
    {
        // TODO: Known bug, when idling right, it quickly idles left first
        animator.SetFloat("posX", rigidbody2D.velocity.x);
        animator.SetFloat("posY", rigidbody2D.velocity.y);

        if (Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1)
        {
            animator.SetFloat("lastDirection", Input.GetAxisRaw("Horizontal"));
            // Debug.Log(Input.GetAxisRaw("Horizontal"));
        }
    }

    float wall;

    private void Jump()
    {
        float dirY = Input.GetAxis("Vertical");


        if (TouchingWall())
        {
            jumpWait = 0f;
            rigidbody2D.velocity = new Vector2(-wall * 15f, 15f);
        }
        else if (NotFalling())
        {
            rigidbody2D.velocity = new Vector2(0, 15f);
        }

    }

    public IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        invincible = true;
        float originalGravity = rigidbody2D.gravityScale;
        rigidbody2D.gravityScale = 0f;
        rigidbody2D.velocity = new Vector2(Mathf.Sign(rigidbody2D.velocity.x) * dashingPower, 0f);
        yield return new WaitForSeconds(dashingTime);
        rigidbody2D.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
        invincible = false;
    }

    // Shoot a raycast down, if it hits a collider that is grater than a certain distance away, then the object is falling.
    private bool NotFalling()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up);

        if (hit.collider != null)
        {
            float height = Mathf.Abs(hit.point.y - transform.position.y);
            return height < 0.41f;
        }
        else
            return false;
    }

    private bool TouchingWall()
    {
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right);
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left);

        if (hitRight.collider != null)
            if (hitRight.collider.name == "Tilemap")
            {
                float distance = Mathf.Abs(hitRight.point.x - transform.position.x);
                wall = 1;
                return distance < 0.41f;
            }
        
        if (hitLeft.collider != null)
            if (hitLeft.collider.name == "Tilemap")
            {
                float distance = Mathf.Abs(hitLeft.point.x - transform.position.x);
                wall = -1;
                return distance < 0.41f;
            }
        return false;
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name == "Death")
        {
            Death();
        }
        else if (col.gameObject.GetComponent<Damage>() != null && !invincible)
        {
            // Handling Your own Arrow shots, this code is yikes
            if (col.gameObject.GetComponent<Damage>().playerAttack)
                return;
            
            // Otherwise inflict damage
            Damage damage = col.gameObject.GetComponent<Damage>();
            ageBehavior.TakeDamage(damage.points);
            rigidbody2D.velocity = new Vector2(-6f * Mathf.Sign(rigidbody2D.velocity.x), 8f);
            
            GetComponent<Animator>().SetTrigger("Damage");
            invincible = true;
        }
    }

    public void Death()
    {
        transform.position = startPos;
        // Destroy(this.gameObject);
        deathScreen.SetActive(true);
        disable = true;
    }

    private void Attack()
    {
        ageBehavior.Attack();
    }
}
