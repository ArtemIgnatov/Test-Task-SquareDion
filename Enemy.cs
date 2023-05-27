using System;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private Healthbar _healthbar;
    [SerializeField] private float _maxHealth = 100f;
    public static event Action<Enemy> OnEnemyKilled;
    private float _currentHealth;
    private Animator _animator;

    void Start()
    {
        _currentHealth = _maxHealth;
        _healthbar.UpdateHealthBar(_maxHealth, _currentHealth);
        _animator = GetComponent<Animator>();
    }

    public void TakeDamage(float damageAmount)
    {
        _currentHealth -= damageAmount;
        _healthbar.UpdateHealthBar(_maxHealth, _currentHealth);

        if (_currentHealth <= 0)
        {
            _animator.SetTrigger("Death");
            OnEnemyKilled?.Invoke(this);
            Destroy(gameObject,4);
        }
    }
}
