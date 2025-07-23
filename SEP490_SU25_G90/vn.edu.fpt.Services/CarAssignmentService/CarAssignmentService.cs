using Microsoft.AspNetCore.Mvc.Rendering;
using SEP490_SU25_G90.vn.edu.fpt.Repositories.InstructorRepository;
﻿using AutoMapper;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects.CarAssignment;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Repositories.CarAssignmentRepository;
using System.Threading.Tasks;

namespace SEP490_SU25_G90.vn.edu.fpt.Services.CarAssignmentService
{
    public class CarAssignmentService : ICarAssignmentService
    {
        private readonly ICarAssignmentRepository _carAssignmentRepository;
        private readonly IInstructorRepository _instructorRepository;
        private readonly IMapper _mapper;

        public CarAssignmentService(ICarAssignmentRepository carAssignmentRepository, IInstructorRepository instructorRepository, IMapper mapper)
        {
            _carAssignmentRepository = carAssignmentRepository;
            _instructorRepository = instructorRepository;
            _mapper = mapper;
        }

        public async Task<List<CarAssignmentResponse>> GetAllCarAssignmentsAsync()
        {
            var carAssignments = await _carAssignmentRepository.GetAllCarAssignmentsAsync();
            return MapToResponseList(carAssignments);
        }

        public async Task<List<CarAssignmentResponse>> GetAllCarsWithAssignmentsAsync()
        {
            var carAssignments = await _carAssignmentRepository.GetAllCarsWithAssignmentsAsync();
            return MapToResponseList(carAssignments);
        }

        public async Task<List<CarAssignmentResponse>> SearchCarAssignmentsAsync(CarAssignmentSearchRequest request, int? currentInstructorId = null)
        {
            var carAssignments = await _carAssignmentRepository.SearchCarAssignmentsAsync(
                request.CarMake, 
                request.ScheduleDate);
            
            var result = MapToResponseList(carAssignments);
            
            // Lọc thêm theo slot nếu có
            if (request.SlotId.HasValue)
            {
                result = result.Where(ca => ca.SlotId == request.SlotId.Value).ToList();
            }
            
            // Lọc theo trạng thái nếu có
            if (request.CarStatus.HasValue)
            {
                result = result.Where(ca => ca.CarStatus == request.CarStatus.Value).ToList();
            }
            
            // Lọc theo xe của user hiện tại nếu checkbox được chọn
            if (request.ShowMyReservationsOnly && currentInstructorId.HasValue)
            {
                result = result.Where(ca => 
                    ca.InstructorId == currentInstructorId.Value || 
                    ca.CarStatus == false // Hoặc xe trống (có thể mượn)
                ).ToList();
            }
            
            return result;
        }

        public async Task<CarAssignmentResponse?> GetCarAssignmentByIdAsync(int assignmentId)
        {
            var carAssignment = await _carAssignmentRepository.GetCarAssignmentByIdAsync(assignmentId);
            return carAssignment != null ? MapToResponse(carAssignment) : null;
        }

        public async Task<bool> RentCarAsync(CarRentalRequest request)
        {
            // Kiểm tra xe có khả dụng không
            if (!await IsCarAvailableAsync(request.CarId, request.ScheduleDate, request.SlotId))
            {
                return false;
            }


            //// Kiểm tra giáo viên có thể mượn xe không
            //if (!await CanInstructorRentCarAsync(request.InstructorId, request.ScheduleDate, request.SlotId))
            //{
            //    return false;
            //}
            var carAssignment = new CarAssignment

            {
                CarId = request.CarId,
                InstructorId = request.InstructorId,
                SlotId = request.SlotId,
                ScheduleDate = request.ScheduleDate,
                CarStatus = request.CarStatus
            };
            var result = await _carAssignmentRepository.CreateCarAssignmentAsync(carAssignment);
            return result != null;
        }

        public async Task<bool> ReturnCarAsync(int assignmentId)
        {
            var carAssignment = await _carAssignmentRepository.GetCarAssignmentByIdAsync(assignmentId);
            if (carAssignment == null)
                return false;

            carAssignment.CarStatus = false; // Trả xe thì status = 0
            return await _carAssignmentRepository.UpdateCarAssignmentAsync(carAssignment);
        }

        public async Task<bool> UpdateCarAssignmentAsync(int assignmentId, CarRentalRequest request)
        {
            var carAssignment = await _carAssignmentRepository.GetCarAssignmentByIdAsync(assignmentId);
            if (carAssignment == null)
                return false;

            // Kiểm tra xe mới có khả dụng không (nếu thay đổi xe, ngày hoặc ca)
            if (carAssignment.CarId != request.CarId || 
                carAssignment.ScheduleDate != request.ScheduleDate || 
                carAssignment.SlotId != request.SlotId)
            {
                if (!await IsCarAvailableAsync(request.CarId, request.ScheduleDate, request.SlotId))
                {
                    return false;
                }
            }

            carAssignment.CarId = request.CarId;
            carAssignment.InstructorId = request.InstructorId;
            carAssignment.SlotId = request.SlotId;
            carAssignment.ScheduleDate = request.ScheduleDate;
            carAssignment.CarStatus = request.CarStatus;

            return await _carAssignmentRepository.UpdateCarAssignmentAsync(carAssignment);
        }

        public async Task<bool> DeleteCarAssignmentAsync(int assignmentId)
        {
            return await _carAssignmentRepository.DeleteCarAssignmentAsync(assignmentId);
        }

        public async Task<List<SelectListItem>> GetCarMakesSelectListAsync()
        {
            var cars = await _carAssignmentRepository.GetAllCarsAsync();
            var carMakes = cars.Where(c => !string.IsNullOrEmpty(c.CarMake))
                              .Select(c => c.CarMake)
                              .Distinct()
                              .OrderBy(make => make)
                              .ToList();
            
            return carMakes.Select(make => new SelectListItem
            {
                Value = make,
                Text = make
            }).ToList();
        }

        public async Task<List<SelectListItem>> GetScheduleSlotsSelectListAsync()
        {
            var slots = await _carAssignmentRepository.GetAllScheduleSlotsAsync();
            return slots.Select(s => new SelectListItem
            {
                Value = s.SlotId.ToString(),
                Text = $"Ca {s.SlotId}: {s.StartTime?.ToString("HH:mm")} - {s.EndTime?.ToString("HH:mm")}"
            }).ToList();
        }

        public async Task<List<SelectListItem>> GetAvailableCarsSelectListAsync(DateOnly date, int slotId)
        {
            var availableCars = await _carAssignmentRepository.GetAvailableCarsAsync(date, slotId);
            return availableCars.Select(c => new SelectListItem
            {
                Value = c.CarId.ToString(),
                Text = $"{c.LicensePlate} - {c.CarMake} {c.CarModel}"
            }).ToList();
        }

        public async Task<List<SelectListItem>> GetInstructorsSelectListAsync()
        {
            var instructors = _instructorRepository.GetAllInstructors().ToList();
            return instructors.Select(i => new SelectListItem
            {
                Value = i.UserId.ToString(),
                Text = $"{i.FirstName} {i.MiddleName} {i.LastName}".Trim()
            }).ToList();
        }

        public async Task<bool> IsCarAvailableAsync(int carId, DateOnly date, int slotId)
        {
            return await _carAssignmentRepository.IsCarAvailableAsync(carId, date, slotId);
        }

        public async Task<bool> CanInstructorRentCarAsync(int instructorId, DateOnly date, int slotId)
        {
            // Kiểm tra giáo viên đã có xe mượn trong cùng slot và ngày chưa
            var instructorAssignments = await _carAssignmentRepository.GetCarAssignmentsByInstructorAsync(instructorId);
            return !instructorAssignments.Any(ca => ca.ScheduleDate == date && ca.SlotId == slotId && ca.CarStatus == true);
        }

        public async Task<List<CarAssignmentResponse>> GetCarAssignmentsByInstructorAsync(int instructorId, DateOnly date, int slotId)
        {
            var instructorAssignments = await _carAssignmentRepository.GetCarAssignmentsByInstructorAsync(instructorId);
            var filteredAssignments = instructorAssignments
                .Where(ca => ca.ScheduleDate == date && ca.SlotId == slotId && ca.CarStatus == true)
                .ToList();
            
            return MapToResponseList(filteredAssignments);
        }

        private List<CarAssignmentResponse> MapToResponseList(List<CarAssignment> carAssignments)
        {
            return carAssignments.Select(MapToResponse).ToList();
        }

        private CarAssignmentResponse MapToResponse(CarAssignment carAssignment)
        {
            var slotDisplayName = "";
            if (carAssignment.Slot != null)
            {
                slotDisplayName = $"Ca {carAssignment.SlotId}: {carAssignment.Slot.StartTime?.ToString("HH:mm")} - {carAssignment.Slot.EndTime?.ToString("HH:mm")}";
            }

            var response = new CarAssignmentResponse
            {
                AssignmentId = carAssignment.AssignmentId,
                CarId = carAssignment.CarId,
                LicensePlate = carAssignment.Car?.LicensePlate,
                CarMake = carAssignment.Car?.CarMake,
                CarModel = carAssignment.Car?.CarModel,
                InstructorId = carAssignment.InstructorId,
                InstructorFullName = GetInstructorFullName(carAssignment.Instructor),
                InstructorPhone = carAssignment.Instructor?.Phone,
                InstructorEmail = carAssignment.Instructor?.Email,
                SlotId = carAssignment.SlotId,
                StartTime = carAssignment.Slot?.StartTime,
                EndTime = carAssignment.Slot?.EndTime,
                SlotDisplayName = slotDisplayName,
                ScheduleDate = carAssignment.ScheduleDate,
                CarStatus = carAssignment.CarStatus,
                CarStatusDisplay = carAssignment.CarStatus == true ? "Đã được mượn" : "Xe trống",
                CanRent = carAssignment.CarStatus != true // Xe có thể mượn khi status != true
            };

            // Map instructor licence types cho xe đã có giáo viên
            if (carAssignment.Instructor?.InstructorSpecializations != null && carAssignment.Instructor.InstructorSpecializations.Any())
            {
                response.InstructorLicenceTypes = carAssignment.Instructor.InstructorSpecializations
                    .Where(ins => ins.LicenceType != null)
                    .Select(ins => new LicenceTypeInfo
                    {
                        LicenceTypeId = ins.LicenceTypeId ?? 0,
                        LicenceCode = ins.LicenceType?.LicenceCode
                    }).ToList();
            }

            return response;
        }

        private string GetInstructorFullName(Models.User? instructor)
        {
            if (instructor == null) return string.Empty;
            
            var fullName = $"{instructor.FirstName}";
            if (!string.IsNullOrEmpty(instructor.MiddleName))
                fullName += $" {instructor.MiddleName}";
            if (!string.IsNullOrEmpty(instructor.LastName))
                fullName += $" {instructor.LastName}";
                
            return fullName.Trim();
        }


        public async Task AddCarAssignment(CarAssignment carAssignment)
        {
            await _carAssignmentRepository.AddCarAssignmentAsync(carAssignment);
        }

        public async Task DeleteCarAssignment(CarAssignment carAssignment)
        {
            if (!await DoesCarAssignmentExist(carAssignment))
            {
                throw new NullReferenceException("Lịch mượn xe không tồn tại.");
            }
            await _carAssignmentRepository.DeleteCarAssignmentAsync(carAssignment);
        }

        public async Task<IList<CarAssignmentInformationResponse>> GetAllCarAssignments()
        {
            var assignments = await _carAssignmentRepository.GetAllCarAssignmentsAsync();
            foreach (var carAssignment in assignments)
            {

            }
            return _mapper.Map<IList<CarAssignmentInformationResponse>>(await _carAssignmentRepository.GetAllCarAssignmentsAsync());
        }

        public async Task<CarAssignmentInformationResponse> GetAssignmentById(int assignmentId)
        {
            return _mapper.Map<CarAssignmentInformationResponse>(await _carAssignmentRepository.GetAssignmentByIdAsync(assignmentId));
        }

        public async Task<IList<CarAssignmentInformationResponse>> GetAssignmentsByCarId(int carId)
        {
            return _mapper.Map<IList<CarAssignmentInformationResponse>>(await _carAssignmentRepository.GetAllAssignmentsByCarIdAsync(carId));
        }

        public async Task UpdateCarAssignment(CarAssignment carAssignment)
        {
            if (!await DoesCarAssignmentExist(carAssignment))
            {
                throw new NullReferenceException("Lịch mượn xe không tồn tại.");
            }
            await _carAssignmentRepository.UpdateCarAssignmentAsync(carAssignment);
        }

        private async Task<bool> DoesCarAssignmentExist(CarAssignment carAssignment)
        {
            return await _carAssignmentRepository.GetAssignmentByIdAsync(carAssignment.CarId) != null;
        }
    }
}

