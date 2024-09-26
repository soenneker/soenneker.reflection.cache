using BenchmarkDotNet.Attributes;
using Soenneker.Reflection.Cache.Attributes;
using Soenneker.Reflection.Cache.Extensions;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Tests.Benchmarking.Benchmarks.Attributes
{
    public class CachedAttributesExtensionBenchmarks
    {
        private CachedAttribute[] _cachedAttributes;

        [Params(10, 100, 1000, 10000)]
        public int ArraySize;

        [GlobalSetup]
        public void Setup()
        {
            _cachedAttributes = new CachedAttribute[ArraySize];

            CachedTypes cachedTypes = new CachedTypes();

            for (int i = 0; i < ArraySize; i++)
            {
                _cachedAttributes[i] = new CachedAttribute(new object(), cachedTypes, true);
            }
        }

        [Benchmark(Baseline = true)]
        public object[] ToObjects()
        {
            return _cachedAttributes.ToObjects();
        }
    }
}
