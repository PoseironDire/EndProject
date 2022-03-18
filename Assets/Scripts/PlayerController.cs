using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Range(100, 1000)] public float maxSpeedY;
    [Range(100, 1000)] public float maxSpeedX;
    [Range(10, 100)] public float maxSpeedRot;
    public float drag;

    [HideInInspector] public Rigidbody2D rb2D;
    private Vector3 velocity = Vector3.zero;

    [SerializeField] Camera playerCamera;

    [SerializeField] ControllerTypes controllerTypes;

    [SerializeField]
    GameObject bulletPrefab;

    [SerializeField]
    Transform bulletSpawnPoint;

    [SerializeField]
    float timeBetweenShots = 0.5f;
    float timeSinceLastShot = 0f;

    float leftHorizontalInput;
    float leftVerticalInput = -1;
    float rightHorizontalInput;
    float rightVerticalInput;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rb2D.drag = drag;

        leftHorizontalInput = Input.GetAxis("Horizontal");
        leftVerticalInput = Input.GetAxis("Vertical");
        rightHorizontalInput = Input.GetAxis("X");
        rightVerticalInput = Input.GetAxis("Y");

        timeSinceLastShot += Time.deltaTime;

        if (Input.GetAxisRaw("Fire1") > 0 || Input.GetButton("Dash"))
        {
            if (timeSinceLastShot > timeBetweenShots)
            {
                print("Fired");
                Fire();
                FindObjectOfType<AudioManager>().Play("Bullet");
                timeSinceLastShot = 0;
            }
        }
    }

    void FixedUpdate()
    {
        //Move
        Vector2 horizontal = playerCamera.transform.right * (leftHorizontalInput * maxSpeedY);
        rb2D.AddForce(horizontal);
        Vector2 vertical = playerCamera.transform.up * (leftVerticalInput * maxSpeedX);
        rb2D.AddForce(vertical);

        //Rotation for Keyboard & Mouse
        if (controllerTypes.xboxOneController != 1 && controllerTypes.ps4Controller != 1)
        {
            if (Input.GetMouseButton(1)) //Right Click
            {
                //Look twoards Cursor
                Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                difference.Normalize();
                float z = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
                var desiredRot = Quaternion.Euler(0f, 0f, z - 90);
                transform.rotation = Quaternion.Lerp(transform.rotation, desiredRot, Time.deltaTime * maxSpeedRot); //Smooth Input
            }
        }
        //Rotation for Joysticks
        if (controllerTypes.xboxOneController == 1 || controllerTypes.ps4Controller == 1)
        {
            //Right Joystick
            Vector3 look = new Vector3(rightHorizontalInput, rightVerticalInput);
            if (rightHorizontalInput != 0 || rightVerticalInput != 0)  //Only Rotate if Input is detected
                transform.up = Vector3.SmoothDamp(transform.up, look, ref velocity, maxSpeedRot / 500); //Smooth Input
        }
    }
    void Fire()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
    }
}
