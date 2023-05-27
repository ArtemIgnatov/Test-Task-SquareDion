using System;
using System.Collections;
using UnityEngine;

public class ShootingController : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;       // Префаб пули
    [SerializeField] private float _bulletSpeed = 10f;       // Скорость пули
    [SerializeField] private Transform _firePoint;
    [SerializeField] private GameObject _gun;
    public static event Action<bool> OnShoot;
    private bool _canShoot = false;

    private void OnEnable()
    {
        WayPointController.OnReadyToShoot += HandleShoot;
    }

    private void OnDisable()
    {
        WayPointController.OnReadyToShoot -= HandleShoot;
    }

    private void Start()
    {
        _gun.gameObject.SetActive(_canShoot);
    }

    private void Update()
    {
        // Проверяем нажатие на экран
        if (_canShoot && Input.GetMouseButtonDown(0))
        {
            StartCoroutine(Shooting());
        }
    }

    private IEnumerator Shooting()
    {
        OnShoot?.Invoke(true);

        yield return new WaitForSeconds(0.3f);

        // Создаем луч из позиции камеры в направлении нажатия
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Переменная для хранения информации о столкновении с объектом
        RaycastHit hit;

        // Пускаем луч и проверяем, есть ли столкновение с объектом
        if (Physics.Raycast(ray, out hit))
        {
            // Получаем позицию столкновения
            Vector3 hitPosition = hit.point;

            // Получаем направление к точке попадания
            Vector3 direction = hitPosition - _firePoint.position;

            // Создаем экземпляр пули из префаба
            GameObject bullet = Instantiate(_bulletPrefab, _firePoint.position, Quaternion.identity);

            // Получаем компонент Rigidbody пули
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

            // Задаем скорость пули в направлении нажатия
            bulletRb.velocity = direction.normalized * _bulletSpeed;

            bullet.transform.rotation = Quaternion.LookRotation(direction);
        }
        OnShoot?.Invoke(false);
    }

    void HandleShoot(bool canShoot)
    {
        _gun.gameObject.SetActive(canShoot);
        _canShoot = canShoot;
    }
}


