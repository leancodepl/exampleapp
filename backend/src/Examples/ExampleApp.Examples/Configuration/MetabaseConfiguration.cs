namespace ExampleApp.Examples.Configuration;

public sealed record class MetabaseConfiguration(
    string MetabaseUrl,
    string MetabaseSecretKey,
    int AssignmentEmployerEmbedQuestion
)
{
    public MetabaseConfiguration()
        : this(default!, default!, default) { }
};
