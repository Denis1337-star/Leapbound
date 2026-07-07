using UnityEngine;

public class Platform : MonoBehaviour
{

    [SerializeField] private Transform _pointA;  //точки откуда куда двигается
    [SerializeField] private Transform _pointB;
    [SerializeField] private float _speed = 2f;  //скорость движения

    private Vector3 _target;  //текущая цель движения(А или В)
    private Vector3 _lastPos;  //предыдущая позиция для расчета скорости
    public Vector2 PlatformVelocity { get; private set; }  //текущая скорость (единица/сек)  можно читать/нельзя изменять извне

    private void Start()
    {
        _target = _pointB.position; //начинает двигаться к В
        _lastPos = transform.position; //для вычисления скорости
    }

    private void FixedUpdate()
    {
        // движение платформы
        transform.position = Vector3.MoveTowards(
            transform.position,             //текущая позиция
            _target,                         //куда двигается
            _speed * Time.fixedDeltaTime    //расстояние за шаг(кадр)
        );

        // меняем цель
        if (Vector3.Distance(transform.position, _target) < 0.05f)   //если близко к точке
        {
            // проверяем, куда пришли
            bool nearA = Vector3.Distance(_target, _pointA.position) < 0.1f; //true если близок к А
            _target = nearA ? _pointB.position : _pointA.position;  //меняет от bool направление
        }

        // вычисление скорости
        PlatformVelocity = (transform.position - _lastPos) / Time.fixedDeltaTime;  //назначает вектор перемещения 
        _lastPos = transform.position;  //обновляет текущую позицию 
    }
}

