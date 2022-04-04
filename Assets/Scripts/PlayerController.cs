using UnityEngine;

public class PlayerController : HealthHost
{
    //Physics
    private Rigidbody2D rb2D;

    //Controlls & Camera
    [Range(100, 1000)] public float maxSpeedY;
    [Range(100, 1000)] public float maxSpeedX;
    [Range(10, 100)] public float maxSpeedRot;
    [Range(10, 500)] public float drag;
    [SerializeField] Camera playerCamera;
    private float rushSpeed = 0;
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
    }

    void Update()
    {
        rb2D.drag = drag;

        //Disable Trail
        GameObject.Find("Trail").GetComponent<TrailRenderer>().emitting = false;

        //Shooting Input
        timeSinceLastShot += Time.deltaTime;
        if (Input.GetAxisRaw("Fire1") > 0)
        {
            if (timeSinceLastShot > timeBetweenShots)
            {
                Fire();
                timeSinceLastShot = 0;
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
            GetComponent<SpriteRenderer>().color = Color.blue;
            GameObject.Find("Handle").GetComponent<SpriteRenderer>().color = Color.blue;
            GameObject.Find("Trail").GetComponent<TrailRenderer>().emitting = true;
        }
        else
        {
            rushSpeed = 0;
            GetComponent<SpriteRenderer>().color = Color.white;
            GameObject.Find("Handle").GetComponent<SpriteRenderer>().color = Color.white;
        }
        //Deadzones for Joysticks
        float deadzone = 0.2f;
        if (upMove <= deadzone && upMove >= -deadzone && rightMove <= deadzone && rightMove >= -deadzone)
            rushing = false;
    }

    void FixedUpdate()
    {
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

        rb2D.AddForce(playerCamera.transform.right * rightMove * (maxSpeedY + rushSpeed));
        rb2D.AddForce(playerCamera.transform.up * upMove * (maxSpeedX + rushSpeed));

        //Rotation with Mouse
        if (Input.GetMouseButton(1)) //Right Mouse Click
        {
            Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            difference.Normalize();
            float z = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            var desiredRot = Quaternion.Euler(0f, 0f, z - 90);
            transform.rotation = Quaternion.Lerp(transform.rotation, desiredRot, Time.deltaTime * maxSpeedRot); //Smooth Input
        }

        Vector3 velocity = Vector3.zero;
        //Rotation with Joystick
        if (Input.GetAxis("RX") != 0 || Input.GetAxis("RY") != 0)  //Only Rotate if Input is detected
        {
            transform.up = Vector3.SmoothDamp(transform.up, new Vector3(Input.GetAxis("RX"), Input.GetAxis("RY")), ref velocity, maxSpeedRot / 500); //Smooth Input
        }

        //Disable Strafe
        if (!Input.GetMouseButton(1) && Input.GetAxis("RX") == 0 && Input.GetAxis("RY") == 0)
        {
            transform.up = Vector3.SmoothDamp(transform.up, new Vector3(rightMove, upMove), ref velocity, maxSpeedRot / 500); //Smooth Input
        }
    }
}
