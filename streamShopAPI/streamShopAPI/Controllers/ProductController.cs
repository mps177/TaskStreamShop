using Microsoft.AspNetCore.Mvc;
using streamShopAPI.Data;
using Microsoft.AspNetCore.Hosting.Builder;

namespace streamShopAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly DataContext _context;
        private IWebHostEnvironment _hostEnvironment;

        public ProductController(DataContext dataContext, IWebHostEnvironment environment)
        {
            _context = dataContext;
            _hostEnvironment = environment;
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetAllProducts()
        {
            var products = new List<Product>();
            products = await _context.Product.ToListAsync();

            foreach (var c in products)
            {
                var query = _context.ProductImage
                       .Where(s => s.ProductId == c.Id && s.Ordem == 1)
                       .FirstOrDefault<ProductImage>();

                _context.Entry(c).GetDatabaseValues();
            }

            return Ok(await _context.Product.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> Get(int id)
        {
            var produto = await _context.Product.FindAsync(id);
            var query = _context.ProductImage
                       .Where(s => s.ProductId == produto.Id)
                       .ToList<ProductImage>();

            _context.Entry(produto).GetDatabaseValues();

            if (produto == null)
                return BadRequest("Product not found.");
            return Ok(produto);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> AddProduct(Product produto)
        {
            try
            {
                _context.Product.Add(produto);
                await _context.SaveChangesAsync();
                _context.Entry(produto).GetDatabaseValues();
                int id = (int)produto.Id;
                return Ok(produto.Id);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("images")]
        public async Task<ActionResult<ProductImage>> AddProductImage(ProductImage produtoImage)
        {
            try
            {
                _context.ProductImage.Add(produtoImage);
                await _context.SaveChangesAsync();
                _context.Entry(produtoImage).GetDatabaseValues();
                int id = (int)produtoImage.Id;
                return Ok(produtoImage);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<List<Product>>> UpdateProduct(Product request)
        {
            var dbProduct = await _context.Product.FindAsync(request.Id);

            if (dbProduct == null)
                return BadRequest("Product not found.");

            dbProduct.Nome = request.Nome;
            dbProduct.CategoriaDesc = request.CategoriaDesc;
            dbProduct.Codigo = request.Codigo;
            dbProduct.Preco = request.Preco;
            dbProduct.PrecoPromocional = request.PrecoPromocional;

            await _context.SaveChangesAsync();

            return Ok();
        }
        
        //[HttpPut("/images")]
        //public async Task<ActionResult<List<ProductImage>>> UpdateProductImage(ProductImage produtoImage)
        //{
        //    try
        //    {
        //        _context.ProductImage.Add(produtoImage);
        //        await _context.SaveChangesAsync();
        //        _context.Entry(produtoImage).GetDatabaseValues();
        //        //int id = (int)produtoImage.ProductId;
        //        return Ok(produtoImage);
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e.Message);
        //    }
        //}
        
        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Product>>> DeleteProduct(int id)
        {
            try
            {
                var produto = await _context.Product.FindAsync(id);

                if (produto == null)
                    return BadRequest("Hero not found.");

                var query = _context.ProductImage
                          .Where(s => s.ProductId == produto.Id)
                          .ToList<ProductImage>();
                foreach (var image in query)
                {
                    _context.ProductImage.Remove(image);
                }
            
                _context.Product.Remove(produto);
                await _context.SaveChangesAsync();

                return Ok(await _context.Product.ToListAsync());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        //[HttpDelete("/images/{id}")]
        //public async Task<ActionResult<List<Product>>> DeleteProductImages(int id)
        //{
        //    try
        //    {
        //        var productImage = await _context.ProductImage.FindAsync(id);

        //        if (productImage == null)
        //            return BadRequest("Hero not found.");

        //        _context.ProductImage.Remove(productImage);
        //        await _context.SaveChangesAsync();

        //        return Ok(await _context.ProductImage.ToListAsync());
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e.Message);
        //    }
        //}
    }
}