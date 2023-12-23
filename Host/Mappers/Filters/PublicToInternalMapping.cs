using OggettoCase.DataAccess.Filters;
using OggettoCase.DataContracts.Filters;
using OggettoCase.Mappers.Users;

namespace OggettoCase.Mappers.Filters;

public static class PublicToInternalMapping
{
    public static CalendarFilterInternal ToInternalFilter(this CalendarFilter filter)
    {
        return new CalendarFilterInternal
        {
            Title = filter.Title,
            OwnerName = filter.OwnerName,
            StartDate = filter.StartDate,
            EndDate = filter.EndDate,
            Category = filter.Category
        };
    }
    
    public static UserFilterInternal ToInternalFilter(this UserFilter filter)
    {
        return new UserFilterInternal
        {
            Name = filter.Name,
            Surname = filter.Surname,
            Email = filter.Email,
            ApprovedState = filter.ApprovedState,
            Role = filter.Role.ToEntity()
        };
    }
}