using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Vector2 startingPos;

    void Start()
    {
        startingPos = transform.position;
    }

    void Update()
    {
        CheckPosition();
    }

    private void CheckPosition()
    {
        if (Mathf.Abs(transform.position.x - startingPos.x) > 50)
            Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<Damage>())
        {

        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        Destroy(this.gameObject);
    }
}
