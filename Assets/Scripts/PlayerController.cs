using UnityEngine;

public class PlayerController : EnergyHost
{
    private Rigidbody2D rb; //Rigidbody

    [Range(100, 1000)] public float maxMoveSpeed = 750; //Max Move Speed
    [Range(100, 1000)] public float rushSpeedBonus = 150; //Speed While Rushing
    [Range(-1000, -100)] public float fireSpeedPenalty = -500; //Speed While Firing
    [Range(0.01f, 1)] public float maxRotationResistance = 50; //Rotation Resistance
    [Range(10, 500)] public float drag = 10; //Drag
    [Range(0, 10)] public float scopeDistance = 1; //Distance Twoards Player's Direction
    public bool ranged = false; //Distance Twoards Player's Direction
    public Camera playerCamera; //Camera
    public Transform cameraLookAt; //Camera's Target
    Animator animator; //Animator

    [HideInInspector] public Vector3 velocity = Vector3.zero; //Vector Zero
    [HideInInspector] public bool rushing = false; //Rushing Bool
    [HideInInspector] public float applyRushSpeed = 0f; //Time Since Last Burst
    [HideInInspector] public bool attacking = false; //Firing Bool
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

        animator = GetComponentInChildren<Animator>();
    }

    public void Attack() //Firing Method
    {
        if (ranged)
        {

            if (!attacking) //Trigger Burst
            {
                storedShots = 0;
                timeSinceLastBurst = 0;
                attacking = true;
            }

            if (attacking && timeSinceLastFire > ((timeBetweenBursts * (burstTime * 10)) / (float)projectileBurst) / 10 && storedShots < projectileBurst)
            {
                applyFireSpeed = fireSpeedPenalty;
                GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation); //Spawn Projectile
                projectile.transform.SetParent(transform.parent); //Make The Projectile A Child Of This Game Object 
                Physics2D.IgnoreCollision(projectile.GetComponent<Collider2D>(), GetComponent<Collider2D>());
                timeSinceLastFire = 0;
                storedShots++;
            }
        }
        else
        {
            if (!attacking) //Trigger Burst
            {
                applyFireSpeed = fireSpeedPenalty;
                animator.SetTrigger("Attack");
                timeSinceLastBurst = 0;
                attacking = true;
            }
        }

        if (timeSinceLastBurst > timeBetweenBursts)
        {
            applyFireSpeed = 0; //Return To Original Move Speed
            attacking = false;
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
        float rotationResistance = maxRotationResistance;
        transform.up = Vector3.SmoothDamp(transform.up, new Vector3(Input.GetAxis("RX"), Input.GetAxis("RY")), ref velocity, rotationResistance / 2);
        //Adapt Rotation Resistance Based On Input Strength:
        rotationResistance = Mathf.Lerp(rotationResistance, (50 - (Mathf.Max(Mathf.Abs(Input.GetAxis("RX")), Mathf.Abs(Input.GetAxis("RY")))) * 10), 1);
    }

    public void MouseRotation()
    {
        Vector3 mouseScreenPos = playerCamera.ScreenToWorldPoint(Input.mousePosition); //Get Mouse Position
        Vector3 difference = mouseScreenPos - transform.position; //Get Mouse & Player Position Difference
        cameraLookAt.position = new Vector3(Mathf.MoveTowards(transform.position.x, mouseScreenPos.x, scopeDistance), Mathf.MoveTowards(transform.position.y, mouseScreenPos.y, scopeDistance)); //Move Camera's Target To Mouse OR Max Scope Distance
        transform.up = Vector3.SmoothDamp(transform.up, new Vector3(difference.x, difference.y), ref velocity, maxRotationResistance); //Rotate Player's Front Twoards Mouse Based On Screen
    }

    public void FreeRotation()
    {
        transform.up = Vector3.SmoothDamp(transform.up, new Vector3(XMove, YMove), ref velocity, maxRotationResistance); //Rotate Player's Front Parallel To Input Based On Screen
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