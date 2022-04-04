using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour
{
    public Text normalText;
    public Text boldText;
    public float lifetime = 0.6f;
    public float minDist = 2f;
    public float maxDist = 3f;

    private Vector3 iniPos;
    private Vector3 targetPos;
    private float timer;

    float normalShift;
    float boldShift;
    float damaged;
    Vector3 value;

    void Awake()
    {
        GetComponent<AudioSource>().pitch = GetComponent<AudioSource>().pitch * UnityEngine.Random.Range(0.9f, 1.1f);
    }

    void Start()
    {
        normalShift = 0;
        boldShift = 1;
        GetComponent<AudioSource>().pitch = 2 * UnityEngine.Random.Range(0.8f, 1.2f);
        GetComponent<AudioSource>().Play();
        float direction = Random.rotation.eulerAngles.z;
        iniPos = transform.position;
        float dist = Random.Range(minDist, maxDist);
        targetPos = iniPos + (Quaternion.Euler(0, 0, direction) * new Vector3(dist, dist, 0f));
        transform.localScale = Vector3.zero;
        value = new Vector3(damaged / 100, damaged / 100, damaged / 100);
    }

    void Update()
    {
        timer += Time.deltaTime;

        float fraction = lifetime / 2f;

        Destroy(gameObject, lifetime);

        transform.position = Vector3.Lerp(iniPos, targetPos, Mathf.Sin(timer / lifetime));
        transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one + value, Mathf.Sin(timer / lifetime));
    }

    void FixedUpdate()
    {
        normalShift = Mathf.Lerp(normalShift, 1, 0.1f);
        Color normalColor = new Color(1f, normalShift, 0, 1f);
        normalText.color = normalColor;

        boldShift = Mathf.Lerp(boldShift, 0, 0.1f);
        Color boldColor = new Color(1f, boldShift, 0, 1f);
        boldText.color = boldColor;
    }

    public void SetDamageText(int damage)
    {
        normalText.text = damage.ToString();
        boldText.text = damage.ToString();
        damaged = damage;
    }
}