public interface IHarvestable
{
    float PercentHarvested { get; }
    void HarvestTick(int harvestPoints);
}
