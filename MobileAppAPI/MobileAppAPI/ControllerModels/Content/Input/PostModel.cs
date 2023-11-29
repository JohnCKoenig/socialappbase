using System.ComponentModel.DataAnnotations;

namespace MobileAppAPI.ControllerModels.Content.Input
{
    public class PostModel
    {
        [Required]
        public int UserId { get; set; }

        [MaxLength(100)]
        public string PostLocation { get; set; }

        [Required]
        public string PostText { get; set; }

        public byte[] PostImage { get; set; }
    }
}
