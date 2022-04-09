using UnityEngine;
using System.Collections.Generic;

public class EnergyHost : MonoBehaviour
{
    [SerializeField] GameObject damageText;
    Dictionary<SpriteRenderer, Color> originalColor = new Dictionary<SpriteRenderer, Color>();

    public GameObject projectilePrefab; //Projectile
    public Transform projectileSpawnPoint; //Projectile Spawn Point
    [Range(0, 300)] public float initialProjectileVelocity; //Projectile Speed
    [Range(0, 50)] public float initialProjectileSpread; //Projectile Spread
    [Range(0, 1)] public float projectileLifeTime; //Projectile Life Time
    [Range(0, 5)] public float timeBetweenShots; //Fires Per Second
    [Range(1, 10)] public int projectileBurst; //Fires Per Second
    [Range(0, 1)] public float projectileVolume = 0.5f; //Volume
    [Range(0, 5)] public float minProjectilePitch = 1; //Minimum Pitch
    [Range(0, 5)] public float maxProjectilePitch = 2.5f; //Maximum Pitch

    void Start()
    {
        if (gameObject.tag != "Projectile" && gameObject.tag != "Player") GetComponent<SpriteRenderer>().color = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), 1);

        SpriteRenderer[] children = transform.GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer renderer in children)
        {
            originalColor.Add(renderer, renderer.color);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            foreach (SpriteRenderer renderer in originalColor.Keys)
            {
                if (renderer.color != Color.red)
                    renderer.color = Color.red;
                else
                    renderer.color = Color.white;
            }
            DamageIndicator indicator = Instantiate(damageText, transform.position, Quaternion.identity).GetComponent<DamageIndicator>();
            indicator.transform.SetParent(transform.parent); //Make The Indicator A Child Of This Game Object 
            indicator.SetDamageText(Random.Range(1, 100));
        }
    }

    public void LerpColor()
    {
        foreach (SpriteRenderer renderer in originalColor.Keys)
        {
            renderer.color = Color.Lerp(renderer.color, originalColor[renderer], 0.1f);
        }
    }
}