namespace SistemaInventarioVentas.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Almacen",
                c => new
                    {
                        AlmacenID = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false),
                        CodigoAlmacen = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.AlmacenID);
            
            CreateTable(
                "dbo.Productos",
                c => new
                    {
                        ProductoID = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 20),
                        PrecioCompra = c.Single(nullable: false),
                        PrecioVenta = c.Single(nullable: false),
                        Cantidad = c.Int(nullable: false),
                        Descripcion = c.String(),
                        Estado = c.Boolean(nullable: false),
                        CategoriaID = c.Int(nullable: false),
                        AlmacenID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProductoID)
                .ForeignKey("dbo.Almacen", t => t.AlmacenID, cascadeDelete: true)
                .ForeignKey("dbo.Categorias", t => t.CategoriaID, cascadeDelete: true)
                .Index(t => t.CategoriaID)
                .Index(t => t.AlmacenID);
            
            CreateTable(
                "dbo.Categorias",
                c => new
                    {
                        CategoriaID = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false),
                        Estado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CategoriaID);
            
            CreateTable(
                "dbo.Clientes",
                c => new
                    {
                        ClienteID = c.Int(nullable: false, identity: true),
                        Apellido = c.String(nullable: false, maxLength: 20),
                        Nombre = c.String(nullable: false, maxLength: 20),
                        Cedula = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ClienteID);
            
            CreateTable(
                "dbo.Ventas",
                c => new
                    {
                        VentaID = c.Int(nullable: false, identity: true),
                        FechaVenta = c.DateTime(nullable: false),
                        Total = c.Single(nullable: false),
                        Estado = c.Boolean(nullable: false),
                        Comentario = c.String(),
                        ClientID = c.Int(nullable: false),
                        Cliente_ClienteID = c.Int(),
                    })
                .PrimaryKey(t => t.VentaID)
                .ForeignKey("dbo.Clientes", t => t.Cliente_ClienteID)
                .Index(t => t.Cliente_ClienteID);
            
            CreateTable(
                "dbo.DetalleVentas",
                c => new
                    {
                        VentaID = c.Int(nullable: false),
                        ProductoID = c.Int(nullable: false),
                        PrecioUnitario = c.Single(nullable: false),
                        SubTotal = c.Single(nullable: false),
                        Cantidad = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.VentaID, t.ProductoID })
                .ForeignKey("dbo.Productos", t => t.ProductoID, cascadeDelete: true)
                .ForeignKey("dbo.Ventas", t => t.VentaID, cascadeDelete: true)
                .Index(t => t.VentaID)
                .Index(t => t.ProductoID);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.DetalleVentas", "VentaID", "dbo.Ventas");
            DropForeignKey("dbo.DetalleVentas", "ProductoID", "dbo.Productos");
            DropForeignKey("dbo.Ventas", "Cliente_ClienteID", "dbo.Clientes");
            DropForeignKey("dbo.Productos", "CategoriaID", "dbo.Categorias");
            DropForeignKey("dbo.Productos", "AlmacenID", "dbo.Almacen");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.DetalleVentas", new[] { "ProductoID" });
            DropIndex("dbo.DetalleVentas", new[] { "VentaID" });
            DropIndex("dbo.Ventas", new[] { "Cliente_ClienteID" });
            DropIndex("dbo.Productos", new[] { "AlmacenID" });
            DropIndex("dbo.Productos", new[] { "CategoriaID" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.DetalleVentas");
            DropTable("dbo.Ventas");
            DropTable("dbo.Clientes");
            DropTable("dbo.Categorias");
            DropTable("dbo.Productos");
            DropTable("dbo.Almacen");
        }
    }
}
