using UnityEngine;

public class PlayerController : EnergyHost
{
    private Rigidbody2D rb; //Rigidbody

    [Range(100, 1000)] public float maxMoveSpeed = 750; //Max Move Speed
    [Range(100, 1000)] public float rushSpeedBonus = 150; //Speed While Rushing
    [Range(-1000, -100)] public float fireSpeedPenalty = -500; //Speed While Firing
    [Range(10, 100)] public float maxRotationResistance = 50; //Rotation Resistance
    [Range(10, 500)] public float drag = 10; //Drag
    [Range(0, 10)] public float scopeDistance = 1; //Distance Twoards Player's Direction
    public Camera playerCamera; //Camera
    public Transform cameraLookAt; //Camera's Target

    [HideInInspector] public Vector3 velocity = Vector3.zero; //Vector Zero
    [HideInInspector] public bool rushing = false; //Rushing Bool
    [HideInInspector] public float applyRushSpeed = 0f; //Time Since Last Burst
    [HideInInspector] public bool firing = false; //Firing Bool
    [HideInInspector] public float applyFireSpeed = 0f; //Time Since Last Burst
    [HideInInspector] public float XMove; //X Movement Input 
    [HideInInspector] public float YMove; //Y Movement Input

    float timeSinceLastFire = 0f; //Time Since Last Fire
    float timeSinceLastBurst = 0f; //Time Since Last Burst
    int storedShots = 0;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); //Get Rigidbody
        rb.drag = drag; //Apply Drag Variable to Rigidbody Drag
    }

    public void Fire() //Firing Method
    {
        if (!firing) //Trigger Burst
        {
            storedShots = 0;
            timeSinceLastBurst = 0;
            firing = true;
        }
        if (firing && timeSinceLastFire > ((timeBetweenShots * 5) / (float)projectileBurst) / 10 && storedShots < projectileBurst)
        {
            applyFireSpeed = fireSpeedPenalty;
            GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation); //Spawn Projectile
            projectile.transform.SetParent(transform.parent); //Make The Projectile A Child Of This Game Object 
            Physics2D.IgnoreCollision(projectile.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            timeSinceLastFire = 0;
            storedShots++;
        }

        if (timeSinceLastBurst > timeBetweenShots)
        {
            firing = false;
        }
    }

    public void Rush() //Rushing Method
    {
        applyRushSpeed = rushSpeedBonus;
        transform.Find("Trail").GetComponent<TrailRenderer>().emitting = true; //Enable Trail

        float deadzone = 0.2f; //For Joysticks
        if (YMove <= deadzone && YMove >= -deadzone && XMove <= deadzone && XMove >= -deadzone || Input.GetButtonDown("Rush") || Input.GetButtonDown("Rush2")) rushing = false; //Stop Rushing If Input Is Lost OR Rush Is Pressed Again
        else rushing = true; //Keep Rushing
    }

    public void JoystickRotation()
    {
        //Move Camera's Target Based On Joystick Input Times Max Scope Distance:
        cameraLookAt.position = new Vector3(transform.position.x + Input.GetAxis("RX") * scopeDistance, transform.position.y + Input.GetAxis("RY") * scopeDistance);
        //Rotate Player's Front Parallel To Joystick Based On Screen:
        transform.up = Vector3.SmoothDamp(transform.up, new Vector3(Input.GetAxis("RX"), Input.GetAxis("RY")), ref velocity, maxRotationResistance / 500);
        //Adapt Rotation Resistance Based On Input Strength:
        maxRotationResistance = Mathf.Lerp(maxRotationResistance, (50 - (Mathf.Max(Mathf.Abs(Input.GetAxis("RX")), Mathf.Abs(Input.GetAxis("RY")))) * 10), 1);
    }

    public void MouseRotation()
    {
        Vector3 mouseScreenPos = playerCamera.ScreenToWorldPoint(Input.mousePosition); //Get Mouse Position
        cameraLookAt.position = new Vector3(Mathf.MoveTowards(transform.position.x, mouseScreenPos.x, scopeDistance), Mathf.MoveTowards(transform.position.y, mouseScreenPos.y, scopeDistance)); //Move Camera's Target To Mouse OR Max Scope Distance
        Vector3 difference = mouseScreenPos - transform.position; //Get Mouse & Player Position Difference
        difference.Normalize();
        float z = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg; //Calculate Desiret Rotation
        var desiredRot = Quaternion.Euler(0f, 0f, z - 90); //Convert Desiret Rotation Amount to Quaternion
        transform.rotation = Quaternion.Lerp(transform.rotation, desiredRot, Time.deltaTime * maxRotationResistance); //Rotate Using The Desiret Quaternion

    }

    public void FreeRotation()
    {
        transform.up = Vector3.SmoothDamp(transform.up, new Vector3(XMove, YMove), ref velocity, maxRotationResistance / 500); //Rotate Player's Front Parallel To Input Based On Screen
        cameraLookAt.position = new Vector3(transform.position.x + XMove * scopeDistance, transform.position.y + YMove * scopeDistance); //Move Camera's Target Based On Input Times Max Scope Distance:
    }
    void Update()
    {
        timeSinceLastFire += Time.deltaTime; //Count Time Since Fire
        timeSinceLastBurst += Time.deltaTime; //Count Time Since Burst
    }

    void FixedUpdate()
    {
        LerpColor(); //Lerp Color From EnergyHost.cs

        transform.Find("Trail").GetComponent<TrailRenderer>().emitting = false; //Disable Trail

        rb.AddForce(playerCamera.transform.right * XMove * (maxMoveSpeed + applyRushSpeed + applyFireSpeed)); //Apply XMovement
        rb.AddForce(playerCamera.transform.up * YMove * (maxMoveSpeed + applyRushSpeed + applyFireSpeed));  //Apply YMovement

        cameraLookAt.position = transform.position; //Center Twoards Camera's Target
    }
}