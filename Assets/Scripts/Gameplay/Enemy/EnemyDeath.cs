using System.Collections;
using UnityEngine;


public class EnemyDeath : MonoBehaviour
{
    [SerializeField] private float fadeSpeed = 1f; //Скорость затухания

    private SpriteRenderer _sprite;
    private Collider2D _collider;
    private EnemyBase _enemyBase;
    private void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
        _enemyBase = GetComponent<EnemyBase>();
    }

    private void OnEnable()
    {
        _enemyBase.OnDeath += Die;
    }

    private void OnDisable()
    {
        _enemyBase.OnDeath -= Die;
    }
    private void Die()
    {
        _collider.enabled = false;
        //Останавливет AI врагов
        MonoBehaviour[] behaviours = GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour behaviour in behaviours)
        {
            if(behaviour ==this)
                continue;

            behaviour.enabled = false;
        }
        
        Animator animator = GetComponent<Animator>();
        if(animator != null)
            animator.enabled = false;

        StartCoroutine(FadeOut());  
    }
    private IEnumerator FadeOut()
    {
        float alpha = 1f;  //исходные данные
        while (alpha > 0)
        {
            alpha -= Time.deltaTime * fadeSpeed; //уменьшаем прозрачность

            _sprite.color = new Color(1, 1, 1, alpha);
            yield return null;
        }
        Destroy(gameObject);
    }
}
