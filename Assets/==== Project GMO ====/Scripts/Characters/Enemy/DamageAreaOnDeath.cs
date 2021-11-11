using UnityEngine;

public class DamageAreaOnDeath : DeathEffect
{
    [SerializeField] private Damage damage;
    [SerializeField] private float damageRadius;

    protected override void Effect(Enemy enemy)
    {
        enemy.OnDeath -= Effect;

        Collider[] hitsInRange = Physics.OverlapSphere(enemy.transform.position, damageRadius);

        for (int i = 0; i < hitsInRange.Length; i++)
        {
            if (hitsInRange[i].GetComponentInParent<Enemy>() == enemy)
            {
                continue;
            }

            ICanBeDamage damageable = hitsInRange[i].GetComponentInParent<ICanBeDamage>();
            if (damageable != null)
            {
                damageable.ReceiveDamage(damage);
            }
        }
    }
}
