using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField][Range(10, 1000)] float initialVelocity = 6;
    [SerializeField][Range(0, 5)] float initialSpread = 0;

    [SerializeField] float lifeTime = 2;

    Rigidbody2D rb2D;

    void Awake()
    {
        GetComponent<AudioSource>().pitch = GetComponent<AudioSource>().pitch * UnityEngine.Random.Range(0.9f, 1.1f);
    }

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();

        initialSpread *= UnityEngine.Random.Range(10, -10);
        rb2D.velocity = transform.up * initialVelocity + transform.right * initialSpread;
        Destroy(this.gameObject, lifeTime);
    }

    void FixedUpdate()
    {
        var dir = rb2D.velocity;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        rb2D.MoveRotation(angle - 90);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
            // Destroy(this.gameObject);
        }
    }
}
