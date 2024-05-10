namespace H6_ChicBotique.Authorization
{
    // This custom attribute indicates that the marked method allows anonymous access, meaning
    // it can be accessed without requiring authentication or authorization.
    [AttributeUsage(AttributeTargets.Method)]
    public class AllowAnonymousAttribute: Attribute
    {
    }
}
