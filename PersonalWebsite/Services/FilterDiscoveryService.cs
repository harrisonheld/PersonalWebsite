using System;
using System.Collections.Generic;
using System.Linq;
using PersonalWebsite.Filters;
using System.Reflection;

public interface IFilterDiscoveryService
{
    IEnumerable<Type> GetFilterImplementations();
}

public class FilterDiscoveryService : IFilterDiscoveryService
{
    public IEnumerable<Type> GetFilterImplementations()
    {
        // find all types that implement IFilter
        Assembly assembly = Assembly.GetExecutingAssembly();
        IEnumerable<Type> filterImplementations = assembly.GetTypes()
            .Where(type => typeof(IFilter).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract);

        return filterImplementations;
    }
}