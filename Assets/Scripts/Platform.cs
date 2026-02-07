using UnityEngine;

public class Platform : MonoBehaviour
{
   
    public Transform pointA;  //точки откуда куда двигается
    public Transform pointB;
    public float speed = 2f;  //скорость движения

    private Vector3 target;  //текущая цель движения(А или В)

    private Vector3 lastPos;  //предыдущая позиция для расчета скорости
    public Vector2 PlatformVelocity { get; private set; }  //текущая скорость (единица/сек)  можно читать/нельзя изменять извне

    private void Start()
    {
        target = pointB.position; //начинает двигаться к В
        lastPos = transform.position; //для вычисления скорости
    }

    private void FixedUpdate()
    {
        // движение платформы
        transform.position = Vector3.MoveTowards(
            transform.position,             //текущая позиция
            target,                         //куда двигается
            speed * Time.fixedDeltaTime    //расстояние за шаг(кадр)
        );

        // меняем цель
        if (Vector3.Distance(transform.position, target) < 0.05f)   //если близко к точке
        {
            // проверяем, куда пришли
            bool nearA = Vector3.Distance(target, pointA.position) < 0.1f; //true если близок к А
            target = nearA ? pointB.position : pointA.position;  //меняет от bool направление
        }

        // вычисление скорости
        PlatformVelocity = (transform.position - lastPos) / Time.fixedDeltaTime;  //назначает вектор перемещения 
        lastPos = transform.position;  //обновляет текущую позицию 
    }

}

