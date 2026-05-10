namespace SimpleApp.Core.Models;

public class TagValue
{
    public string TagId { get; set; } = string.Empty;
    public double Value { get; set; }
    public bool IsValid { get; set; } = true;
}
