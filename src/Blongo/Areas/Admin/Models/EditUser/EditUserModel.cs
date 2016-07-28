using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace Blongo.Areas.Admin.Models.EditUser
{
    public class EditUserModel
    {
        [Required]
        public string EmailAddress { get; set; }

        public ObjectId Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Role { get; set; }
    }
}
