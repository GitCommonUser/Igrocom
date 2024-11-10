using System.ComponentModel.DataAnnotations;

namespace Igrocom.Models;

public class Content
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Не указано название!")]
    public string? Title { get; set; }

    [Required(ErrorMessage = "Не указан основной текст!")]
    public string? Text { get; set; }

    [Required(ErrorMessage = "Не указано предисловие!")]
    public string? Preface { get; set; }


    [DataType(DataType.Date)]
    public DateOnly ReleaseDate { get; set; }

    public byte[]? Image { get; set; }
    public string? ImageMime { get; set; }
    public List<MediaFiles>? MediaFiles { get; set; }

    public ICollection<UserContent> UserContent { get; set; }
}

public class MediaFiles
{
    [Required]
    public int Id { get; set; }
    public byte[]? File { get; set; }
    public string? FileMime { get; set; }

    [Required]
    public int ContentId { get; set; }
}