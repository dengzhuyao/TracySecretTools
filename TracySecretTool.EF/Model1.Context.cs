﻿//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace TracySecretTool.EF
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class TracySecretToolEntities : DbContext
    {
        public TracySecretToolEntities()
            : base("name=TracySecretToolEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<TMsg_Code> TMsg_Code { get; set; }
        public DbSet<TTotal_Config> TTotal_Config { get; set; }
        public DbSet<TTotal_Error> TTotal_Error { get; set; }
        public DbSet<TTrain_Station> TTrain_Station { get; set; }
    }
}
