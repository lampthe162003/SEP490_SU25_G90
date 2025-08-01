﻿using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Repositories.ScheduleSlotRepository;

namespace SEP490_SU25_G90.vn.edu.fpt.Services.ScheduleSlotService
{
    public class ScheduleSlotService : IScheduleSlotService
    {
        private readonly IScheduleSlotRepository _scheduleSlotRepository;

        public ScheduleSlotService(IScheduleSlotRepository scheduleSlotRepository)
        {
            _scheduleSlotRepository = scheduleSlotRepository;
        }

        public async Task<IList<ScheduleSlot>> GetAllSlots()
        {
            return await _scheduleSlotRepository.GetAllSlots();
        }

        public async Task<ScheduleSlot> GetSlotById(int slotId)
        {
            return await _scheduleSlotRepository.GetSlotById(slotId);
        }
    }
}
