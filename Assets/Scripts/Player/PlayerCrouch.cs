using UnityEngine;

[RequireComponent (typeof(CapsuleCollider2D))]
public class PlayerCrouch : MonoBehaviour
{  //Для управления приседанием игрока

    [Header("Celing Check")]
    [SerializeField] private Transform _ceilingCheck;  //Точка из которой производится проверка на припятствие
    [SerializeField] private float _ceilingRadius = 0.2f;  //Радиус круга для проверки пересечения 
    [SerializeField] private LayerMask _groundLayer;  //Слой который считается землей

    [Header("Collider")]
    [SerializeField] private float _hightMultiplier = 0.7f; //Множитель высоты колайдера при приседании

    private CapsuleCollider2D _collider;
    private Vector2 _originalSize;  //Исходны размер коллайдера
    private Vector2 _originalOffset;  //Исходное смещение коллайдера
    public bool IsCrouching { get; private set; } //Флаг в приседе или нет
    private PlayerInput _input;
    private void Awake()
    {
        _collider = GetComponent<CapsuleCollider2D>();
        _input = GetComponent<PlayerInput>();

        //сохраняем исходные данные 
        _originalSize = _collider.size;
        _originalOffset = _collider.offset;
    }
    private void Update()
    {
        //Проверка есть ли припятствие над головой
        bool ceilingBlocked = Physics2D.OverlapCircle(_ceilingCheck.position, _ceilingRadius,_groundLayer);

        //Условия при котором игрок приседает
        IsCrouching = _input.CrouchHeld || ceilingBlocked;

        //Обновляет параметры коллайдера от  текущего состояния 
        UpdateCollider();
    }

    //Изменяет размеры\смещение коллайдера
    private void UpdateCollider()
    {
        if (IsCrouching)
        {
            //Приседаем уменьшая высоту 
            _collider.size = new Vector2(_originalSize.x, _originalSize.y * _hightMultiplier);

            //Изменение смещения 
            _collider.offset = new Vector2(_originalOffset.x, _originalOffset.y - (_originalSize.y - _collider.size.y) / 2f);
        }
        else
        {
            //встает (востановление исходных данных
            _collider.size = _originalSize;
            _collider.offset = _originalOffset;
        }
        
    }
}
