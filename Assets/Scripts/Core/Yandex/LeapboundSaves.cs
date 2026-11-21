namespace YG
{
    public partial class SavesYG
    {
        public int lastUnlockedLevelBuildIndex = 1;
        public int[] starsByLevelBuildIndex = new int[16];
        public bool[] crownByLevelBuildIndex = new bool[16];
        public float[] bestTimeByLevelBuildIndex = new float[16]; 
        public int winCount;
        public bool reviewPromptShown;
        public bool reviewCompleted;


    }
}