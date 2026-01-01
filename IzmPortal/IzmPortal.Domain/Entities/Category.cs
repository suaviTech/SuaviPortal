using IzmPortal.Domain.Common;

public class Category : BaseEntity
{
    public string Name { get; private set; } = null!;
    public bool IsActive { get; private set; } = true;

    protected Category() { }

    public Category(string name)
    {
        Name = name;
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
}
