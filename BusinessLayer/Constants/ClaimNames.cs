namespace BusinessLayer.Constants
{
    /// <summary>
    /// Custom claim names written into the JWT. The controller reads them back from User.
    /// </summary>
    public static class ClaimNames
    {
        public const string UserId = "userId";
        public const string Email = "email";
    }
}
