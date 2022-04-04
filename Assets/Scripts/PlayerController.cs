using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Physics
    private Rigidbody2D rb2D;

    //Controlls & Camera
    private ControllerTypes controllerTypes;
    [Range(100, 1000)] public float maxSpeedY;
    [Range(100, 1000)] public float maxSpeedX;
    [Range(10, 100)] public float maxSpeedRot;
    [Range(10, 500)] public float drag;
    [SerializeField] Camera playerCamera;
    private float rushSpeed = 0;

    //Shooting
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform bulletSpawnPoint;
    [SerializeField] float timeBetweenShots = 0.5f;
    private float timeSinceLastShot = 0f;

    void Start()
    {
        //Get Components
        rb2D = GetComponent<Rigidbody2D>();
        controllerTypes = GetComponent<ControllerTypes>();
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
        }
        //Deadzones for Joysticks
        float deadzone = 0.2f;
        if (Input.GetAxis("LY") <= deadzone && Input.GetAxis("LY") >= -deadzone && Input.GetAxis("LX") <= deadzone && Input.GetAxis("LX") >= -deadzone)
            rushing = false;
    }

    void FixedUpdate()
    {
        //Movement
        float right = (controllerTypes.xboxOneController == 1 || controllerTypes.ps4Controller == 1) ? Input.GetAxis("LX") : Input.GetAxisRaw("Horizontal");
        rb2D.AddForce(playerCamera.transform.right * right * (maxSpeedY + rushSpeed));
        float up = (controllerTypes.xboxOneController == 1 || controllerTypes.ps4Controller == 1) ? Input.GetAxis("LY") : Input.GetAxisRaw("Vertical");
        rb2D.AddForce(playerCamera.transform.up * up * (maxSpeedX + rushSpeed));

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
            transform.up = Vector3.SmoothDamp(transform.up, new Vector3(right, up), ref velocity, maxSpeedRot / 500); //Smooth Input
        }
    }
}
