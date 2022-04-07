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
    float maxViewSize;

    GameObject player;
    Transform lookAt;

    void Awake()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
        lookAt = player.GetComponentInChildren<Transform>().Find("CameraLookAt");

        maxViewSize = viewSize * 2;
    }

    void FixedUpdate()
    {
        //View Size Zooming
        var distance = player.GetComponent<Rigidbody2D>().velocity.magnitude * 0.1f;
        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, Mathf.Min(viewSize + distance, maxViewSize), 0.1f);

        //Follow "CameraLookAt" Gameobject's Position
        Vector3 point = GetComponent<Camera>().WorldToViewportPoint(lookAt.position);
        Vector3 delta = lookAt.position - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
        Vector3 destination = new Vector3(lookAt.position.x + offset.x, lookAt.position.y + offset.y, offset.z - 10) + delta;
        transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, movementDamping);
    }
}
