using System;
using System.Collections.Generic;

namespace SEP490_SU25_G90.vn.edu.fpt.Models;

public partial class ClassSchedule
{
    public int ScheduleId { get; set; }

    public int? ClassId { get; set; }

    public int? SlotId { get; set; }

    public DateOnly? ScheduleDate { get; set; }

    public virtual Class? Class { get; set; }

    public virtual ScheduleSlot? Slot { get; set; }
}
