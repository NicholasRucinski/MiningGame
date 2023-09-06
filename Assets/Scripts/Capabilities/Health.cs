using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField, Range(1, 200)]
    public int maxHealth = 100;
    float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            death();
        }
    }

    void death()
    {
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        //Temporary \/
        Destroy(this.gameObject);
    }
}
