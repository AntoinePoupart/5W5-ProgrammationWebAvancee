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

//public class QuestionHub : Hub
//{
//    private static List<Question> _questions = new();
//    private static Dictionary<string, List<string>> _groupViewers = new();

//    // 1️ Ajouter une question
//    public async Task AddQuestion(Question question)
//    {
//        _questions.Add(question);

//        // Envoyer la liste complète à tout le monde
//        await Clients.All.SendAsync("AllQuestions", _questions);

//        // Répondre à celui qui vient d’ajouter la question
//        await Clients.Caller.SendAsync("AddQuestion", question.Text);
//    }

//    // 2️ Rejoindre un groupe (sélection d'une question)
// public async Task JoinQuestionGroup(string questionId)
//  {
//    await Groups.AddToGroupAsync(Context.ConnectionId, questionId);
// on notifie le groupe en envoyant seulement l'id
//    await Clients.Group(questionId).SendAsync("nbviewers", questionId);
//  }

//    // 3️ Quitter le groupe
//    public async Task LeaveGroup(string questionId)
//    {
//        await Groups.RemoveFromGroupAsync(Context.ConnectionId, questionId);
//        await Clients.Group(questionId).SendAsync("nbviewers", questionId);
//    }

//    // 4️ Voter (facile)
//    public async Task Vote(string questionId, bool isYes)
//    {
//        // Tu ferais juste une logique simple ici (pas besoin de base de données)
//        await Clients.Group(questionId).SendAsync("VoteReceived", questionId, isYes);
//    }

//    // 5️ Quand un client se connecte / déconnecte
//    public override async Task OnConnectedAsync()
//    {
//        await base.OnConnectedAsync();
//        Console.WriteLine($"Connecté : {Context.ConnectionId}");
//    }

//    public override async Task OnDisconnectedAsync(Exception? exception)
//    {
//        await base.OnDisconnectedAsync(exception);
//        Console.WriteLine($"Déconnecté : {Context.ConnectionId}");
//    }
//}

//public class Question
//{
//    public string Id { get; set; } = Guid.NewGuid().ToString();
//    public string Text { get; set; }
//}