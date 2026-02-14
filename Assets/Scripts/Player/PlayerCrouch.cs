using UnityEngine;

[RequireComponent (typeof(CapsuleCollider2D))]
public class PlayerCrouch : MonoBehaviour
{  //ƒл€ управлени€ приседанием игрока

    [Header("Celing Check")]
    public Transform ceilingCheck;  //“очка из которой производитс€ проверка на прип€тствие
    public float ceilingRadius = 0.2f;  //–адиус круга дл€ проверки пересечени€ 
    public LayerMask groundLayer;  //—лой который считаетс€ землей

    [Header("Collider")]
    public float hightMultiplier = 0.7f; //ћножитель высоты колайдера при приседании

    private CapsuleCollider2D col;
    private Vector2 originalSize;  //»сходны размер коллайдера
    private Vector2 originalOffset;  //»сходное смещение коллайдера
    public bool IsCrouching { get; private set; } //‘лаг в приседе или нет
    private PlayerInput input;
    private void Awake()
    {
        col = GetComponent<CapsuleCollider2D>();
        input = GetComponent<PlayerInput>();

        //сохран€ем исходные данные 
        originalSize = col.size;
        originalOffset = col.offset;
    }
    private void Update()
    {
        //ѕроверка есть ли прип€тствие над головой
        bool ceilingBlocked = Physics2D.OverlapCircle(ceilingCheck.position, ceilingRadius,groundLayer);

        //”слови€ при котором игрок приседает
        IsCrouching = input.CrouchHeld || ceilingBlocked;

        //ќбновл€ет параметры коллайдера от  текущего состо€ни€ 
        UpdateCollider();
    }

    //»змен€ет размеры\смещение коллайдера
    private void UpdateCollider()
    {
        if (IsCrouching)
        {
            //ѕриседаем уменьша€ высоту 
            col.size = new Vector2(originalSize.x, originalSize.y * hightMultiplier);

            //»зменение смещени€ 
            col.offset = new Vector2(originalOffset.x, originalOffset.y - (originalSize.y - col.size.y) / 2f);
        }
        else
        {
            //встает (востановление исходных данных
            col.size = originalSize;
            col.offset = originalOffset;
        }
        
    }
}
