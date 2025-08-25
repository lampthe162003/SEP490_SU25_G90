using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.vn.edu.fpt.Services.AddressService
{
    public interface IAddressService
    {
        List<CityResponse> GetAllCities();
        List<ProvinceResponse> GetProvincesByCity(int cityId);
        List<WardResponse> GetWardsByProvince(int provinceId);
        List<RoadResponse> GetRoadsByWard(int wardId);
        Task<int> CreateAddressAsync(int wardId, string? houseNumber = null, string? roadName = null);
        Task<Address?> GetAddressAsync(int addressId);
        Task UpdateAddressAsync(int addressId, int wardId, string? houseNumber = null, string? roadName = null);
    }
}