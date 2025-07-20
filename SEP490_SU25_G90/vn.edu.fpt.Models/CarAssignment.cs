using System;
using System.Collections.Generic;

namespace SEP490_SU25_G90.vn.edu.fpt.Models;

public partial class CarAssignment
{
    public int AssignmentId { get; set; }

    public int CarId { get; set; }

    public int InstructorId { get; set; }

    public int SlotId { get; set; }

    public DateOnly? ScheduleDate { get; set; }

    public bool? CarStatus { get; set; }

    public virtual Car Car { get; set; } = null!;

    public virtual User Instructor { get; set; } = null!;

    public virtual ScheduleSlot Slot { get; set; } = null!;
}
