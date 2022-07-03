namespace streamShopAPI
{
    public class Product
    {
        public int? Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int Codigo { get; set; }
        public float Preco { get; set; }
        public float PrecoPromocional { get; set; }
        public string CategoriaDesc { get; set; } = string.Empty;
        public List<ProductImage>? ProductImages { get; set; }
    }

    public class ProductImage
    {
        public int? Id { get; set; }
        public int ProductId { get; set; }
        public string Caminho { get; set; } = string.Empty;
        public int Ordem { get; set; }
    }
}