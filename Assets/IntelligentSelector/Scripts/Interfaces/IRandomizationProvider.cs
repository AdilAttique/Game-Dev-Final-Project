using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.IntelligentSelector.Scripts.Interfaces
{
    public interface IRandomizationProvider
    {
       /// <summary>
       /// Generates the random value
       /// </summary>
       /// <returns>float</returns>
       float GetRandomValue();

        /// <summary>
        /// Seeds the provider for randomization
        /// </summary>
        /// <param name="seed">seed value</param>
        void SeedProvider(int seed);

    }
}
