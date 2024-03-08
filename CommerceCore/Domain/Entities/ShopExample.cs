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
    public class ShopExample
    {
        private readonly Dictionary<string, string> RoleDefine = new Dictionary<string, string>()
        {
            { "ADMIN","0x01" },
            { "WRITTER", "0x02"}, { "READER", "0x04"}, {"SHOP","0x08" }
        };
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
        public ICollection<string> Role {  get; set; } = new List<string>();

        public void SetRoleInList(string role) => Role.Add(RoleDefine[role]);
        public string GetRoleInList(string role) => RoleDefine[role];

    }
}
