﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class TimeTrackingEntities : DbContext
    {
        public TimeTrackingEntities()
            : base("name=TimeTrackingEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<UserDetail> UserDetails { get; set; }
        public virtual DbSet<TimeInOut> TimeInOuts { get; set; }
        public virtual DbSet<TimeSheet> TimeSheets { get; set; }
        public virtual DbSet<serviceCode> serviceCodes { get; set; }
        public virtual DbSet<planSection> planSections { get; set; }
        public virtual DbSet<status> status { get; set; }
        public virtual DbSet<DaysOfWeek> DaysOfWeeks { get; set; }
        public virtual DbSet<LogsItem> LogsItems { get; set; }
        public virtual DbSet<LogsLkp> LogsLkps { get; set; }
        public virtual DbSet<LogsType> LogsTypes { get; set; }
        public virtual DbSet<UserLog> UserLogs { get; set; }
        public virtual DbSet<LogCategory> LogCategories { get; set; }
        public virtual DbSet<Toileting> Toiletings { get; set; }
        public virtual DbSet<v_Logs> v_Logs { get; set; }
    }
}
