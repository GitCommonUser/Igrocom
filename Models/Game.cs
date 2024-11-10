using System.ComponentModel.DataAnnotations;

namespace Igrocom.Models;

public class Game
{
    public int Id { get; set; }

    [Required(ErrorMessage ="Не указано название!")]
    public string? Title { get; set; }
    [DataType(DataType.Date)]
    [Required(ErrorMessage = "Не указана дата!")]
    public /*DateTime*/DateOnly ReleaseDate { get; set; }

    [Required(ErrorMessage = "Не указан жанр!")]
    public string? Genre { get; set; }
    [Required(ErrorMessage = "Не указано описание!")]
    public string? Description {get;set;}
    [Required(ErrorMessage = "Не указаны особенности!")]
    public string? Peculiarities {get; set;}
    [Required(ErrorMessage = "Не указана рецензия!")]
    public string? Review { get; set; }
    //public string? Image {get;set;}
    public byte[]? Image {get; set; }
    public string? ImageMime {get;set;}

    // [Required(ErrorMessage = "Не указан рейтинг!")]
    // [Range(1,100, ErrorMessage = "Укажите рейтинг от 1 до 100 !" )]
    [Range(1,100)]
    public byte Rating {get;set;} = 1;

    public ICollection<Rating> Ratings {get;set;}

    public List<GameFiles>? GameFiles {get; set; }

    public ICollection<UserGame> UserGame { get; set; }
}

public class GameFiles{
    [Required]
    public int Id {get; set; }
    public byte[]? File {get; set; }
    public string? FileMime {get; set; }
    public string? FileName {get; set; }

    [Required]
    public int GameId {get; set;}
}

public class Rating {
    [Required]
    public int Id {get; set;}
    [Range(1,100)]
    public int Value {get; set;}
    public int UserId {get;set;}
    public int GameId {get; set;}

}