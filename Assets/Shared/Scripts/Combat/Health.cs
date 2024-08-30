using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    public int currentHealth { get; private set; }

    public event Action OnTakeDamage;
    public event Action OnDie;

    public bool IsDead => currentHealth == 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DealDamage(int damage)
    {
        if (currentHealth <= 0) { return; }

        currentHealth -= damage;

        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        OnTakeDamage?.Invoke();

        if (currentHealth == 0)
        {
            OnDie?.Invoke();
        }

        Debug.Log(currentHealth);

        //the above from currentHealth-= damage can be written as:
        //currentHealth = Mathf.Max(currentHealth - damage, 0);
    }
}
