using System;
using System.Collections.Generic;

namespace SEP490_SU25_G90.vn.edu.fpt.Models;

public partial class MockTestQuestion
{
    public int QuestionId { get; set; }

    public byte? TestType { get; set; }

    public string? QuestionText { get; set; }

    public string? OptionA { get; set; }

    public string? OptionB { get; set; }

    public string? OptionC { get; set; }

    public string? OptionD { get; set; }

    public string? CorrectOption { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool? ReadyStatus { get; set; }

    public virtual LicenceType? TestTypeNavigation { get; set; }
}
