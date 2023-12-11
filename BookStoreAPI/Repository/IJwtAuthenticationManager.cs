namespace BookStoreAPI.Repository
{
    public interface IJwtAuthenticationManager
    {
        string Authenticate(string username, string password);
    }
}
