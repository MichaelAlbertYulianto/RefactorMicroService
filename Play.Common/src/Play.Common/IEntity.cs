using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;


namespace Play.Common.Entities
{
    public interface IEntity
    {
        Guid Id { get; init; }
    }
}