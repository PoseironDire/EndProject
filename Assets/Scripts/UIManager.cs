using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Text fpsText;
    [SerializeField] Text debugText;
    [SerializeField] float deltaTime;

    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsText.text = Mathf.Ceil(fps).ToString();

        debugText.text = (Mathf.Max(Mathf.Abs(Input.GetAxis("RX")), Mathf.Abs(Input.GetAxis("RY"))) * 10).ToString();
    }
}
