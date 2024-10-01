using UnityEngine;

public interface IDamageable
{
    void TakeDamage(long damage, DamageType damageTypeValue);

    void FloatingDamage(Vector3 position, long damage, DamageType damageTypeValue);
}
