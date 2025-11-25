using Moq;
using Stonk;

var stonkMarketMock = new Mock<IStonkMarketService>();
var analyzerController = new StonksAnalyzerController(stonkMarketMock.Object);

//stonkMarketMock.Setup(x => x.GetStonkValue(Stonks.Nvda)).Returns(42.42M);
//stonkMarketMock.Setup(x => x.GetStonkValue(Stonks.Opai)).Returns(14.42M);

stonkMarketMock.Setup(x => x.GetStonkValue(It.IsAny<Stonks>())).Returns(10M);
stonkMarketMock.SetupSequence(x => x.GetNbStonks(It.IsAny<Stonks>())).Returns(10).Returns(20).Returns(30);

Console.WriteLine("Bienvenu dans votre portfolio. Voici vos actifs :");
Console.WriteLine("- NVIDIA   " + analyzerController.GetPortfolioStonkValue(Stonks.Nvda));
Console.WriteLine("- Open AI  " + analyzerController.GetPortfolioStonkValue(Stonks.Opai));
Console.WriteLine("- Apple    " + analyzerController.GetPortfolioStonkValue(Stonks.Aapl));
Console.WriteLine("- AMD      " + analyzerController.GetPortfolioStonkValue(Stonks.Amd));

var allo = new Random(123);
Console.WriteLine(allo.NextInt64());