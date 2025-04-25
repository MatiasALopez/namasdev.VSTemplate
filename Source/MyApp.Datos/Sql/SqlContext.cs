using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;

using namasdev.Data.Entity;

using MyApp.Entidades;

namespace MyApp.Datos.Sql
{
    public class SqlContext : DbContextBase
    {
        private const string FUNCTIONS_NAMESPACE = "SqlContext";

        public SqlContext()
            : base("name=MyApp")
        {
            Configuration.LazyLoadingEnabled = false;
            Database.CommandTimeout = int.Parse(ConfigurationManager.AppSettings["BDCommandTimeoutEnSeg"]);
        }

        public DbSet<Parametro> Parametros { get; set; }
        public DbSet<CorreoParametros> CorreosParametros { get; set; }
        public DbSet<AspNetRole> AspNetRoles { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            IgnoreTypes(modelBuilder);

            // NOTA (ML): solo en caso de ser necesaria siguiente linea
            //modelBuilder.Configurations.AddFromAssembly(Assembly.GetExecutingAssembly());
            RegisterComplexTypes(modelBuilder);

            // NOTA (ML): solo en caso de ser necesaria siguiente linea (nuget: CodeFirstStoreFunctions v1.2.0)
            //modelBuilder.Conventions.Add(new FunctionsConvention<SqlContext>("dbo"));

            base.OnModelCreating(modelBuilder);
        }

        private void IgnoreTypes(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Ignore<TypeA>();
        }

        private void RegisterComplexTypes(DbModelBuilder modelBuilder)
        {
            //modelBuilder.ComplexType<TypeA>();
        }

        #region Functions

        //[DbFunction(FUNCTIONS_NAMESPACE, nameof(uf_Function))]
        //public IQueryable<TypeA> uf_Function(string param1, Guid? param2 = null)
        //{
        //    var p1 = new ObjectParameter("Param1", param1);

        //    var p2 =
        //        param2.HasValue
        //        ? new ObjectParameter("Param2", param2)
        //        : new ObjectParameter("Param2", typeof(Guid));

        //    return ((IObjectContextAdapter)this).ObjectContext
        //        .CreateQuery<TypeA>(
        //            $"{FUNCTIONS_NAMESPACE}.{nameof(uf_Function)}(@Param1,@Param2)",
        //            p1, p2);
        //}

        //public int ufn_FunctionScalar()
        //{
        //    return Database
        //        .SqlQuery<int>("SELECT [dbo].[ufn_FunctionScalar]()")
        //        .Single();
        //}
        #endregion

        #region Stored Procedures

        //public IEnumerable<TypeA> usp_StoredProcedureA(string param1, Guid? param2 = null)
        //{
        //    var p1 = new SqlParameter("Param1", param1);

        //    var p2 =
        //        param2.HasValue
        //        ? new SqlParameter("Param2", param2)
        //        : new SqlParameter("Param2", typeof(Guid));

        //    return Database.SqlQuery<TypeA>(
        //        "EXEC dbo.usp_StoredProcedureA @Param1,@Param2",
        //        new[] { p1, p2 })
        //        .ToList();
        //}

        //public void usp_StoredProcedureB(
        //    string param1, 
        //    out int paramOut,
        //    Guid? param2 = null)
        //{
        //    var pOut = new SqlParameter("@ParamOut", SqlDbType.Int) { Direction = ParameterDirection.Output };
        //    var p1 = new SqlParameter("Param1", param1);

        //    var p2 =
        //        param2.HasValue
        //        ? new SqlParameter("Param2", param2)
        //        : new SqlParameter("Param2", typeof(Guid));

        //    this.Database.ExecuteSqlCommand(
        //        TransactionalBehavior.DoNotEnsureTransaction, 
        //        $"exec dbo.usp_StoredProcedureB @Param1, @Param2, @ParamOut OUT", 
        //        new[] { p1, p2, pOut });

        //    paramOut = (int)pOut.Value;
        //}

        #endregion

    }
}
