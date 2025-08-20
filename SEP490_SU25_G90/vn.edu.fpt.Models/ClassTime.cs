using System;
using System.Collections.Generic;

namespace SEP490_SU25_G90.vn.edu.fpt.Models;

public partial class ClassTime
{
    public int ClassTimeId { get; set; }

    public int ClassId { get; set; }

    public byte Thu { get; set; }

    public int SlotId { get; set; }

    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    public virtual Class Class { get; set; } = null!;

    public virtual ScheduleSlot Slot { get; set; } = null!;
}