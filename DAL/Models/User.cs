using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    [NotMapped]
    public string Password { get; set; }
    public string HashedPassword { get; set; }
    public string Salt { get; set; }
}