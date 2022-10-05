using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float speed;
    Rigidbody2D rb;
    Animator animator;

    GameObject leftCollider;
    Vector2 leftInitialPos;
    GameObject rightCollider;
    Vector2 rightInitialPos;
    float timer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Initialize Colliders
        leftCollider = transform.GetChild(0).GetChild(0).gameObject;
        leftInitialPos = leftCollider.transform.position;
        
        rightCollider = transform.GetChild(0).GetChild(1).gameObject;
        rightInitialPos = rightCollider.transform.position;
        
        // Start Right
        rb.velocity = new Vector2(speed, 0f);

        // Timer to enable direction swap
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        leftCollider.transform.position = leftInitialPos;
        rightCollider.transform.position = rightInitialPos;

        animator.SetFloat("posX", rb.velocity.x);

        timer += Time.deltaTime;
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if ((col.name == "Left" || col.name == "Right") && timer > 0.1f)
        {
            timer = 0f;
            rb.velocity = new Vector2(-rb.velocity.x, 0.0f);
        }
    }
}
