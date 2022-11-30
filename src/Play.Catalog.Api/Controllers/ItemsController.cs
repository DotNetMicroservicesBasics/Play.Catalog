using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Contracts.Dtos;
using Play.Catalog.Data.Entities;
using Play.Catalog.Service;
using Play.Common.Contracts.Interfaces;

namespace Play.Catalog.Api.Controllers
{
    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        private readonly IRepository<Item> _itemsRepository;

        private static int requestCounter = 0;

        public ItemsController(IRepository<Item> itemsRepository)
        {
            _itemsRepository = itemsRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemDto>>> GetAsync()
        {
            requestCounter++;
            Console.WriteLine($"Request {requestCounter}: Starting...");
            if (requestCounter <= 2)
            {
                Console.WriteLine($"Request {requestCounter}: Delaying...");
                await Task.Delay(TimeSpan.FromSeconds(10));
            }
            if (requestCounter <= 4)
            {
                Console.WriteLine($"Request {requestCounter}: 500 (Internal Server Error)...");
                return StatusCode(500);
            }

            Console.WriteLine($"Request {requestCounter}: 200 (Ok)...");
            var items = (await _itemsRepository.GetAllAsync()).Select(item => item.AsDto());
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetByIdAsync(Guid id)
        {
            var item = await _itemsRepository.GetAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item.AsDto());
        }

        [HttpPost]
        public async Task<ActionResult<ItemDto>> PostAsync(CreateItemDto createItemDto)
        {
            var createdItem = new Item()
            {
                Id = Guid.NewGuid(),
                Name = createItemDto.Name,
                Description = createItemDto.Description,
                Price = createItemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };

            await _itemsRepository.CreateAsync(createdItem);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = createdItem.Id }, createdItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(Guid id, UpdateItemDto updateItemDto)
        {
            var existingItem = await _itemsRepository.GetAsync(id);
            if (existingItem == null)
            {
                return NotFound();
            }

            existingItem.Name = updateItemDto.Name;
            existingItem.Description = updateItemDto.Description;
            existingItem.Price = updateItemDto.Price;

            await _itemsRepository.UpdateAsync(existingItem);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var existingItem = await _itemsRepository.GetAsync(id);
            if (existingItem == null)
            {
                return NotFound();
            }
            await _itemsRepository.DeleteAsync(id);
            return NoContent();
        }

    }
}