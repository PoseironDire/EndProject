using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody2D rb2D; //Rigidbody
    GameObject player; //Player
    PlayerController controller; //Player

    [SerializeField] GameObject hitEffectPrefab; //Hit Effect
    [SerializeField] Transform hitEffectSpawn; //Hit Effect Spawn Point

    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>(); //Get Rigidbody
        player = FindObjectOfType<PlayerController>().gameObject; //Get Player
        controller = player.GetComponent<PlayerController>(); //Get Controller

        //Get Random Spread
        rb2D.velocity = transform.up * controller.initialProjectileVelocity + transform.right * UnityEngine.Random.Range(controller.initialProjectileSpread, -controller.initialProjectileSpread); ;

        //Config Sound
        GetComponent<AudioSource>().volume = controller.projectileVolume;
        GetComponent<AudioSource>().pitch = UnityEngine.Random.Range(controller.minProjectilePitch, controller.maxProjectilePitch);

        Destroy(this.gameObject, controller.projectileLifeTime); //Destroy
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
        projectile.transform.SetParent(transform.parent); //Make The Hit Effect A Child Of This Game Object 
    }
}
