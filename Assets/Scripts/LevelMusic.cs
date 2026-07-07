using UnityEngine;

public class LevelMusic : MonoBehaviour
{
    [SerializeField] private AudioClip _levelMusic;  //ссылка на фоновую музыку

    private void Start()
    {
        if (_levelMusic != null && AudioManager.Instance != null)  //если клип есть и микшер тоже
        {
            AudioManager.Instance.PlayMusic(_levelMusic, true);  //отправляет в микшер(где регулирует звук)
        }
    }
}
