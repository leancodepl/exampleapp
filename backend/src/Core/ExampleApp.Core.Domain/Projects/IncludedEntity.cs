namespace ExampleApp.Core.Domain.Projects
{
    public class IncludedEntity
    {
        public ProjectId ProjectId { get; private init; }
        public int SomeInt { get; private set; }
        public string SomeString { get; private set; } = null!;

        public IncludedEntity(ProjectId projectId, int someInt, string someString)
        {
            ProjectId = projectId;
            SomeInt = someInt;
            SomeString = someString;
        }

        public void Update(string someString)
        {
            SomeString = someString;
        }
    }
}
