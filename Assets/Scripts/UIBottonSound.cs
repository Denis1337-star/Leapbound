using UnityEngine;
using UnityEngine.UI;

public class UIBottonSound : MonoBehaviour
{
    public AudioClip clip; // назначаем в инспекторе короткий звук клика

   
    public void Play()
    {
        if (clip == null)
            return; // ничего не делаем если не назначен

        if (AudioManager.Instance != null) //считывает код
            AudioManager.Instance.PlaySFX(clip);    //воспроизводит звук(который регулируется через микшер)
    
    }

}
