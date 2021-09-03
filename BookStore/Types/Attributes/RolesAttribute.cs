using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Types.Attributes
{
    public class RolesAttribute: AuthorizeAttribute
    {
        public RolesAttribute(params string[] allowedRoles)
        {
            this.Roles = string.Join(",", allowedRoles);
        }
    }
}
