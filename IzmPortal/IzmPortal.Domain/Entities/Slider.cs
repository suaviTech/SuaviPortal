using IzmPortal.Domain.Common;

namespace IzmPortal.Domain.Entities;

public class Slider : BaseEntity
{
    public string ImagePath { get; private set; } = null!;

    protected Slider() { } // EF Core

    public Slider(string imagePath)
    {
        ImagePath = imagePath;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateImage(string imagePath)
    {
        ImagePath = imagePath;
        UpdatedAt = DateTime.UtcNow;
    }
}
