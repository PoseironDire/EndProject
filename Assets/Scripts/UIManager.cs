using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    FormManager formManager;
    Text fpsText;

    void Awake()
    {
        formManager = GameObject.Find("Right_Buttons_Outline").GetComponent<FormManager>(); //Find Form Manager
        fpsText = GameObject.Find("FPSText").GetComponent<Text>(); //Find FPS Text
    }

    float deltaTime;
    void Update()
    {
        //Draw Fps
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsText.text = Mathf.Ceil(fps).ToString();

        //Right Buttons Inputs
        if (Input.GetButtonDown("Right Button 1") || Input.GetKeyDown(KeyCode.Alpha1))
            formManager.ToggleForm(0);
        if (Input.GetButtonDown("Right Button 2") || Input.GetKeyDown(KeyCode.Alpha4))
            formManager.ToggleForm(1);
        if (Input.GetButtonDown("Right Button 3") || Input.GetKeyDown(KeyCode.Alpha3))
            formManager.ToggleForm(2);
        if (Input.GetButtonDown("Right Button 4") || Input.GetKeyDown(KeyCode.Alpha2))
            formManager.ToggleForm(3);
    }
}
