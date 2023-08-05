using System;
using System.Collections.Generic;
using System.Linq;
using PersonalWebsite.Models.Filters;
using System.Reflection;

public interface IFilterDiscoveryService
{
    public IEnumerable<Type> GetAllFilterImplementations();
    public Type GetFilterTypeByName(string filterName);
}

public class FilterDiscoveryService : IFilterDiscoveryService
{
    public IEnumerable<Type> GetAllFilterImplementations()
    {
        // find all types that implement IFilter
        Assembly assembly = Assembly.GetExecutingAssembly();
        IEnumerable<Type> filterImplementations = assembly.GetTypes()
            .Where(type => typeof(IFilter).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract);

        return filterImplementations;
    }

    public Type GetFilterTypeByName(string filterName)
    {
        // find all types that implement IFilter
        Assembly assembly = Assembly.GetExecutingAssembly();
        IEnumerable<Type> filterImplementations = assembly.GetTypes()
            .Where(type => typeof(IFilter).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract);

        // find the type that matches the filter name
        Type filterType = filterImplementations.FirstOrDefault(type => type.Name == filterName);

        return filterType;
    }
}