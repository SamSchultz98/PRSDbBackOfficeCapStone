﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace PRSDbBackOfficeCapStone.Models
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> Options) : base(Options) { }


        protected override void OnModelCreating(ModelBuilder builder) { }


    }
}
