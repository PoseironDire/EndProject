using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int playerNumber;
    public PlayerController player;

    void Update()
    {
        if (playerNumber == 1)  //Player1
        {
            player.XMove = Input.GetAxis("LX");
            player.YMove = Input.GetAxis("LY");

            if (Input.GetAxisRaw("Rush2") > 0 || player.rushing) //Rushing Input
            {
                player.Rush();
            }
            else player.applyRushSpeed = 0; //Return To Original Move Speed

            if (Input.GetAxisRaw("R2") > 0 || player.firing)  //Shooting Input
            {
                player.Fire();
            }
            else player.applyFireSpeed = 0; //Return To Original Move Speed
        }

        if (playerNumber == 2) //Player2
        {
            player.XMove = Input.GetAxisRaw("Horizontal");
            player.YMove = Input.GetAxisRaw("Vertical");

            if (Input.GetAxisRaw("Rush") > 0 || player.rushing) //Rushing Input
            {
                player.Rush();
            }
            else player.applyRushSpeed = 0; //Return To Original Move Speed

            if (Input.GetAxisRaw("Fire1") > 0 || player.firing)  //Shooting Input
            {
                player.Fire();
            }
            else player.applyFireSpeed = 0; //Return To Original Move Speed
        }
    }

    void FixedUpdate()
    {
        if (playerNumber == 1) //Player1
        {
            if (Input.GetAxis("RX") != 0 || Input.GetAxis("RY") != 0) //Rotation with Joystick Only  If Input Is Detected
            {
                player.JoystickRotation();
            }
            else //If Rotational Input Isn't Detected
            {
                player.FreeRotation();
            }
        }

        if (playerNumber == 2) //Player2
        {
            if (Input.GetMouseButton(1)) //Rotation with Right Mouse Click
            {
                player.MouseRotation();
            }
            else //If Rotational Input Isn't Detected
            {
                player.FreeRotation();
            }
        }
    }
}
