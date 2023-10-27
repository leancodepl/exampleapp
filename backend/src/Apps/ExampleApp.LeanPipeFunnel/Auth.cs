namespace ExampleApp.LeanPipeFunnel;

public static class Auth
{
    public static class Roles
    {
        public const string User = "user";
        public const string Admin = "admin";
    }

    public static class KnownClaims
    {
        public const string UserId = "sub";
        public const string Role = "role";
    }
}
