using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _damage = 0f;

    private void Update()
    {
        Destroy(gameObject, 3);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (collision.TryGetComponent(out IDamageable hit))
            {
                hit.TakeDamage(_damage);
                Destroy(gameObject);
            }
        }
    }
}