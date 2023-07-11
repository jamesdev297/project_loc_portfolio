using System;
using System.Collections.Generic;

[Serializable]
public class StageModel
{

    private int stageNumber;
    private int levelOfDifficultyIndex;
    private string stageBoss;

    public StageModel(int stageNumber, int levelOfDifficultyIndex, string stageBoss)
    {
        this.stageNumber = stageNumber;
        this.levelOfDifficultyIndex = levelOfDifficultyIndex;
        this.stageBoss = stageBoss;
    }

    public int StageNumber
    {
        get => stageNumber;
    }

    public int LevelOfDifficultyIndex
    {
        get => levelOfDifficultyIndex;
        set => levelOfDifficultyIndex = value;
    }
    
    public string StageBoss
    {
        get => stageBoss;
    }
}