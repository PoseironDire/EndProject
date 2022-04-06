using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField][Range(10, 1000)] float initialVelocity = 6;
    [SerializeField][Range(0, 5)] float initialSpread = 0;

    [SerializeField] float lifeTime = 2;

    Rigidbody2D rb2D;

    [SerializeField] GameObject hitmarkPrefab;
    public Transform hitmarkSpawn;

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
        if (rb2D.bodyType != RigidbodyType2D.Static)
            rb2D.MoveRotation(angle - 90);

        GetComponent<AudioSource>().pitch = UnityEngine.Random.Range(1, 3);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject bullet = Instantiate(hitmarkPrefab, hitmarkSpawn.position, hitmarkSpawn.rotation);
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        rb2D.bodyType = RigidbodyType2D.Static;
        Destroy(this.gameObject, lifeTime);
        if (transform.childCount > 1)
            Destroy(transform.GetChild(1).gameObject);
    }
}
