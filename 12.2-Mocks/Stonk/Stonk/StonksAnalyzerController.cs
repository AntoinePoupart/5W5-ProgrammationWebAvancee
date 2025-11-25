namespace Stonk;

public class StonksAnalyzerController(IStonkMarketService stonkMarketService)
{
    public decimal GetPortfolioStonkValue(Stonks stonk)
    {
        return stonkMarketService.GetNbStonks(stonk) * stonkMarketService.GetStonkValue(stonk);
    }
}