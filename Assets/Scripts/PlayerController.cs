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
    public float speed;
    public bool playerFacingRight;
    AgeStats currentAge;
    Vector2 startPos;

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
        speed = ageBehavior.speed;
        if (rigidbody2D.velocity.x > 0 )
            animator.SetFloat("lastDirection", 1);
        if (rigidbody2D.velocity.x < 0)
            animator.SetFloat("lastDirection", 0);
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            Move();
        if (Input.GetKey(KeyCode.W))
            Jump();
        if (Input.GetMouseButtonDown(0))
            Attack();
        UpdateAnimation();
    }

    private void Move()
    {
        float dirX = Input.GetAxis("Horizontal");
        if (dirX >= 0)
            playerFacingRight = true;
        else
            playerFacingRight = false;
        rigidbody2D.velocity = new Vector2(dirX * speed, rigidbody2D.velocity.y);
    }

    private void UpdateAnimation()
    {
        if (rigidbody2D.velocity.y < 0)
        {
            animator.SetTrigger("Falling");
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, rigidbody2D.velocity.y * 1.005f);
        }
        else
        {
            animator.SetBool("OnGround", true);
        }
        animator.SetFloat("posX", rigidbody2D.velocity.x);
    }

    private void Jump()
    {
        float dirY = Input.GetAxis("Vertical");

        if (notFalling())
        {
            animator.SetBool("OnGround", false);
            animator.SetTrigger("Jumping");

            rigidbody2D.velocity = new Vector2(0, Mathf.Min((jumpVelocity * dirY) + 2f, 10f)); // Short hop and tall hop
        }
    }

    // Shoot a raycast down, if it hits a collider that is grater than a certain distance away, then the object is falling.
    private bool notFalling()
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

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name == "Death")
        {
            transform.position = startPos;
        }
    }

    private void Attack()
    {
        ageBehavior.Attack();
    }
}
