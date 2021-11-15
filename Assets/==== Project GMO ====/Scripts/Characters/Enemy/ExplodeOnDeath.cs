using UnityEngine;

public class ExplodeOnDeath : DeathEffect
{
    [SerializeField] private Damage damage;
    [SerializeField] private float damageRadius;
    [SerializeField] private GameObject explosionFX;

    protected override void Effect(Enemy enemy)
    {
        base.Effect(enemy);

        Explosion explode = new Explosion(enemy.transform.position, damage, damageRadius, explosionFX);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }
}

public class Explosion
{
    public Explosion(Vector3 explodePosition, Damage damage, float damageRadius, GameObject explosionFX)
    {
        GameObject effectInstance = GameObject.Instantiate(explosionFX, explodePosition, Quaternion.identity);
        GameObject.Destroy(effectInstance, 1f);
        ExplodeDamage(explodePosition, damage, damageRadius);
    }

    private void ExplodeDamage(Vector3 explodePosition, Damage damage, float damageRadius)
    {
        Collider[] hitsInRange = Physics.OverlapSphere(explodePosition, damageRadius);

        for (int i = 0; i < hitsInRange.Length; i++)
        {
            //if (hitsInRange[i].GetComponentInParent<Enemy>() == enemy)
            //{
            //    continue;
            //}

            ICanBeDamage damageable = hitsInRange[i].GetComponentInParent<ICanBeDamage>();
            if (damageable != null)
            {
                damageable.ReceiveDamage(damage);
            }
        }
    }

}
