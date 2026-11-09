namespace YG
{
    public partial class SavesYG
    {
        public int lastUnlockedLevelBuildIndex = 1;
        public int[] starsByLevelBuildIndex = new int[16];
        public int winCount;
        public bool reviewPromptShown;
        public bool reviewCompleted;

    }
}