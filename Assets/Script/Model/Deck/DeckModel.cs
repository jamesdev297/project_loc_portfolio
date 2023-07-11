using System;
using System.Collections.Generic;

[Serializable]
public class DeckModel
{
    
    private string name;
    private List<CardModel> cards;

    public DeckModel(string name, List<CardModel> cards)
    {
        this.name = name;
        this.cards = cards;
    }

    public void AddCard(CardModel cardModel)
    {
        cards.Add(cardModel);
    }

    public void RemoveCard(int index)
    {
        if(cards.Count > 0)
            cards.RemoveAt(index);
    }

    public List<CardModel> GetCardList()
    {
        return cards;
    }
}
