namespace IzmPortal.Application.DTOs.Category;

public class UpdateCategoryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public bool IsActive { get; set; }
}
