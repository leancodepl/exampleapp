namespace ExampleApp.Core.Domain.Projects
{
    public class IncludedEntity
    {
        public int SomeInt { get; private set; }
        public string SomeString { get; private set; } = null!;

        public IncludedEntity(int someInt, string someString)
        {
            SomeInt = someInt;
            SomeString = someString;
        }

        public void Update(int someInt, string someString)
        {
            SomeInt = someInt;
            SomeString = someString;
        }
    }
}
