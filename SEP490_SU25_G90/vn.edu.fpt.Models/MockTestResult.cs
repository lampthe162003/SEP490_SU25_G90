using System;
using System.Collections.Generic;

namespace SEP490_SU25_G90.vn.edu.fpt.Models;

public partial class MockTestResult
{
    public int ResultId { get; set; }

    public int? UserId { get; set; }

    public DateTime? SubmittedAt { get; set; }

    public int? Score { get; set; }

    public virtual User? User { get; set; }
}
