using UnityEngine;

public class Projectile : MonoBehaviour
{
    //Values
    float initialVelocity;
    float initialSpread;
    float lifeTime;
    float shotsPerSecond;
    float volume;
    float maxPitch;
    float minPitch;

    GameObject player; //Player
    Rigidbody2D rb2D; //Rigidbody

    [SerializeField] GameObject hitEffectPrefab; //Hit Effect
    [SerializeField] Transform hitEffectSpawn; //Hit Effect Spawn Point

    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>(); //Get Rigidbody
        player = FindObjectOfType<PlayerController>().gameObject; //Get Player

        //Get Values From Player (Temporary Feature)
        initialVelocity = player.GetComponent<PlayerController>().initialProjectileVelocity;
        initialSpread = player.GetComponent<PlayerController>().initialProjectileSpread;
        lifeTime = player.GetComponent<PlayerController>().projectileLifeTime;
        shotsPerSecond = player.GetComponent<PlayerController>().firesPerSecond;
        volume = player.GetComponent<PlayerController>().projectileVolume;
        maxPitch = player.GetComponent<PlayerController>().maxPitch;
        minPitch = player.GetComponent<PlayerController>().minPitch;

        //Get Random Spread
        initialSpread *= UnityEngine.Random.Range(10, -10);
        rb2D.velocity = transform.up * initialVelocity + transform.right * initialSpread;

        //Config Sound
        GetComponent<AudioSource>().volume = volume;
        GetComponent<AudioSource>().pitch = UnityEngine.Random.Range(minPitch, maxPitch);

        Destroy(this.gameObject, lifeTime); //Destroy
    }

    void FixedUpdate()
    {
        //Rotate Twoards Velocity
        var dir = rb2D.velocity;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (rb2D.bodyType != RigidbodyType2D.Static) rb2D.MoveRotation(angle - 90);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        GetComponent<BoxCollider2D>().enabled = false; //Disable Hitbox
        if (transform.Find("Visual")) transform.Find("Visual").gameObject.SetActive(false); //Disable Visuals
        GameObject projectile = Instantiate(hitEffectPrefab, hitEffectSpawn.position, hitEffectSpawn.rotation); //Spawn Hit Effect
    }
}
