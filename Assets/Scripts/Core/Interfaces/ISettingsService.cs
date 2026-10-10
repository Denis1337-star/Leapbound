public interface ISettingsService 
{
    float MusicVolume { get; }
    float SfxVolume { get; }
    void SetMusicVolume(float value);
    void SetSfxVolume(float value);

}
