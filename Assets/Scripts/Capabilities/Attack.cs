using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{

    [SerializeField]
    private InputController input = null;
    [SerializeField]
    private Transform attackPoint;
    [SerializeField, Range(0.1f, 4f)]
    private float attackRange = 0.5f;
    [SerializeField, Range(50, 200)]
    private int weaponDamage = 50;
    public LayerMask enemyLayers;

    [SerializeField, Range(0.1f, 5f)]
    private float attackRate = 2f;
    private float nextAttackTime = 0f;

    void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            if (input.RetrieveAttackInput() && enabled)
            {
                doAttack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
        
    }

    void doAttack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Health>().TakeDamage(weaponDamage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
}
