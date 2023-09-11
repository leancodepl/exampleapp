namespace ExampleApp.Core.Domain.Projects
{
    public class OwnedEntity
    {
        public int SomeInt { get; private set; }
        public string SomeString { get; private set; } = null!;

        public OwnedEntity(int someInt, string someString)
        {
            SomeInt = someInt;
            SomeString = someString;
        }

        public void Update(string someString)
        {
            SomeString = someString;
        }
    }
}
