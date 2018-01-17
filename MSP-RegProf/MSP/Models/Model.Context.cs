﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TurApp.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class TurAppEntities : DbContext
    {
        public TurAppEntities()
            : base("name=TurAppEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Accion> Accion { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<Departamento> Departamento { get; set; }
        public virtual DbSet<Localidad> Localidad { get; set; }
        public virtual DbSet<Menu> Menu { get; set; }
        public virtual DbSet<MenuAspNetRoles> MenuAspNetRoles { get; set; }
        public virtual DbSet<MenuAspNetRolesAccion> MenuAspNetRolesAccion { get; set; }
        public virtual DbSet<Pais> Pais { get; set; }
        public virtual DbSet<Provincia> Provincia { get; set; }
    
        public virtual ObjectResult<GetPermisosPorNombreDeUsuario_Result> GetPermisosPorNombreDeUsuario(string userName)
        {
            var userNameParameter = userName != null ?
                new ObjectParameter("UserName", userName) :
                new ObjectParameter("UserName", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetPermisosPorNombreDeUsuario_Result>("GetPermisosPorNombreDeUsuario", userNameParameter);
        }
    }
}
