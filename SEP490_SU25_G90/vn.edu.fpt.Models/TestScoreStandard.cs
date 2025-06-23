using System;
using System.Collections.Generic;

namespace SEP490_SU25_G90.vn.edu.fpt.Models;

public partial class TestScoreStandard
{
    public int Id { get; set; }

    public byte LicenceTypeId { get; set; }

    public string PartName { get; set; } = null!;

    public int MaxScore { get; set; }

    public int PassScore { get; set; }

    public virtual LicenceType LicenceType { get; set; } = null!;
}
