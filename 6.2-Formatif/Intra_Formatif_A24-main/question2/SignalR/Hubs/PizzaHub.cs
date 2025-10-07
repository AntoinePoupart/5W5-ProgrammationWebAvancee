using System.Threading.Channels;
using Microsoft.AspNetCore.SignalR;
using SignalR.Services;

namespace SignalR.Hubs
{
    public class PizzaHub : Hub
    {
        private readonly PizzaManager _pizzaManager;

        public PizzaHub(PizzaManager pizzaManager) {
            _pizzaManager = pizzaManager;
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            _pizzaManager.AddUser();
            int nbUser = _pizzaManager.NbConnectedUsers;
            await Clients.All.SendAsync("UpdateNbUsers", nbUser);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnConnectedAsync();
            _pizzaManager.RemoveUser();
            int nbUser = _pizzaManager.NbConnectedUsers;
            await Clients.All.SendAsync("UpdateNbUsers", nbUser);
        }

        public async Task SelectChoice(PizzaChoice choice)
        {
            int prix = _pizzaManager.PIZZA_PRICES[(int)choice];
            int nbPizza = _pizzaManager.NbPizzas[(int)choice];
            int money = _pizzaManager.Money[(int)choice];
            string groupName = _pizzaManager.GetGroupName(choice);

            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("selectedChoice", prix, nbPizza, money);

        }

        public async Task UnselectChoice(PizzaChoice choice)
        {
            string groupName = _pizzaManager.GetGroupName(choice);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
           
        }

        public async Task AddMoney(PizzaChoice choice)
        {
            _pizzaManager.IncreaseMoney(choice);
            int money = _pizzaManager.Money[(int)choice];
            await Clients.Group(_pizzaManager.GetGroupName(choice)).SendAsync("addMoney", money);
        }

        public async Task BuyPizza(PizzaChoice choice)
        {
            _pizzaManager.BuyPizza(choice);
            int nbPizza = _pizzaManager.NbPizzas[(int)choice];
            await Clients.Group(_pizzaManager.GetGroupName(choice)).SendAsync("buyPizza", nbPizza);
        }
    }
}
