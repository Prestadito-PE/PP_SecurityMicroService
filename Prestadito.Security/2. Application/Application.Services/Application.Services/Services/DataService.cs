using Prestadito.Security.Application.Services.Interfaces;
using Prestadito.Security.Application.Services.Repositories;
using Prestadito.Security.Infrastructure.Data.Context;

namespace Prestadito.Security.Application.Services.Services
{
    public class DataService : IDataService
    {
        private readonly MongoContext context;

        public DataService(MongoContext _context)
        {
            context = _context;
        }

        public IUserRepository Users
        {
            get
            {
                return new UserRepository(context.Database);
            }
        }

        public ISessionRepository Sessions
        {
            get
            {
                return new SessionRepository(context.Database);
            }
        }
    }
}
