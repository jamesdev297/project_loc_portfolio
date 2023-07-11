using System;

public static class ItemCardFactory {
    
    public enum ItemCardType
    {
        LongSword, // 롱소드
        Dagger, // 단검
        RubyCrystal, //루비 수정
        ClothArmor, // 천 갑옷
        RejuvenationBead, // 원기 회복의 구슬
        VampiricScepter,// 흡혈의 낫
        Stonemail, // 돌갑옷
        Thronmail, // 가시갑옷
    }
    
    public static CardModel Create(ItemCardType type)
    {
        switch (type)
        {
            case ItemCardType.LongSword:
                return new LongSwordModel();
            case ItemCardType.Dagger:
                return new DaggerModel();
            case ItemCardType.RubyCrystal:
                return new RubyCrystalModel();
            case ItemCardType.ClothArmor:
                return new ClothArmorModel();
            case ItemCardType.RejuvenationBead:
                return new RejuvenationBeadModel();
            case ItemCardType.VampiricScepter:
                return new VampiricScepterModel();
            default:
                return null;
        }
    }

    public static CardModel Create(string name)
    {
        ItemCardType itemCardType;
        try
        {
            itemCardType = (ItemCardType) Enum.Parse(typeof(ItemCardType), name);
        }
        catch (Exception e)
        {
            return null;
        }
        return Create(itemCardType);
    }
    
    public static CardModel Create(int id)
    {
        switch (id)
        {
            case 0:
                return new LongSwordModel();
            case 1:
                return new DaggerModel();
            case 2:
                return new ClothArmorModel();
            case 3:
                return new ThronmailModel();
            case 4:
                return new StonemailModel();
            case 5:
                return new VampiricScepterModel();
            default:
                return null;
        }
    }
}