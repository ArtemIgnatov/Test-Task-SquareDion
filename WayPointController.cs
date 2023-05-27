using System;
using System.Collections.Generic;
using UnityEngine;

public class WayPointController : MonoBehaviour
{
    [SerializeField] private List<Enemy> _enemies;
    public static event Action<bool, bool> OnMove;
    public static event Action<bool> OnReadyToShoot;

    private void OnEnable()
    {
        Enemy.OnEnemyKilled += HandleEnemyDefeated;
    }

    private void OnDisable()
    {
        Enemy.OnEnemyKilled -= HandleEnemyDefeated;
    }

    /// <summary>
    /// ћетод удал€ющий врагов из списка
    /// </summary>
    /// <param name="enemy"></param>
    void HandleEnemyDefeated(Enemy enemy)
    {
        _enemies.Remove(enemy);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnReadyToShoot?.Invoke(true);
            OnMove?.Invoke(false,true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_enemies.Count == 0)
            {
                OnReadyToShoot?.Invoke(false);
                OnMove?.Invoke(true, false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnReadyToShoot?.Invoke(false);
            OnMove?.Invoke(true, false);
        }
    }
}
