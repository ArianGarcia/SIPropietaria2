namespace SistemaInventarioVentas.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RestoreProductoTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Productos", "AlmacenID", "dbo.Almacen");
            DropForeignKey("dbo.Productos", "CategoriaID", "dbo.Categorias");
            AddForeignKey("dbo.Productos", "AlmacenID", "dbo.Almacen", "AlmacenID", cascadeDelete: true);
            AddForeignKey("dbo.Productos", "CategoriaID", "dbo.Categorias", "CategoriaID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Productos", "CategoriaID", "dbo.Categorias");
            DropForeignKey("dbo.Productos", "AlmacenID", "dbo.Almacen");
            AddForeignKey("dbo.Productos", "CategoriaID", "dbo.Categorias", "CategoriaID");
            AddForeignKey("dbo.Productos", "AlmacenID", "dbo.Almacen", "AlmacenID");
        }
    }
}
