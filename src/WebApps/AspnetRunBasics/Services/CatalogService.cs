using AspnetRunBasics.Extensions;
using AspnetRunBasics.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AspnetRunBasics.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly HttpClient _client;
        private readonly ILogger<CatalogService> _logger;

        public CatalogService(
            HttpClient client,
            ILogger<CatalogService> logger)
        {
            _client = client;
            _logger = logger;
        }

        public Task<CatalogModel> CreateCatalog(CatalogModel model)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<CatalogModel>> GetCatalog()
        {
            _logger.LogInformation("{ApplicationName} {ClassName} {FunctionName} Get catalog from Url {Url} Start",
                "AspnetRunBasics", nameof(CatalogService), nameof(GetCatalog), _client.BaseAddress);
            var response = await _client.GetAsync("/Catalog");
            var catalogModelList = await response.ReadContentAs<List<CatalogModel>>();

            _logger.LogInformation("{ApplicationName} {ClassName} {FunctionName} Get catalog from Url {Url} End",
                "AspnetRunBasics", nameof(CatalogService), nameof(GetCatalog), _client.BaseAddress);

            return catalogModelList;
        }

        public async Task<CatalogModel> GetCatalog(string id)
        {
            _logger.LogInformation("{ApplicationName} {ClassName} {FunctionName} Get catalog by id {CatalogId} from Url {Url} Start",
               "AspnetRunBasics", nameof(CatalogService), nameof(GetCatalog), id, _client.BaseAddress);
            var response = await _client.GetAsync($"/Catalog/{id}");
            var catalog = await response.ReadContentAs<CatalogModel>();
            _logger.LogInformation("{ApplicationName} {ClassName} {FunctionName} Get catalog by id {CatalogId} from Url {Url} CatalogName {CatalogName} End",
                "AspnetRunBasics", nameof(CatalogService), nameof(GetCatalog), id, _client.BaseAddress, catalog.Name);
            return catalog;
        }

        public async Task<IEnumerable<CatalogModel>> GetCatalogByCategory(string category)
        {
            var response = await _client.GetAsync($"/Catalog/GetProductByCategory/{category}");
            return await response.ReadContentAs<List<CatalogModel>>();
        }
    }
}
