using UnityEngine;

public class HealthHost : MonoBehaviour
{
    [SerializeField] GameObject damageText;
    float shift = 1;

    void FixedUpdate()
    {
        shift = Mathf.Lerp(shift, 1, 0.1f);
        Color newColor = new Color(1f, shift, shift, 1f);
        GetComponent<SpriteRenderer>().color = newColor;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            shift = 0;
            DamageIndicator indicator = Instantiate(damageText, transform.position, Quaternion.identity).GetComponent<DamageIndicator>();
            indicator.SetDamageText(Random.Range(1, 100));
        }
    }
}
