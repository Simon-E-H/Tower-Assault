using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform target;

    public float speed = 20f;

    public GameObject impactEffect;
    public void Seek(Transform _target)
    {
        target = _target;
    } 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 direction = target.position - transform.position;
        float distanceToMove = speed * Time.deltaTime;

        if (direction.magnitude <=  distanceToMove)
        {
            HitTarget();
            return;
        }

        transform.Translate(direction.normalized * distanceToMove, Space.World);


    }

    void HitTarget ()
    {
        GameObject effect = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(effect, 2f);
        
        Destroy(gameObject);

    }
}
