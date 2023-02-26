﻿namespace Prestadito.Security.Application.Services.Interfaces
{
    public interface IDataService
    {
        public IUserRepository Users { get; }
        public ISessionRepository Sessions { get; }
    }
}
