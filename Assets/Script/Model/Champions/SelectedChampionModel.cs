
using System.Collections.Generic;
using System.Linq;

public class MyChampionModel
{
    private ChampionModel championModel;
    private List<CardModel> mountedCards;
    private List<ChampionModel> unlockedChampionModels;

    MyChampionModel(ChampionFactory.ChampionType championType, List<CardModel> mountedCards, List<ChampionModel> unlockedChampionModels)
    {
        championModel = ChampionFactory.Create(championType);
        this.mountedCards = mountedCards;
        this.unlockedChampionModels = unlockedChampionModels;
    }
    // public int getOffensePower()
    // {
    //     int sum = 0;
    //     mountedCards.ForEach(mountedCard => sum += mountedCard.getOffensePower());
    //     return sum;
    // }
    
    public void addMountedCard(CardModel cardModel)
    {
        mountedCards.Add(cardModel);
    }
    
    public CardModel removeMountedCard(CardModel cardModel)
    {
        mountedCards.Remove(cardModel);
        return cardModel;
    }
    
    public List<CardModel> MountedCards
    {
        get
        {
            return mountedCards;
        }
    }
}
