﻿namespace Prestadito.Security.Application.Dto.User.UpdateUser
{
    public class UpdateUserRequest
    {
        public string StrId { get; set; } = string.Empty;
        public string StrEmail { get; set; } = string.Empty;
        public string StrPassword { get; set; } = string.Empty;
        public string StrRolId { get; set; } = string.Empty;
        public bool BlnEmailValidated { get; set; }
        public bool BlnLockByAttempts { get; set; }
        public bool BlnCompleteInformation { get; set; }
        public bool BlnActive { get; set; }
    }
}
