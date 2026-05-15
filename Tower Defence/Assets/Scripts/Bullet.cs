using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public float damage = 20f;

    [Header("AoE Settings")]
    public float splashRadius = 0f;
    public float splashDamage = 0f;

    private Transform target;

    public void Seek(Transform _target)
    {
        target = _target;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject, 2f);
            return;
        }

        Vector3 direction = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        transform.Translate(direction.normalized * distanceThisFrame, Space.World);

        if (direction.magnitude <= distanceThisFrame + 0.2f)
        {
            HitTarget();
        }
    }

    void HitTarget()
    {
        Enemy enemy = target.GetComponent<Enemy>();
        if (enemy != null)
            enemy.TakeDamage(damage);

        if (splashRadius > 0)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, splashRadius);
            foreach (Collider col in hitColliders)
            {
                Enemy e = col.GetComponent<Enemy>();
                if (e != null && e != enemy)
                {
                    e.TakeDamage(splashDamage);
                }
            }
        }

        Destroy(gameObject);
    }
}