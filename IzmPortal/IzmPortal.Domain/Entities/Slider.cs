using IzmPortal.Domain.Common;

namespace IzmPortal.Domain.Entities;

public class Slider : BaseEntity
{
    public string ImagePath { get; private set; } = null!;
    public string? Title { get; private set; }

    protected Slider() { }

    public Slider(string imagePath, string? title)
    {
        ImagePath = imagePath;
        Title = title;
        CreatedAt = DateTime.UtcNow;
    }
}
