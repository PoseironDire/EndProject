using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    FormManager formManager;
    Text fpsText;
    float deltaTime;

    void Awake()
    {
        formManager = GameObject.Find("Right_Buttons_Outline").GetComponent<FormManager>();
        fpsText = GameObject.Find("FPSText").GetComponent<Text>();
    }

    void Update()
    {
        //Right Buttons Input
        if (Input.GetButtonDown("Right Button 1"))
            formManager.ToggleForm(0);
        if (Input.GetButtonDown("Right Button 2"))
            formManager.ToggleForm(1);
        if (Input.GetButtonDown("Right Button 3"))
            formManager.ToggleForm(2);
        if (Input.GetButtonDown("Right Button 4"))
            formManager.ToggleForm(3);

        //Draw Fps
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsText.text = Mathf.Ceil(fps).ToString();
    }
}
