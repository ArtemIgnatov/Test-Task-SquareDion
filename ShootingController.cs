using System;
using System.Collections;
using UnityEngine;

public class ShootingController : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;       // ������ ����
    [SerializeField] private float _bulletSpeed = 10f;       // �������� ����
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
        // ��������� ������� �� �����
        if (_canShoot && Input.GetMouseButtonDown(0))
        {
            StartCoroutine(Shooting());
        }
    }

    private IEnumerator Shooting()
    {
        OnShoot?.Invoke(true);

        yield return new WaitForSeconds(0.3f);

        // ������� ��� �� ������� ������ � ����������� �������
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // ���������� ��� �������� ���������� � ������������ � ��������
        RaycastHit hit;

        // ������� ��� � ���������, ���� �� ������������ � ��������
        if (Physics.Raycast(ray, out hit))
        {
            // �������� ������� ������������
            Vector3 hitPosition = hit.point;

            // �������� ����������� � ����� ���������
            Vector3 direction = hitPosition - _firePoint.position;

            // ������� ��������� ���� �� �������
            GameObject bullet = Instantiate(_bulletPrefab, _firePoint.position, Quaternion.identity);

            // �������� ��������� Rigidbody ����
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

            // ������ �������� ���� � ����������� �������
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


