namespace GameProject.Inventory;

/// <summary>Pickup-owned counters and unlocks for one dungeon run; player health stays on the player.</summary>
public sealed class InventoryState
{
    public int Rupees { get; private set; }
    public int Keys { get; private set; }
    public int Bombs { get; private set; }
    public int StarShards { get; private set; }
    public bool HasBow { get; private set; }
    public bool HasBoomerang { get; private set; }
    public bool HasMap { get; private set; }
    public bool HasCompass { get; private set; }
    public bool HasObjective => StarShards > 0;

    internal void AddRupee() => Rupees++;
    internal void AddKey() => Keys++;
    internal void AddBomb() => Bombs++;
    internal void AddStarShard() => StarShards++;
    internal void UnlockBow() => HasBow = true;
    internal void UnlockBoomerang() => HasBoomerang = true;
    internal void RevealMap() => HasMap = true;
    internal void RevealCompass() => HasCompass = true;

    /// <summary>Restores the empty inventory used at the start of a run.</summary>
    public void Reset()
    {
        Rupees = 0;
        Keys = 0;
        Bombs = 0;
        StarShards = 0;
        HasBow = false;
        HasBoomerang = false;
        HasMap = false;
        HasCompass = false;
    }
}
