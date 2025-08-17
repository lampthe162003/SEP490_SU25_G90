using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SEP490_SU25_G90.vn.edu.fpt.Models;

public partial class Cccd
{
    public int CccdId { get; set; }

    [StringLength(12, ErrorMessage = "Số CCCD phải có 12 chữ số")]
    [Required(ErrorMessage = "Số CCCD không được để trống")]
    public string CccdNumber { get; set; } = null!;

    public string? ImageMt { get; set; }

    public string? ImageMs { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
