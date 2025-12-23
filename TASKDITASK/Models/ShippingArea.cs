using System.ComponentModel.DataAnnotations;
namespace TASKDITASK.Models;

public class ShippingArea
{
    public int Id { get; set; }
    [MaxLength(15)]
    [MinLength(5)]
    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;
    [Required]
    public string ImagePath { get; set; } = null!;
}
