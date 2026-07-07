using UnityEngine;

public class UIButtonSound : MonoBehaviour
{
    [SerializeField] private AudioClip _clip; // назначаем в инспекторе короткий звук клика

   
    public void Play()
    {
        if (_clip == null)
            return; // ничего не делаем если не назначен

        if (AudioManager.Instance != null) //считывает код
            AudioManager.Instance.PlaySFX(_clip);    //воспроизводит звук(который регулируется через микшер)
    
    }

}
