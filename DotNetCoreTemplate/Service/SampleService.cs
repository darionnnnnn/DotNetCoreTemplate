using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCoreTemplate.Service
{
    public class SampleService : ISampleTransient, ISampleScoped, ISampleSingleton
    {
        private static int _counter { get; set; }
        private int _value { get; }

        public SampleService()
        {
            _value = ++_counter;
        }

        public int GetSomething()
        {
            return _value;
        }
    }
}
