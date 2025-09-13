using CWB.CompanySettings.ViewModels.Location;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CWB.CompanySettings.Services.Location
{
    public interface IPlantService
    {
        IEnumerable<PlantVM> GetPlants(long TenantId);
        Task<PlantVM> Plant(PlantVM plantVM);
        bool CheckPlantExisit(CheckPlantVM checkPlantVM);

        Task<PlantVM> GetPlant(long plantId);
        Task<IEnumerable<PlantVM>> GetPlantsWithWorkDetails(long plantId);
        Task<bool> DelPlant(long plantId);
        Task<bool> DelHoliday(long holidayId);
        IEnumerable<HolidayVM> Holidays(long plantId);
        Task<HolidayVM> PostHoliday(HolidayVM holidayVM);
        Task<PlantWorkingDetailsVM> PostPlantWD(PlantWorkingDetailsVM plantWd);
        Task<CityVM> PostCity(CityVM plantWd);
        Task<CountryVM> PostCountry(CountryVM plantWd);
        IEnumerable<CityVM> GetCitys(long TenantId);
        IEnumerable<SectionsVM> GetSections(long TenantId);
        Task<SectionsVM> PostSections(SectionsVM plantWdVM);
        Task<IEnumerable<NoOfDaysTimeSlotVM>> GetNoOfDaysTimeSlots();
        Task<IEnumerable<TimeSlotDurationVM>> GetTimeSlotDurations();
        IEnumerable<CountryVM> GetCountrys(long TenantId);
        Task<bool> CheckCity(string city);
        Task<bool> CheckSections(string city);
        Task<bool> CheckCountry(string country);
        Task<bool> DelSections(long designationId);

        Task<PlantWorkingDetailsVM> GetPlantWD(long tenantId,long plantId);
    }
}
