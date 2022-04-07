using UnityEngine;

public class PlayerController : HealthHost
{
    //Player
    [SerializeField] GameObject player;
    private Rigidbody2D rb;

    //Controlls & Camera
    [Range(100, 1000)] public float maxSpeedY = 1000;
    [Range(100, 1000)] public float maxSpeedX = 1000;
    [Range(10, 100)] public float maxSpeedRot = 35;

    [Range(10, 500)] public float drag = 20;
    [Range(0, 10)] public float scopeDistance = 4;
    [SerializeField] Camera playerCamera;
    [SerializeField] Transform cameraLookAt;
    float rushSpeed = 0;
    float rightMove = 0;
    float upMove = 0;

    //Shooting
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform bulletSpawnPoint;
    [SerializeField][Range(10, 1000)] float initialBulletVelocity = 100;
    [SerializeField][Range(0, 10)] float initialBulletSpread = 1;
    [SerializeField][Range(0, 1)] float bulletLifeTime = 0.7f;
    [SerializeField][Range(0, 1)] float timeBetweenShots = 0.5f;
    private float timeSinceLastShot = 0f;

    //Rushing
    bool hasReleasedRushButton = true;
    bool rushing = false;

    void Awake()
    {
        //Get Components
        rb = GetComponent<Rigidbody2D>();
    }

    void Fire() //Shooting Method
    {
        bulletPrefab.GetComponent<BulletController>().initialVelocity = initialBulletVelocity;
        bulletPrefab.GetComponent<BulletController>().initialSpread = initialBulletSpread;
        bulletPrefab.GetComponent<BulletController>().lifeTime = bulletLifeTime;

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        timeSinceLastShot = 0;
    }

    void Update()
    {
        rb.drag = drag;

        //Disable Trail
        player.transform.Find("Trail").GetComponent<TrailRenderer>().emitting = false;

        //Shooting Input
        timeSinceLastShot += Time.deltaTime;
        if (Input.GetAxisRaw("Fire1") > 0)
        {
            if (timeSinceLastShot > timeBetweenShots)
            {
                Fire();
            }
        }

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

        //Movement
        if (Input.GetAxis("LX") == 0 && Input.GetAxis("LY") == 0)
        {
            rightMove = Input.GetAxisRaw("Horizontal");
            upMove = Input.GetAxisRaw("Vertical");
        }
        if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
        {
            rightMove = Input.GetAxis("LX");
            upMove = Input.GetAxis("LY");
        }

        rb.AddForce(playerCamera.transform.right * rightMove * (maxSpeedY + rushSpeed));
        rb.AddForce(playerCamera.transform.up * upMove * (maxSpeedX + rushSpeed));

        Vector3 velocity = Vector3.zero;

        //Rotation with Mouse
        if (Input.GetMouseButton(1)) //Right Mouse Click
        {
            Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            difference.Normalize();
            float z = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            var desiredRot = Quaternion.Euler(0f, 0f, z - 90);
            transform.rotation = Quaternion.Lerp(transform.rotation, desiredRot, Time.deltaTime * maxSpeedRot); //Smooth Input
            //Mouse Scope
            Vector3 mouseScreenPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            cameraLookAt.position = new Vector3(Mathf.MoveTowards(transform.position.x, mouseScreenPos.x, scopeDistance), Mathf.MoveTowards(transform.position.y, mouseScreenPos.y, scopeDistance));
        }
        else cameraLookAt.position = transform.position;

        //Rotation with Joystick
        if (Input.GetAxis("RX") != 0 || Input.GetAxis("RY") != 0)  //Only Rotate if Input is detected
        {
            transform.up = Vector3.SmoothDamp(transform.up, new Vector3(Input.GetAxis("RX"), Input.GetAxis("RY")), ref velocity, maxSpeedRot / 500); //Smooth Input
            //Joystick Scope
            cameraLookAt.position = new Vector3(transform.position.x + Input.GetAxis("RX") * scopeDistance, transform.position.y + Input.GetAxis("RY") * scopeDistance);
        }

        //Disable Strafe
        if (!Input.GetMouseButton(1) && Input.GetAxis("RX") == 0 && Input.GetAxis("RY") == 0)
        {
            maxSpeedRot = 25;
            transform.up = Vector3.SmoothDamp(transform.up, new Vector3(rightMove, upMove), ref velocity, maxSpeedRot / 500); //Smooth Input
            //Scope
            cameraLookAt.position = new Vector3(transform.position.x + rightMove * scopeDistance, transform.position.y + upMove * scopeDistance);
        }
        else maxSpeedRot = Mathf.Lerp(maxSpeedRot, (35 - (Mathf.Max(Mathf.Abs(Input.GetAxis("RX")), Mathf.Abs(Input.GetAxis("RY")))) * 10), 1);
    }
}
