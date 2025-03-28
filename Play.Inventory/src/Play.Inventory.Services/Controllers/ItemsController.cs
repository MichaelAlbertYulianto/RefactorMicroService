using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Play.Common.Repositories;
using Play.Inventory.Services.Clients;
using Play.Inventory.Services.Entities;
using static Play.Inventory.Services.Dto;

namespace Play.Inventory.Services.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly IRepository<InventoryItem> itemsRepository;
        private readonly CatalogClient catalogClient;

        public ItemsController(IRepository<InventoryItem> repository, CatalogClient catalogClient)
        {
            this.itemsRepository = repository;
            this.catalogClient = catalogClient;
        }

        // [HttpGet]
        // public async Task<ActionResult<IEnumerable<InventoryItemDto>>> GetAsync()
        // {
        //     var items = (await itemsRepository.GetAllAsync()).Select(item => item.AsDto());
        //     return Ok(items);
        // }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventoryItemDto>>> GetAsync(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                return BadRequest();
            }

            var catalogItems = await catalogClient.GetCatalogItemsAsync();

            var items = (await itemsRepository.GetAllAsync())
                .Where(item => item.UserId == userId);

            var inventoryItemDtos = items.Select(item =>
            {
                var catalogItem = catalogItems.Single(catalogItem => catalogItem.Id == item.CatalogItemId);
                return item.AsDto(catalogItem.Name, catalogItem.Description);
            });

            return Ok(inventoryItemDtos);
        }

        [HttpPost]
        public async Task<ActionResult<InventoryItemDto>> PostAsync(GrantItemsDto grantItemsDto)
        {
            var catalogItems = await catalogClient.GetCatalogItemsAsync();
            var catalogItem = catalogItems.Single(catalogItem => catalogItem.Id == grantItemsDto.CatalogItemId);

            var inventoryItem = await itemsRepository.GetAsync(grantItemsDto.UserId);
            if (inventoryItem is null)
            {
                inventoryItem = new InventoryItem
                {
                    UserId = grantItemsDto.UserId,
                    CatalogItemId = grantItemsDto.CatalogItemId,
                    Quantity = grantItemsDto.Quantity,
                    AcquiredDate = DateTimeOffset.UtcNow
                };
                await itemsRepository.CreateAsync(inventoryItem);
            }
            else
            {
                inventoryItem.Quantity += grantItemsDto.Quantity;
                await itemsRepository.UpdateAsync(inventoryItem);
            }
            return Ok(inventoryItem.AsDto(catalogItem.Name, catalogItem.Description));
        }
    }
}
