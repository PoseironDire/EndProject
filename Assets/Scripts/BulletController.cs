using UnityEngine;

public class BulletController : MonoBehaviour
{
    [HideInInspector] public float initialVelocity = 50;
    [HideInInspector] public float initialSpread = 1;
    [HideInInspector] public float lifeTime = 1;

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
        GetComponent<BoxCollider2D>().enabled = false;
        Destroy(this.gameObject, lifeTime);
        if (transform.Find("Texture"))
            transform.Find("Texture").gameObject.SetActive(false);
        GameObject bullet = Instantiate(hitmarkPrefab, hitmarkSpawn.position, hitmarkSpawn.rotation);
    }
}
