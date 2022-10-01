using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rigidbody2D;
    SpriteRenderer sprite;
    Transform transform;
    Animator animator;
    [SerializeField] float jumpVelocity;
    // Start is called before the first frame update
    void Start()
    {
        Physics2D.queriesStartInColliders = false;
        rigidbody2D = GetComponent<Rigidbody2D>();
        transform = GetComponent<Transform>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!notFalling() && rigidbody2D.velocity.y < 0)
        {
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, rigidbody2D.velocity.y * 1.005f);
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            Move();
        if (Input.GetKey(KeyCode.W))
        {
            // animator.SetBool("Jumping", true);
            Jump();
        }
    }

    private void Move()
    {
        float dirX = Input.GetAxis("Horizontal");
        rigidbody2D.velocity = new Vector2(dirX * 7.0f, rigidbody2D.velocity.y);
    
        if (rigidbody2D.velocity.x < -.1f)
            sprite.flipX = true;
        else
            sprite.flipX = false;
    }

    private void Jump()
    {
        float dirY = Input.GetAxis("Vertical");

        if (notFalling())
        {
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
}
