using CommerceCore.Application.Constant;
using CommerceCore.Domain.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceCore.Domain.Entities
{
    public class Shop
    {
        [BsonId]
        [BsonElement(elementName: "_id")]
        public ObjectId Id { get; set; }
        [BsonRequired]
        [MaxLength(150)]
        public string Name { get; set; } = "";
        [BsonRequired]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = "";
        [BsonRequired]
        [DataType(DataType.Password)]
        public string Password { get; set; } = "";
        [EnumDataType(typeof(StatusEnum))]
        public string Status { get; set; } = "Inactive";
        public bool Verify { get; set; } = false;
        [BsonElement]
        public IList<string> Role {  get; set; } = new List<string>();

    }
}
