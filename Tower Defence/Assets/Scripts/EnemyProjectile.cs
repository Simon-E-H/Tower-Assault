using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float speed = 12f;
    public float damage = 10f;

    private Transform target;

    public void Seek(Transform _target)
    {
        target = _target;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 direction = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (direction.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(direction.normalized * distanceThisFrame, Space.World);
    }

    void HitTarget()
    {
        Turret turret = target.GetComponent<Turret>();
        if (turret != null)
        {
            turret.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}