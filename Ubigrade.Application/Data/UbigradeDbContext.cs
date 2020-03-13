using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Ubigrade.Application.Data
{
    public class UbigradeDbContext : IdentityDbContext
    {
        public UbigradeDbContext(DbContextOptions<UbigradeDbContext> options)
            : base(options)
        {
        }
    }
}
