using UnityEngine;

public class NpcController : HealthHost
{
    //Physics
    [SerializeField] GameObject player;
    private Rigidbody2D rb2D;

    //Controlls
    [Range(100, 1000)] public float maxSpeedY = 1000;
    [Range(100, 1000)] public float maxSpeedX = 1000;
    [Range(10, 100)] public float maxSpeedRot = 30;
    [Range(10, 500)] public float drag = 20;
    float rushSpeed = 0;
    float rightMove = 0;
    float upMove = 0;

    //Shooting
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform bulletSpawnPoint;
    [SerializeField] float timeBetweenShots = 0.5f;
    private float timeSinceLastShot = 0f;

    //Rushing
    bool hasReleasedRushButton = true;
    bool rushing = false;

    void Start()
    {
        //Get Components
        rb2D = GetComponent<Rigidbody2D>();
    }

    void Fire() //Shooting Method
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        timeSinceLastShot = 0;
    }

    void Update()
    {
        rb2D.drag = drag;

        timeSinceLastShot += Time.deltaTime;
        if (timeSinceLastShot > timeBetweenShots)
        {
            Fire();
        }

        //Disable Trail
        player.transform.Find("Trail").GetComponent<TrailRenderer>().emitting = false;

        //Rush Input
        if (Input.GetAxisRaw("Rush") > 0 || Input.GetAxisRaw("Rush2") > 0)
        {
            if (hasReleasedRushButton)
            {
                rushing = !rushing;
                hasReleasedRushButton = false;
            }
        }
        else
        {
            hasReleasedRushButton = true;
        }
        if (rushing)
        {
            rushSpeed = (maxSpeedX + maxSpeedY) / 4;
            player.transform.Find("Trail").GetComponent<TrailRenderer>().emitting = true;
        }
        else
        {
            rushSpeed = 0;
        }
        //Deadzones for Joysticks
        float deadzone = 0.2f;
        if (upMove <= deadzone && upMove >= -deadzone && rightMove <= deadzone && rightMove >= -deadzone)
            rushing = false;
    }


    void FixedUpdate()
    {
        Coloring();

        float degreesPerSecond = 20;
        transform.Rotate(new Vector3(0, 0, degreesPerSecond) * Time.deltaTime);
    }
}
