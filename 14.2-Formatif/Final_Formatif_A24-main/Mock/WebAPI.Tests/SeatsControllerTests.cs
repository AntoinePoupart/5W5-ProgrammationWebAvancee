using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebAPI.Controllers;
using WebAPI.Exceptions;
using WebAPI.Models;
using WebAPI.Services;

namespace WebAPI.Tests;

[TestClass]
public class SeatsControllerTests
{
    Mock<SeatsService> serviceMock;
    Mock<SeatsController> controllerMock;

    public SeatsControllerTests()
    {
        // Créer en utilisant le Service du controller
        serviceMock = new Mock<SeatsService>();

        // Notez l'utilisation de CallBase = true
        // On veut un véritable objet CatsController et changer son comportement seulement pour la propriété UserId!
        // L'option CallBase = true nous permet de garder le comportement normal des méthode de la classe. 
        controllerMock = new Mock<SeatsController>(serviceMock.Object) { CallBase = true };
        controllerMock.Setup(x => x.UserId).Returns("6767");
    }

    [TestMethod]
    public void ReserveSeatBon()
    {
        Seat seat = new Seat();
        seat.Id = 1;
        seat.Number = 1;

        //Mettre dans le It.IsAny ce que la méthode retounrne donc ici ReserveSeat(string userid, int seatnumber)
        serviceMock.Setup(x => x.ReserveSeat(It.IsAny<string>(), It.IsAny<int>())).Returns(seat);

        var actionresult = controllerMock.Object.ReserveSeat(seat.Number);
        //Pour le résultat toujours mettre ce que retourne le test avec Object ou sans dépendant si retourne un objet à l'intérieur ou non.
        //Ex : return Ok(seat) DONC OkObjectResult
        //     return Unauthorized() DONC UnauthorizedResult
        var result = actionresult.Result as OkObjectResult;
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void ReserveSeatAlreadyTaken()
    {
        //Quand il y a exception, Throw la même execption à l'endroit que tu test, ici : SeatsController.cs
        serviceMock.Setup(x => x.ReserveSeat(It.IsAny<string>(), It.IsAny<int>())).Throws(new SeatAlreadyTakenException());
        
        var actionresult = controllerMock.Object.ReserveSeat(1);
        var result = actionresult.Result as UnauthorizedResult;
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void ReserveSeatOutOfBounds()
    {
        Seat seat = new Seat();
        seat.Id = 1;
        var seatNumber = seat.Number = 1;

        serviceMock.Setup(x => x.ReserveSeat(It.IsAny<string>(), It.IsAny<int>())).Throws(new SeatOutOfBoundsException());

        var actionresult = controllerMock.Object.ReserveSeat(1);
        var result = actionresult.Result as NotFoundObjectResult;
        Assert.IsNotNull(result);
        Assert.AreEqual("Could not find " + seatNumber, result.Value);

    }

    [TestMethod]
    public void UserAlreadySeated()
    {
        serviceMock.Setup(x => x.ReserveSeat(It.IsAny<string>(), It.IsAny<int>())).Throws(new UserAlreadySeatedException());

        var actionresult = controllerMock.Object.ReserveSeat(1);
        var result = actionresult.Result as BadRequestResult;
        Assert.IsNotNull(result);
    }
}
