using UnityEngine;

[CreateAssetMenu(fileName = "LevelTimeCatalog", menuName = "Leapbound/Level Time Catalog")]
public sealed class LevelTimeCatalog : ScriptableObject
{
    [SerializeField] private float[] _targetTimeByBuildIndex;

    public int LevelCount => _targetTimeByBuildIndex.Length;
    public float GetTargetTimeSeconds(int levelBuildIndex)
    {
        if (_targetTimeByBuildIndex == null ||
            levelBuildIndex < 0 ||
            levelBuildIndex >= _targetTimeByBuildIndex.Length)
            return 60f;

        return _targetTimeByBuildIndex[levelBuildIndex];
    }
}