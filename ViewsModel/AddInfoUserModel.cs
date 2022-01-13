using System;
using System.ComponentModel.DataAnnotations;

namespace SocialNetworkOnSharp.ViewsModel
{
    public class AddInfoUserModel
    {
        [Required(ErrorMessage = "Id обязателен")]
        public int Id { get; set; }
        [MaxLength(1000, ErrorMessage = "Размер истории превышен")]
        public String MyHistory { get; set; }
        public bool UseMyHistory { get; set; }
        public bool UserAddInfo { get; set; }
    }
}
