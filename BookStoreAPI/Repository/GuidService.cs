
namespace BookStoreAPI.Repository
{
    public class GuidService : IGuidService
    {
        private readonly Guid _guid;

        public GuidService()
        {
            _guid = Guid.NewGuid();
        }
        public Guid GetGuid()
        {
            return _guid;
        }
    }
}
