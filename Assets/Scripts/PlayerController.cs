using UnityEngine;
using XInputDotNetPure;

public class PlayerController : EnergyHost
{
    private Rigidbody2D rb; //Rigidbody

    [Range(100, 1000)] public float maxMoveSpeed = 750; //Max Move Speed
    [Range(10, 100)] public float maxRotationResistance = 50; //Rotation Resistance
    [Range(10, 500)] public float drag = 10; //Drag
    [Range(0, 10)] public float scopeDistance = 6; //Distance Twoards Player's Direction
    [SerializeField] Camera playerCamera; //Camera
    [SerializeField] Transform cameraLookAt; //Camera's Target

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

    bool rushing = false; //Rushing Bool

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); //Get Rigidbody
        rb.drag = drag; //Apply Drag Variable to Rigidbody Drag
    }

    PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;
    void Fire() //Firing Method
    {
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation); //Spawn Projectile
        fireSpeed = (maxMoveSpeed) / -4; //Firing Move Speed
        timeSinceLastFire = 0; //Confirm Fire

        GamePad.SetVibration(playerIndex, 0.5f, 0.5f);
    }

    void Rush() //Rushing Method
    {
        transform.Find("Trail").GetComponent<TrailRenderer>().emitting = true; //Enable Trail
        rushSpeed = (maxMoveSpeed) / 4; //Rushing Move Speed

        float deadzone = 0.2f; //For Joysticks
        if (YMove <= deadzone && YMove >= -deadzone && XMove <= deadzone && XMove >= -deadzone || Input.GetButtonDown("Rush") || Input.GetButtonDown("Rush2")) rushing = false; //Stop Rushing If Input Is Lost OR Rush Is Pressed Again
        else rushing = true; //Keep Rushing
    }

    void Update()
    {
        if (Input.GetAxis("LX") == 0 && Input.GetAxis("LY") == 0) //If Joystick Movement Isn't Detected, Enable Keyboard Movement
        {
            XMove = Input.GetAxisRaw("Horizontal");
            YMove = Input.GetAxisRaw("Vertical");
        }
        if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)  //If Keyboard Movement Isn't Detected, Enable Joystick Movement
        {
            XMove = Input.GetAxis("LX");
            YMove = Input.GetAxis("LY");
        }

        timeSinceLastFire += Time.deltaTime; //Count Time Since Fire
        if (Input.GetAxisRaw("Fire1") > 0)  //Shooting Input
        {
            if (timeSinceLastFire > firesPerSecond) //Fires Per Second If Statement
            {
                Fire();
            }
        }
        else fireSpeed = 0; //Return To Original Move Speed

        if (Input.GetAxisRaw("Rush") > 0 || Input.GetAxisRaw("Rush2") > 0 || rushing) //Rushing Input
        {
            Rush();
        }
        else rushSpeed = 0; //Return To Original Move Speed
    }

    void FixedUpdate()
    {
        LerpColor(); //Lerp Color From EnergyHost.cs

        transform.Find("Trail").GetComponent<TrailRenderer>().emitting = false; //Disable Trail

        rb.AddForce(playerCamera.transform.right * XMove * (maxMoveSpeed + rushSpeed + fireSpeed)); //Apply XMovement
        rb.AddForce(playerCamera.transform.up * YMove * (maxMoveSpeed + rushSpeed + fireSpeed));  //Apply YMovement

        cameraLookAt.position = transform.position; //Center Camera's Target At Player Position

        if (Input.GetMouseButton(1)) //Rotation with Right Mouse Click
        {
            Vector3 mouseScreenPos = playerCamera.ScreenToWorldPoint(Input.mousePosition); //Get Mouse Position
            cameraLookAt.position = new Vector3(Mathf.MoveTowards(transform.position.x, mouseScreenPos.x, scopeDistance), Mathf.MoveTowards(transform.position.y, mouseScreenPos.y, scopeDistance)); //Move Camera's Target To Mouse OR Max Scope Distance

            Vector3 difference = mouseScreenPos - transform.position; //Get Mouse & Player Position Difference
            difference.Normalize();
            float z = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg; //Calculate Desiret Rotation
            var desiredRot = Quaternion.Euler(0f, 0f, z - 90); //Convert Desiret Rotation Amount to Quaternion
            transform.rotation = Quaternion.Lerp(transform.rotation, desiredRot, Time.deltaTime * maxRotationResistance); //Rotate Using The Desiret Quaternion
        }

        if (Input.GetAxis("RX") != 0 || Input.GetAxis("RY") != 0) //Rotation with Joystick Only  If Input Is Detected
        {
            //Move Camera's Target Based On Joystick Input Times Max Scope Distance:
            cameraLookAt.position = new Vector3(transform.position.x + Input.GetAxis("RX") * scopeDistance, transform.position.y + Input.GetAxis("RY") * scopeDistance);
            //Rotate Player's Front Parallel To Joystick Based On Screen:
            transform.up = Vector3.SmoothDamp(transform.up, new Vector3(Input.GetAxis("RX"), Input.GetAxis("RY")), ref velocity, maxRotationResistance / 500);
            //Adapt Rotation Resistance Based On Input Strength:
            maxRotationResistance = Mathf.Lerp(maxRotationResistance, (50 - (Mathf.Max(Mathf.Abs(Input.GetAxis("RX")), Mathf.Abs(Input.GetAxis("RY")))) * 10), 1);
        }

        if (!Input.GetMouseButton(1) && Input.GetAxis("RX") == 0 && Input.GetAxis("RY") == 0) //If Rotational Input Isn't Detected
        {
            transform.up = Vector3.SmoothDamp(transform.up, new Vector3(XMove, YMove), ref velocity, maxRotationResistance / 500); //Rotate Player's Front Parallel To Input Based On Screen
            cameraLookAt.position = new Vector3(transform.position.x + XMove * scopeDistance, transform.position.y + YMove * scopeDistance); //Move Camera's Target Based On Input Times Max Scope Distance:
        }
    }
}
