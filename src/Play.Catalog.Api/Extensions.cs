using Play.Catalog.Contracts.Dtos;
using Play.Catalog.Data.Entities;

namespace Play.Catalog.Service
{
    public static class Extensions
    {
        public static ItemDto AsDto(this Item entity)
        {
            return new ItemDto(entity.Id, entity.Name, entity.Description, entity.Price, entity.CreatedDate);
        }
    }
}