﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EvidencijaVremena
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class EvidencijaVremenaEntities : DbContext
    {
        public EvidencijaVremenaEntities()
            : base("name=EvidencijaVremenaEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Aktivnost> Aktivnost { get; set; }
        public virtual DbSet<Evidencija> Evidencija { get; set; }
        public virtual DbSet<Korisnik> Korisnik { get; set; }
        public virtual DbSet<Opterecenje> Opterecenje { get; set; }
        public virtual DbSet<Predmet> Predmet { get; set; }
        public virtual DbSet<Pretplata> Pretplata { get; set; }
        public virtual DbSet<TipAktivnosti> TipAktivnosti { get; set; }
    }
}
