using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Camera cam;
    public PlayerController controller;
    public PlayerManager playerManager;

    float deltaTime;
    void Update()
    {
        if (playerManager.playerNumber <= FindObjectOfType<NetworkManager>().decidPlayerCount)
        {
            controller.gameObject.SetActive(true);
            cam.gameObject.SetActive(true);

            if (FindObjectOfType<NetworkManager>().decidPlayerCount == 2)
            {
                if (playerManager.playerNumber == 1) cam.rect = new Rect(cam.rect.x, 0.5f, 1, 0.5f);
                if (playerManager.playerNumber == 2) cam.rect = new Rect(cam.rect.x, cam.rect.y, 1, 0.5f);
            }
            else
            {
                cam.rect = new Rect(cam.rect.x, 0, 1, 1);
            }
        }
        else
        {
            controller.gameObject.SetActive(false);
            cam.gameObject.SetActive(false);
        }

        //Draw Fps
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        if (playerManager.playerNumber == 1) transform.Find("FPSText").GetComponent<Text>().text = Mathf.Ceil(fps).ToString();

        //Draw Player Count
        transform.Find("PlayerText").GetComponent<Text>().text = playerManager.playerNumber.ToString();

        //Right Buttons Inputs
        if (playerManager.playerNumber == 1)
        {
            if (Input.GetButtonDown("Right Button 1"))
            {
                FindObjectsOfType<FormManager>()[0].ToggleForm(0);
            }
            if (Input.GetButtonDown("Right Button 2"))
            {
                FindObjectsOfType<FormManager>()[0].ToggleForm(1);
            }
            if (Input.GetButtonDown("Right Button 3"))
            {
                FindObjectsOfType<FormManager>()[0].ToggleForm(2);
            }
            if (Input.GetButtonDown("Right Button 4"))
            {
                FindObjectsOfType<FormManager>()[0].ToggleForm(3);
            }
        }
        if (playerManager.playerNumber == 2)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                FindObjectsOfType<FormManager>()[1].ToggleForm(0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                FindObjectsOfType<FormManager>()[1].ToggleForm(1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                FindObjectsOfType<FormManager>()[1].ToggleForm(2);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                FindObjectsOfType<FormManager>()[1].ToggleForm(3);
            }
        }
    }
}
