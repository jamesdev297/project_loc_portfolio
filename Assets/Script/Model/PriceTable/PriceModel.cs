
using System;
using LitJson;

[Serializable]
public class PriceModel
{
    public int day;
    public int price;

    public static PriceModel FromJson(JsonData map)
    {
        PriceModel priceModel = new PriceModel();
        priceModel.day = (int) map["day"];
        priceModel.price = (int) map["price"];
        return priceModel;
    }
}