using UnityEngine;

public class LevelMusic : MonoBehaviour
{
    public AudioClip levelMusic;  //ссылка на фоновую музыку

    private void Start()
    {
        if (levelMusic != null && AudioManager.Instance != null)  //если клип есть и микшер тоже
        {
            AudioManager.Instance.PlayMusic(levelMusic, true);  //отправляет в микшер(где регулирует звук)
        }
    }
}
