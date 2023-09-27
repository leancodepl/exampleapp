namespace LeanCode.AuditLogs;

public record EntityData(
    IReadOnlyList<string> Ids,
    string Type,
    string Changes,
    string ShadowProperties,
    string EntityState
);
