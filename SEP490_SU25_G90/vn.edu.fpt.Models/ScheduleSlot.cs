using System;
using System.Collections.Generic;

namespace SEP490_SU25_G90.vn.edu.fpt.Models;

public partial class ScheduleSlot
{
    public int SlotId { get; set; }

    public TimeOnly? StartTime { get; set; }

    public TimeOnly? EndTime { get; set; }

    public virtual ICollection<CarAssignment> CarAssignments { get; set; } = new List<CarAssignment>();

    public virtual ICollection<ClassSchedule> ClassSchedules { get; set; } = new List<ClassSchedule>();
}
