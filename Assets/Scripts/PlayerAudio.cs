using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource sfxSource;  //ссылка на группу в микшере(чтобы регулировать громкость)

    [Header("Footstep Clips")]
    public AudioClip walkStep;  //клип для ходьбы
    public AudioClip runStep;  //для бега
    public AudioClip crawlStep;  //присевшим

    [Header("Action Clips")]
    public AudioClip jumpClip;  //для прыжка
    public AudioClip hurtClip;  //урона
    public AudioClip deathClip;  //смерти

    private PlayerController player; //ссылка на скрипт

    private void Awake()
    {
        player = GetComponent<PlayerController>();  //ищем скрипт 
    }

    // Вызывается анимацией
    public void PlayWalkStep()
    {
        if (!player.isCrawl && !player.isRun)  //если игрок не бежит и не ползет (звук ходьбы)
            PlaySFX(walkStep);
    }

    public void PlayRunStep()
    {
        if (player.isRun && !player.isCrawl) //бежи и не ползет
            PlaySFX(runStep);
    }

    public void PlayCrawlStep()
    {
        if (player.isCrawl)  //если ползет
            PlaySFX(crawlStep);
    }

    public void PlayJump()
    {
        PlaySFX(jumpClip);  //на прыжок
    }

    public void PlayHurt()
    {
        PlaySFX(hurtClip);  //на получения урона
    }

    public void PlayDeath()
    {
        PlaySFX(deathClip);  //звук смерти
    }

    private void PlaySFX(AudioClip clip)
    {
        if (clip != null)  //проверка
            AudioManager.Instance.PlaySFX(clip);  //передает клип для регулировки громкости
    }
}
