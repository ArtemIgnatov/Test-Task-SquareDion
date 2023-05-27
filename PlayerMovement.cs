using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform[] _waypoints; // ������ ����� �����������
    [SerializeField] private float _speed = 2f; // �������� �����������
    [SerializeField] private float _rotationSpeed = 2f; // �������� ��������
    private int _currentWaypoint = 0; // ������ ������� ����� �����������

    private Rigidbody _rb; // ������ �� ��������� Rigidbody

    private bool _canRun = false;

    private void OnEnable()
    {
        WayPointController.OnMove += HandleMove;
        GameManager.OnStart += HandleMove;
    }

    private void OnDisable()
    {
        WayPointController.OnMove -= HandleMove;
        GameManager.OnStart -= HandleMove;
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.isKinematic = true;

        if (_waypoints.Length == 0)
        {
            Debug.LogError("������ ����� ����������� ����!");
            return;
        }

        // ���������� ������ � ������ �����
        transform.position = _waypoints[_currentWaypoint].position;
    }

    private void Update()
    {
        // ���������, �������� �� ������� �����
        if ((transform.position - _waypoints[_currentWaypoint].position).magnitude < 0.5 )
        {
            // ��������� � ��������� �����
            _currentWaypoint++;

            // ���������, �������� �� ����� ������� �����
            if (_currentWaypoint >= _waypoints.Length)
            {
                // ��������� �����������
                _rb.isKinematic = true;
                enabled = false;  // ��������� ������
                return;
            }
        }
    }

    private void FixedUpdate()
    {
        if (_canRun)
        {
            // ��������� ����������� �������� � ������� �����
            Vector3 direction = _waypoints[_currentWaypoint].position - transform.position;
            direction.Normalize();

            // ���������� ������ � ����������� ������� ����� � ������ ��������
            _rb.velocity = direction * _speed;

            // ������������ ������ ���, ����� �� ������ ������� �� ������� ����
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // ������ ���������� ������� ������� � �������� ��������
            _rb.MoveRotation(Quaternion.Lerp(_rb.rotation, targetRotation, _rotationSpeed * Time.deltaTime));
        }
        else
        {
            // ������������� ��������
            _rb.velocity = Vector3.zero;
        }
    }

    void HandleMove(bool canMove, bool isKinematic)
    {
        _canRun = canMove;
        _rb.isKinematic = isKinematic;
    }
}
