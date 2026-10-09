using UnityEngine;

public class Trampoline : MonoBehaviour
{
    [Header("Trampoline Settings")]
    [SerializeField] private float _launchForce = 14f;     
    [SerializeField] private float _delayBeforeJump = 1f;  

    private Animator _animator;  
    private bool _playerOnPlatform = false; 
    private float _timer = 0f;  

    private void Awake()
    {
        _animator = GetComponent<Animator>(); 
    }

    private void Update()
    {
        if (_playerOnPlatform)  
        {
            _timer += Time.deltaTime;  

            if (_timer >= _delayBeforeJump)
            {
                LaunchPlayer();
                _timer = 0f;
                _playerOnPlatform = false;
            }
        }
    }

    private void LaunchPlayer()
    {
        _animator.SetTrigger("Jump"); 

        Collider2D[] cols = Physics2D.OverlapBoxAll(transform.position, new Vector2(1.2f, 0.5f), 0); 

        foreach (var col in cols) 
        {
            if (col.CompareTag("Player"))   
            {
                Rigidbody2D rb = col.GetComponent<Rigidbody2D>();
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, _launchForce);  
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            _playerOnPlatform = true;
            _timer = 0f;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            _playerOnPlatform = false;
            _timer = 0f;
        }
    }
}
