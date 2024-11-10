
using System.ComponentModel.DataAnnotations;

namespace Igrocom.Models;

public class User
{
    public int Id { get; set; }

    [StringLength(20, ErrorMessage = "Логин не может иметь длину более 20-ти букв.")]
    [RegularExpression("^[a-zA-z]{2,}$", ErrorMessage = "Неккоректный логин!")]
    [Display(Name = "Логин")]
    [Required(ErrorMessage = "Укажите логин !")]
    public string Login {get; set;}
    

    [DataType(DataType.Password)]
    //[RegularExpression("^[a-zA-Z\\d]{8,}$", ErrorMessage = "Пароль должен иметь длину минимум 8 символов. И содержать только латинские буквы.")]
    [RegularExpression("^[a-zA-Z\\d]{8,}$", ErrorMessage = "Некорректный пароль!")]
    [StringLength(100, ErrorMessage = "Поле {0} должно иметь минимум {2} и максимум {1} символов.", MinimumLength = 8)]
    [Display(Name = "Пароль")]
    [Required(ErrorMessage = "Введите пароль !")]
    public string Password {get; set;}

    public enum Roles 
    {
        [Display(Name = "Пользователь")]
        common,
        [Display(Name = "Администратор")]
        admin
    }

    public Roles Role {get;set;} = Roles.common;

    public ICollection<UserGame> UserGame { get; set; }
    public ICollection<UserContent> UserContent { get; set; }

}

public class UserGame 
{
    public int GameId {get; set;}
    public Game Game {get;set;}
    public int UserId {get; set;}
    public User User {get; set;}
}

public class UserContent
{
    public int ContentId { get; set; }
    public Content Content { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
}

