using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CommerceCore.Domain.Entities
{
    public class Keys
    {
        [BsonRequired]
        public ObjectId UserID { get; set; }
        [BsonRequired]
        public string PublicKey { get; set; } = "";
        [BsonIgnore]
        public string AccessToken { get; set; } = "";
        [BsonElement]
        public ICollection<string> RefreshToken { get; set; } = new List<string>();

        public void SetTokenInList(string token) => RefreshToken.Add(token);
    }
}
