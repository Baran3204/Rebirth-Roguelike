using UnityEngine;
using UnityEngine.Rendering;

public class Bullet : MonoBehaviour
{

    [SerializeField] private float _bulletDamage;
    void OnTriggerEnter2D(Collider2D collision)
    {
         if(collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.TryGetComponent<IDamagables>(out IDamagables component);
            component.Damage(_bulletDamage);
            Destroy(gameObject);
        }
    }
}
