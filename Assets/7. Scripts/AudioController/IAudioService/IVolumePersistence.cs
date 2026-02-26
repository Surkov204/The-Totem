public interface IVolumePersistence
{
    float LoadMusicVolume();
    float LoadSfxVolume();

    void SaveMusicVolume(float value);
    void SaveSfxVolume(float value);
}