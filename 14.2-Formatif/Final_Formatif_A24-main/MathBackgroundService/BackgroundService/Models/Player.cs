using System.ComponentModel;
using Microsoft.AspNetCore.Identity;

namespace BackgroundServiceMath.Models;

public class Player
{
    public int Id { get; set; }
    public string UserId { get; set; }
    [DisplayName("Username")]
    public IdentityUser User { get; set; }
    public int NbRightAnswers { get; set; }
}