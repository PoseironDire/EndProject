using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int playerNumber;
    PlayerController controller;

    void Awake()
    {
        controller = transform.GetComponentInChildren<PlayerController>();
    }

    void Update()
    {
        if (playerNumber == 1)  //Player1
        {
            controller.XMove = Input.GetAxis("LX");
            controller.YMove = Input.GetAxis("LY");

            if (Input.GetAxisRaw("Rush2") > 0 || controller.rushing) //Rushing Input
            {
                controller.Rush();
            }
            else controller.applyRushSpeed = 0; //Return To Original Move Speed

            if (Input.GetAxisRaw("R2") > 0 || controller.attacking)  //Shooting Input
            {
                controller.Attack();
            }
        }

        if (playerNumber == 2) //Player2
        {
            controller.XMove = Input.GetAxisRaw("Horizontal");
            controller.YMove = Input.GetAxisRaw("Vertical");

            if (Input.GetAxisRaw("Rush") > 0 || controller.rushing) //Rushing Input
            {
                controller.Rush();
            }
            else controller.applyRushSpeed = 0; //Return To Original Move Speed

            if (Input.GetAxisRaw("Fire1") > 0 || controller.attacking)  //Shooting Input
            {
                controller.Attack();
            }
        }
    }

    void FixedUpdate()
    {
        if (playerNumber == 1) //Player1
        {
            if (Input.GetAxis("RX") != 0 || Input.GetAxis("RY") != 0) //Rotation with Joystick Only  If Input Is Detected
            {
                controller.JoystickRotation();
            }
            else //If Rotational Input Isn't Detected
            {
                controller.FreeRotation();
            }
        }

        if (playerNumber == 2) //Player2
        {
            if (Input.GetMouseButton(1)) //Rotation with Right Mouse Click
            {
                controller.MouseRotation();
            }
            else //If Rotational Input Isn't Detected
            {
                controller.FreeRotation();
            }
        }
    }
}
