using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Vector3 velocity = Vector3.zero;
    public Vector3 offset;
    [Range(0, 30)] public float viewSize = 10;
    [Range(0, 1)] public float movementDamping = 0.15f;
    [Range(1, 20)] public float rotationDamping = 3;

    GameObject player;
    void Start()
    {

        player = FindObjectOfType<PlayerController>().gameObject;
    }

    void FixedUpdate()
    {
        var distance = player.GetComponent<Rigidbody2D>().velocity.magnitude * 0.5f;
        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, viewSize + distance * 0.2f, 0.1f);

        Vector3 point = GetComponent<Camera>().WorldToViewportPoint(player.transform.position);
        Vector3 delta = player.transform.position - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
        Vector3 destination = new Vector3(player.transform.position.x + offset.x, player.transform.position.y + offset.y, offset.z - 10) + delta;
        transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, movementDamping);
    }
}
