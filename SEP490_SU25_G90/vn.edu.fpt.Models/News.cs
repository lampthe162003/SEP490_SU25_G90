using System;
using System.Collections.Generic;

namespace SEP490_SU25_G90.vn.edu.fpt.Models;

public partial class News
{
    public int NewsId { get; set; }

    public string? Title { get; set; }

    public string? NewsContent { get; set; }

    public int? AuthorId { get; set; }

    public DateTime? PostTime { get; set; }

    public string? Image { get; set; }

    public virtual User? Author { get; set; }
}
