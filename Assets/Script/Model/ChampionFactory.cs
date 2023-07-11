
using System;
using Json;

public static class ChampionFactory {
    
    public enum ChampionType
    {
        Jax,
        Lucian,
        Thresh,
        Orn
    }
    
    public static ChampionModel Create(ChampionType type)
    {
        switch (type)
        {
            case ChampionType.Jax:
                return new JaxModel();
            case ChampionType.Lucian:
                return new LucianModel();
            case ChampionType.Thresh:
                return new ThreshModel();
            case ChampionType.Orn:
                return new OrnModel();
            default:
                return null;
        }
    }
    
    public static ChampionModel Create(String name)
    {
        switch (name)
        {
            case "잭스":
                return new JaxModel();
            case "루시안":
                return new LucianModel();
            case "쓰레쉬":
                return new ThreshModel();
            case "오른":
                return new OrnModel();
            default:
                return null;
        }
    }
    
    public static ChampionModel Create(int id)
    {
        switch (id)
        {
            case 0:
                return new JaxModel();
            case 1:
                return new ThreshModel();
            case 2:
                return new LucianModel();
            case 3:
                return new OrnModel();
            case 4:
                return new OrnModel();
            case 5:
                return new AkaliModel();
            default:
                return null;
        }
    }
 
}
