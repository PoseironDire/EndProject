using UnityEngine;


public class ControllerTypes : MonoBehaviour
{
    public int xboxOneController = 0;
    public int ps4Controller = 0;
    void Update()
    {
        string[] names = Input.GetJoystickNames();
        for (int x = 0; x < names.Length; x++)
        {
            if (names[x].Length == 19)
            {
                print("PS4 CONTROLLER IS CONNECTED");
                ps4Controller = 1;
            }
            else
            {
                ps4Controller = 0;
            }
            if (names[x].Length == 33)
            {
                print("XBOX ONE CONTROLLER IS CONNECTED");
                //set a controller bool to true
                xboxOneController = 1;
            }
            else
            {
                xboxOneController = 0;
            }
        }
        if (xboxOneController == 1)
        {
            //do something
        }
        else if (ps4Controller == 1)
        {
            //do something
        }
        else
        {
            // there is no controllers
        }
    }

}