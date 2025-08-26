using Microsoft.EntityFrameworkCore;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.vn.edu.fpt.Services.AddressService
{
    public class AddressService : IAddressService
    {
        private readonly Sep490Su25G90DbContext _context;

        public AddressService(Sep490Su25G90DbContext context)
        {
            _context = context;
        }

        public List<CityResponse> GetAllCities()
        {
            return _context.Cities
                .OrderBy(c => c.CityName)
                .Select(c => new CityResponse
                {
                    CityId = c.CityId,
                    CityName = c.CityName
                })
                .ToList();
        }

        public List<ProvinceResponse> GetProvincesByCity(int cityId)
        {
            return _context.Provinces
                .Where(p => p.CityId == cityId)
                .OrderBy(p => p.ProvinceName)
                .Select(p => new ProvinceResponse
                {
                    ProvinceId = p.ProvinceId,
                    ProvinceName = p.ProvinceName,
                    CityId = p.CityId
                })
                .ToList();
        }

        public List<WardResponse> GetWardsByProvince(int provinceId)
        {
            return _context.Wards
                .Where(w => w.ProvinceId == provinceId)
                .OrderBy(w => w.WardName)
                .Select(w => new WardResponse
                {
                    WardId = w.WardId,
                    WardName = w.WardName,
                    ProvinceId = w.ProvinceId
                })
                .ToList();
        }

        public List<RoadResponse> GetRoadsByWard(int wardId)
        {
            var roads = _context.Addresses
                .Where(a => a.WardId == wardId && !string.IsNullOrEmpty(a.RoadName))
                .Select(a => a.RoadName)
                .Distinct()
                .OrderBy(r => r)
                .Select((roadName, index) => new RoadResponse
                {
                    RoadId = index + 1,
                    RoadName = roadName,
                    WardId = wardId
                })
                .ToList();

            return roads;
        }

        public async Task<int> CreateAddressAsync(int wardId, string? houseNumber = null, string? roadName = null)
        {
            var address = new Address
            {
                WardId = wardId,
                HouseNumber = houseNumber,
                RoadName = roadName,
            };

            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();

            return address.AddressId;
        }

        public async Task<Address?> GetAddressAsync(int addressId)
        {
            return await _context.Addresses
                .Include(a => a.Ward)
                    .ThenInclude(w => w.Province)
                        .ThenInclude(p => p.City)
                .FirstOrDefaultAsync(a => a.AddressId == addressId);
        }

        public async Task UpdateAddressAsync(int addressId, int wardId, string? houseNumber = null, string? roadName = null)
        {
            var address = await _context.Addresses.FindAsync(addressId);
            if (address != null)
            {
                address.WardId = wardId;
                address.HouseNumber = houseNumber;
                address.RoadName = roadName;

                await _context.SaveChangesAsync();
            }
        }
    }
}