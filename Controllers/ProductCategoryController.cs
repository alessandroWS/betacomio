using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using betacomio.Dtos.ProductCategory;
using betacomio.Services.ProductCategoryService;
using Microsoft.AspNetCore.Mvc;

namespace betacomio.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductCategoryController : ControllerBase
    {

        private readonly IProductCategoryService _productCategoryService;

        private static Logger logger= LogManager.GetCurrentClassLogger();
        public ProductCategoryController(IProductCategoryService productCategoryService)
        {
            _productCategoryService = productCategoryService;
        }

        [HttpGet("GetAll")] // Attributo per specificare il percorso dell'endpoint di questo metodo
        
        public async Task<ActionResult<ServiceResponse<List<GetProductCategoryDto>>>> GetAllCategories()
        {
            // Chiamata al servizio IProductService per ottenere tutti i prodotti
            var products = await _productCategoryService.GetProductCategory();

            // Restituisce una risposta HTTP con lo status 200 (OK) e i dati dei prodotti
            return products;
        }

        [HttpGet("{id}")] // Attributo per specificare il percorso dell'endpoint di questo metodo con un parametro "id"
        public async Task<ActionResult<ServiceResponse<ProductCategory>>> GetSingleProductCategory(int id)
        {
            // Chiama il servizio IOrderService per ottenere l'ordine con l'Id specificato per l'utente autenticato
            var order = await _productCategoryService.GetProductCategoryById(id);

            // Restituisce una risposta HTTP con lo status 200 (OK) e i dati dell'ordine
            return Ok(order);
        }
        

    }
}