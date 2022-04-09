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
        projectile.transform.SetParent(transform.parent); //Make The Projectile A Child Of This Game Object 
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
        if (timeSinceLastFire > timeBetweenShots) //Fires Per Second If Statement
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