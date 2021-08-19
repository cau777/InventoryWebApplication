using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using InventoryWebApplication.Models;

namespace InventoryWebApplication.CustomExceptions
{
    public class InvalidRoleException : Exception
    {
        public override IDictionary Data => new Dictionary<string, string>
        {
            {"Role Name", _roleName}
        };

        public override string Message
        {
            get
            {
                if (Role.AvailableRoles.Any(o => o.ToLower() == _roleName.ToLower()))
                {
                    return "Case Mismatch";
                }

                return $"Invalid Role {_roleName}";
            }
        }

        private readonly string _roleName;

        public InvalidRoleException(string roleName)
        {
            _roleName = roleName;
        }
    }
}