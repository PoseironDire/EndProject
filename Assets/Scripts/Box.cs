using UnityEngine;

public class Box : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    float shift = 1;

    public GameObject damageText;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        shift = Mathf.Lerp(shift, 1, 0.1f);
        Color thisColor = new Color(1f, shift, shift, 1f);
        spriteRenderer.color = thisColor;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            shift = 0;
            DamageIndicator indicator = Instantiate(damageText, transform.position, Quaternion.identity).GetComponent<DamageIndicator>();
            indicator.SetDamageText(Random.Range(1, 100));
        }
    }
}
