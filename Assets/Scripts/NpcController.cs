using UnityEngine;

public class NpcController : EnergyHost
{
    private Rigidbody2D rb; //Rigidbody

    [Range(100, 1000)] public float maxMoveSpeed = 750; //Max Move Speed
    [Range(10, 100)] public float maxRotationResistance = 50; //Rotation Resistance
    [Range(10, 500)] public float drag = 10; //Drag

    Vector3 velocity = Vector3.zero; //Vector Zero
    float rushSpeed; //Speed While Rushing
    float fireSpeed; //Speed While Firing
    float XMove; //X Movement Input 
    float YMove; //Y Movement Input

    [SerializeField] GameObject projectilePrefab; //Projectile
    [SerializeField] Transform projectileSpawnPoint; //Projectile Spawn Point
    [Range(30, 300)] public float initialProjectileVelocity = 100; //Projectile Speed
    [Range(0, 10)] public float initialProjectileSpread = 1; //Projectile Spread
    [Range(0, 1)] public float projectileLifeTime = 0.7f; //Projectile Life Time
    [Range(0, 1)] public float firesPerSecond = 0.5f; //Fires Per Second
    [Range(0, 1)] public float projectileVolume = 0.5f; //Volume
    [Range(0, 5)] public float minPitch = 1; //Minimum Pitch
    [Range(0, 5)] public float maxPitch = 2.5f; //Maximum Pitch

    private float timeSinceLastFire = 0f; //Time Since Last Fire

    // bool rushing = false; //Rushing Bool

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); //Get Rigidbody
        rb.drag = drag; //Apply Drag Variable to Rigidbody Drag
    }

    void Fire() //Firing Method
    {
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation); //Spawn Projectile
        fireSpeed = (maxMoveSpeed) / -4; //Firing Move Speed
        timeSinceLastFire = 0; //Confirm Fire
    }

    // void Rush() //Rushing Method
    // {
    //     transform.Find("Trail").GetComponent<TrailRenderer>().emitting = true; //Enable Trail
    //     rushSpeed = (maxMoveSpeed) / 4; //Rushing Move Speed

    //     float deadzone = 0.2f; //For Joysticks
    //     if (YMove <= deadzone && YMove >= -deadzone && XMove <= deadzone && XMove >= -deadzone || Input.GetButtonDown("Rush") || Input.GetButtonDown("Rush2")) rushing = false; //Stop Rushing If Input Is Lost OR Rush Is Pressed Again
    //     else rushing = true; //Keep Rushing
    // }

    void Update()
    {
        timeSinceLastFire += Time.deltaTime; //Count Time Since Fire
        if (timeSinceLastFire > firesPerSecond) //Fires Per Second If Statement
        {
            Fire();
        }
    }

    void FixedUpdate()
    {
        LerpColor(); //Lerp Color From EnergyHost.cs

        transform.Find("Trail").GetComponent<TrailRenderer>().emitting = false; //Disable Trail

        float degreesPerSecond = 30;
        transform.Rotate(new Vector3(0, 0, degreesPerSecond) * Time.deltaTime); //Rotate NPC
    }
}