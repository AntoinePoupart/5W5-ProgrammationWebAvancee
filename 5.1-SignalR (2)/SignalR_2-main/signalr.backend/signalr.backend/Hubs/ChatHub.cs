using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using signalr.backend.Data;
using signalr.backend.Models;


namespace signalr.backend.Hubs
{
    // On garde en mémoire les connexions actives (clé: email, valeur: userId)
    // Note: Ce n'est pas nécessaire dans le TP
    public static class UserHandler
    {
        public static Dictionary<string, string> UserConnections { get; set; } = new Dictionary<string, string>();
    }

    // L'annotation Authorize fonctionne de la même façon avec SignalR qu'avec Web API
    [Authorize]
    // Le Hub est le type de base des "contrôleurs" de SignalR
    public class ChatHub : Hub
    {
        public ApplicationDbContext _context;

        public IdentityUser CurentUser
        {
            get
            {
                // On récupère le userid à partir du Cookie qui devrait être envoyé automatiquement
                string userid = Context.UserIdentifier!;
                return _context.Users.Single(u => u.Id == userid);
            }
        }

        public ChatHub(ApplicationDbContext context)
        {
            _context = context;
        }

        public async override Task OnConnectedAsync()
        {
            UserHandler.UserConnections.Add(CurentUser.Email!, Context.UserIdentifier);

            // TODO: Envoyer des message aux clients pour les mettre à jour
            await Clients.All.SendAsync("UsersList", UserHandler.UserConnections.ToList());

            // Envoie la liste des canaux au client qui vient de se connecter
            var channel = await _context.Channel.ToListAsync();
            await Clients.All.SendAsync("ChannelsList", channel);

        }

        public async override Task OnDisconnectedAsync(Exception? exception)
        {
            // Lors de la fermeture de la connexion, on met à jour notre dictionnary d'utilisateurs connectés
            KeyValuePair<string, string> entrie = UserHandler.UserConnections.SingleOrDefault(uc => uc.Value == Context.UserIdentifier);
            UserHandler.UserConnections.Remove(entrie.Key);

            // TODO: Envoyer un message aux clients pour les mettre à jour
            await Clients.All.SendAsync("UsersList", UserHandler.UserConnections.ToList());
        }

        public async Task CreateChannel(string title)
        {
            _context.Channel.Add(new Channel { Title = title });
            await _context.SaveChangesAsync();

            // TODO: Envoyer un message aux clients pour les mettre à jour
            var channel = await _context.Channel.ToListAsync();
            await Clients.All.SendAsync("ChannelsList", channel);
        }

        public async Task DeleteChannel(int channelId)
        {
            Channel channel = _context.Channel.Find(channelId);

            if(channel != null)
            {
                _context.Channel.Remove(channel);
                await _context.SaveChangesAsync();
            }
            string groupName = CreateChannelGroupName(channelId);
            // Envoyer les messages nécessaires aux clients

            await Clients.Group(CreateChannelGroupName(channelId)).SendAsync("NewMessage", channel.Title + " a été supprimé ");
            await Clients.Group(CreateChannelGroupName(channelId)).SendAsync("LeaveChannel");
            await Clients.All.SendAsync("ChannelsList", await _context.Channel.ToListAsync());
        }

        public async Task JoinChannel(int oldChannelId, int newChannelId)
        {
            string userTag = "[" + CurentUser.Email! + "]";

            if(oldChannelId > 0)
            {

                // TODO: Faire quitter le vieux canal à l'utilisateur
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, CreateChannelGroupName(oldChannelId));

                //Récupérer le nom du canal pour ensuite faire un message exclusivement au gens dans le canal que l'utilisateur a quitter
                Channel channel = _context.Channel.Find(oldChannelId);
                await Clients.Group(CreateChannelGroupName(oldChannelId)).SendAsync("NewMessage", userTag + " a quitte le canal : " + channel.Title);

            }
         
            if (newChannelId > 0)
            {

                // TODO: Faire joindre le nouveau canal à l'utilisateur
                await Groups.AddToGroupAsync(Context.ConnectionId, CreateChannelGroupName(newChannelId));

                //Récupérer le nom du canal pour ensuite faire un message exclusivement au gens dans le canal que l'utilisateur a rejoint
                Channel channel = _context.Channel.Find(newChannelId);
                await Clients.Group(CreateChannelGroupName(newChannelId)).SendAsync("NewMessage", userTag + " a rejoint le canal : " + channel.Title);

            }
                
        }

        public async Task SendMessage(string message, int channelId, string userId)
        {
            if (userId != null)
            {
                // TODO: Envoyer le message à cet utilisateur
                await Clients.User(userId).SendAsync("NewMessage", "[De: " + CurentUser.Email + "] " + message);


            }
            else if (channelId != 0)
            {
                // TODO: Envoyer le message aux utilisateurs connectés à c be canal
                Channel channel = _context.Channel.Find(channelId);
                await Clients.Group(CreateChannelGroupName(channelId)).SendAsync("NewMessage", "[" + channel.Title + "] " + message );
            }
            else
            {
                await Clients.All.SendAsync("NewMessage", "[Tous] " + message);
            }
        }

        private static string CreateChannelGroupName(int channelId)
        {
            return "Channel" + channelId;
        }
    }
}