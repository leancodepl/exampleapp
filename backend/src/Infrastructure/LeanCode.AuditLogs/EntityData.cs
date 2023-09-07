namespace LeanCode.AuditLogs;

public class EntityData
{
    public IEnumerable<string> Ids { get; set; } = null!;
    public string Type { get; set; } = null!;
    public string Changes { get; set; } = null!;
}
