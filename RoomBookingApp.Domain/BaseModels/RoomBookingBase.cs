using System.ComponentModel.DataAnnotations;

namespace RoomBookingApp.Domain.BaseModels;

public abstract class RoomBookingBase : IValidatableObject
{
    public int? Id { get; set; }

    [Required]
    [StringLength(80)]
    public string FullName { get; set; }

    [Required]
    [StringLength(80)]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime Date { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Date < DateTime.Now)
        {
            yield return new ValidationResult("Date cannot be in the past", new[] { nameof(Date) });
        }
    }
}