using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{

    [SerializeField]
    float initialVelocity = 6;

    [SerializeField]
    float lifeTime = 2;

    void Start()
    {
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.velocity = transform.up * initialVelocity;

        Destroy(this.gameObject, lifeTime);
    }
}
