namespace LeanCode.AuditLogs;

public class EntityData
{
    public IReadOnlyList<object> Ids { get; set; } = null!;
    public string Type { get; set; } = null!;
    public string Changes { get; set; } = null!;
}
