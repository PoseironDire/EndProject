using UnityEngine;

public class HitEffect : MonoBehaviour
{
    [SerializeField] float lifeTime = 0.1f;

    void Start()
    {
        Destroy(this.gameObject, lifeTime);
    }
}
