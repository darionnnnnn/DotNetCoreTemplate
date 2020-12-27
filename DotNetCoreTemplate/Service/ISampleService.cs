using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCoreTemplate.Service
{
    public interface ISampleService
    {
        int GetSomething();
    }

    public interface ISampleTransient : ISampleService
    {
    }

    public interface ISampleScoped : ISampleService
    {
    }

    public interface ISampleSingleton : ISampleService
    {
    }
}
