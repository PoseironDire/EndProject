using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Physics
    [HideInInspector] public Rigidbody2D rb2D;
    private Vector3 velocity = Vector3.zero;
    //Controlls & Camera
    [Range(100, 1000)] public float maxSpeedY;
    [Range(100, 1000)] public float maxSpeedX;
    [Range(10, 100)] public float maxSpeedRot;
    [Range(10, 500)] public float drag;
    float rushSpeed = 0;
    [SerializeField] ControllerTypes controllerTypes;
    [SerializeField] Camera playerCamera;
    //Shooting
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform bulletSpawnPoint;
    [SerializeField] float timeBetweenShots = 0.5f;
    float timeSinceLastShot = 0f;
    //Inputs
    float kXInput;
    float kYInput = -1;
    float lXCInput;
    float lYCInput = -1;
    float rXCInput;
    float rYCInput;

    void Start()
    {
        GameObject.Find("Trail").GetComponent<TrailRenderer>().emitting = false;
        rb2D = GetComponent<Rigidbody2D>();
    }

    void Fire()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
    }

    void Update()
    {
        rb2D.drag = drag;

        //Movement Inputs
        kXInput = Input.GetAxisRaw("Horizontal");
        kYInput = Input.GetAxisRaw("Vertical");
        lXCInput = Input.GetAxis("LX");
        lYCInput = Input.GetAxis("LY");
        rXCInput = Input.GetAxis("RX");
        rYCInput = Input.GetAxis("RY");

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
        bool hasReleasedRushButton = true;
        bool rushing = false;
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
            GameObject.Find("Trail").GetComponent<TrailRenderer>().emitting = false;
        }
        float deadzone = 0.2f;
        if (lYCInput <= deadzone && lYCInput >= -deadzone && lXCInput <= deadzone && lXCInput >= -deadzone)
            rushing = false;
    }

    void FixedUpdate()
    {
        //Movement with Keyboard
        if (controllerTypes.xboxOneController != 1 && controllerTypes.ps4Controller != 1)
        {
            rb2D.AddForce(playerCamera.transform.right * (kXInput * (maxSpeedY + rushSpeed)));
            rb2D.AddForce(playerCamera.transform.up * (kYInput * (maxSpeedX + rushSpeed)));
        }
        //Movement with Joystick
        else if (controllerTypes.xboxOneController == 1 || controllerTypes.ps4Controller == 1)
        {
            rb2D.AddForce(playerCamera.transform.right * (lXCInput * (maxSpeedY + rushSpeed)));
            rb2D.AddForce(playerCamera.transform.up * (lYCInput * (maxSpeedX + rushSpeed)));
        }

        //Rotation with Mouse
        if (Input.GetMouseButton(1)) //Right Mouse Click
        {
            Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            difference.Normalize();
            float z = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            var desiredRot = Quaternion.Euler(0f, 0f, z - 90);
            transform.rotation = Quaternion.Lerp(transform.rotation, desiredRot, Time.deltaTime * maxSpeedRot); //Smooth Input
        }
        //Rotation with Joystick
        if (rXCInput != 0 || rYCInput != 0)  //Only Rotate if Input is detected
        {
            transform.up = Vector3.SmoothDamp(transform.up, new Vector3(rXCInput, rYCInput), ref velocity, maxSpeedRot / 500); //Smooth Input
        }
        //Disable Strafe
        if (!Input.GetMouseButton(1) && rXCInput == 0 && rYCInput == 0)
        {
            transform.up = Vector3.SmoothDamp(transform.up, new Vector3(lXCInput, lYCInput), ref velocity, maxSpeedRot / 500); //Smooth Input
        }
    }
}
