namespace IzmPortal.Application.DTOs.Slider;

public class SliderDto
{
    public Guid Id { get; set; }
    public string ImagePath { get; set; } = null!;
    public string? Title { get; set; }
    public DateTime CreatedAt { get; set; }
}
