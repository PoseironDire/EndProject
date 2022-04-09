using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField][Range(0, 30)] float viewSize = 10; //View Size
    [SerializeField][Range(0, 1)] float movementDamping = 0.5f; //Movement Damping
    public Vector3 offset;

    GameObject player; //Player
    Transform lookAt; //What Object To Follow

    float maxViewSize; //Max View Size

    public Matrix4x4 originalProjection;
    Camera cam;

    void Start()
    {
        player = transform.parent.Find("Body").gameObject;
        lookAt = player.GetComponentInChildren<Transform>().Find("CameraLookAt");

        maxViewSize = viewSize * 2;

        cam = GetComponent<Camera>();
        originalProjection = cam.projectionMatrix;
    }

    // void Update()
    // {
    //     Matrix4x4 p = originalProjection;
    //     p.m01 += Mathf.Sin(Time.time * 1.2F) * 0.1F;
    //     p.m10 += Mathf.Sin(Time.time * 1.5F) * 0.1F;
    //     cam.projectionMatrix = p;
    // }

    Vector3 velocity = Vector3.zero;
    void FixedUpdate()
    {
        //View Size Zooming
        var distance = player.GetComponent<Rigidbody2D>().velocity.magnitude * 0.1f;
        GetComponent<Camera>().orthographicSize = Mathf.Lerp(GetComponent<Camera>().orthographicSize, Mathf.Min(viewSize + distance, maxViewSize), 0.1f);

        //Follow "CameraLookAt" Gameobject's Position
        Vector3 point = GetComponent<Camera>().WorldToViewportPoint(lookAt.position);
        Vector3 delta = lookAt.position - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
        Vector3 destination = new Vector3(lookAt.position.x + offset.x, lookAt.position.y + offset.y, -10) + delta;
        transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, movementDamping);
    }
}
