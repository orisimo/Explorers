public interface IConsumer
{
    PricesDictionary Price { get; }
    void Pay(ItemType currency);
}
