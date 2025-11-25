namespace Stonk;

public interface IStonkMarketService
{
    decimal GetStonkValue(Stonks stonk);

    int GetNbStonks(Stonks stonk);
}

public enum Stonks
{
    Nvda, // NVIDIA
    Opai, // Open AI
    Aapl, // Apple
    Amd, // AMD
}

