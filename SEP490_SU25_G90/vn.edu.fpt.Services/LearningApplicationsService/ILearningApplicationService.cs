﻿using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using System.Linq.Expressions;

namespace SEP490_SU25_G90.vn.edu.fpt.Services.LearningApplicationsService
{
    public interface ILearningApplicationService
    {
        public List<LearningApplication> GetAll();
        Task<List<LearningApplicationsResponse>> GetAllAsync(string? searchString = null);
        Task<List<LearnerSummaryResponse>> GetLearnerSummariesAsync(string? searchString = null);
        Task<LearningApplicationsResponse?> GetDetailAsync(int id);
        Task<List<LearningApplicationsResponse>> FindByCCCD(string cccd, Expression<Func<LearningApplication, bool>>? additional = null);
        Task AddAsync(LearningApplication entity);
        Task<LearningApplicationsResponse?> FindLearnerByCccdAsync(string cccd);
    }
}

