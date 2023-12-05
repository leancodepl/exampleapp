namespace ExampleApp.Examples.Services.Configuration;

public sealed record class MetabaseConfiguration(
    string MetabaseUrl,
    string MetabaseSecretKey,
    int AssignmentEmployerEmbedQuestion
)
{
    public MetabaseConfiguration()
        : this(default!, default!, default) { }
};
