using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 8f;
    [SerializeField] private float lifeTime = 5f;

    private int damage;
    private Transform target;

    private bool flyStraight;

    public void SetTarget(Transform t, int dmg)
    {
        target = t;
        damage = dmg;

        flyStraight = (target == null);

        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        if (flyStraight)
        {
            transform.position += transform.forward * speed * Time.deltaTime;
            return;
        }

        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = (target.position - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!flyStraight && other.transform == target)
        {
            IDamageable dmg = other.GetComponent<IDamageable>();
            if (dmg != null)
                dmg.TakeDamage(damage);

            Destroy(gameObject);
        }
    }
}