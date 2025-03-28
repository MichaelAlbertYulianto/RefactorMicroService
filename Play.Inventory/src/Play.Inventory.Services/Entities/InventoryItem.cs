using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using Play.Common.Entities;

namespace Play.Inventory.Services.Entities
{
    public class InventoryItem : IEntity
    {
        // [BsonId]
        // [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public Guid Id { get; init; }
        public Guid UserId { get; set; }
        public Guid CatalogItemId { get; set; }
        public int Quantity { get; set; }
        public DateTimeOffset AcquiredDate { get; set; }
    }
}