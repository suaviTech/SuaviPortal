using IzmPortal.Domain.Common;

namespace IzmPortal.Domain.Entities;

public class Slider : BaseEntity
{
    public string Title { get; private set; } = null!;
    public string ImagePath { get; private set; } = null!;
    public bool IsActive { get; private set; } = true;

    protected Slider() { }

    public Slider(string title, string imagePath)
    {
        Title = title;
        ImagePath = imagePath;
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
}

