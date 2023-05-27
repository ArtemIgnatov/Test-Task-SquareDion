using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform[] _waypoints; // Массив точек перемещения
    [SerializeField] private float _speed = 2f; // Скорость перемещения
    [SerializeField] private float _rotationSpeed = 2f; // Скорость поворота
    private int _currentWaypoint = 0; // Индекс текущей точки перемещения

    private Rigidbody _rb; // Ссылка на компонент Rigidbody

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
            Debug.LogError("Массив точек перемещения пуст!");
            return;
        }

        // Перемещаем объект к первой точке
        transform.position = _waypoints[_currentWaypoint].position;
    }

    private void Update()
    {
        // Проверяем, достигли ли текущей точки
        if ((transform.position - _waypoints[_currentWaypoint].position).magnitude < 0.5 )
        {
            // Переходим к следующей точке
            _currentWaypoint++;

            // Проверяем, достигли ли конца массива точек
            if (_currentWaypoint >= _waypoints.Length)
            {
                // Завершаем перемещение
                _rb.isKinematic = true;
                enabled = false;  // Отключаем скрипт
                return;
            }
        }
    }

    private void FixedUpdate()
    {
        if (_canRun)
        {
            // Вычисляем направление движения к текущей точке
            Vector3 direction = _waypoints[_currentWaypoint].position - transform.position;
            direction.Normalize();

            // Перемещаем объект в направлении текущей точки с учетом скорости
            _rb.velocity = direction * _speed;

            // Поворачиваем объект так, чтобы он всегда смотрел на текущую цель
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Плавно приближаем текущий поворот к целевому повороту
            _rb.MoveRotation(Quaternion.Lerp(_rb.rotation, targetRotation, _rotationSpeed * Time.deltaTime));
        }
        else
        {
            // Останавливаем движение
            _rb.velocity = Vector3.zero;
        }
    }

    void HandleMove(bool canMove, bool isKinematic)
    {
        _canRun = canMove;
        _rb.isKinematic = isKinematic;
    }
}
