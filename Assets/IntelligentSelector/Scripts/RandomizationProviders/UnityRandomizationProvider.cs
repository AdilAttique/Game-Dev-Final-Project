using Assets.IntelligentSelector.Scripts.Interfaces;
using UnityEngine;

namespace Assets.IntelligentSelector.Scripts.RandomizationProviders
{
    /// <summary>
    /// Provides the randomization service in Unity 3D
    /// </summary>
    public class UnityRandomizationProvider : IRandomizationProvider
    {
        /// <summary>
        /// Generates the random value
        /// </summary>
        /// <returns>float</returns>
        public float GetRandomValue()
        {
            return Random.Range(0f, 1f);
        }

        /// <summary>
        /// Seeds the provider for randomization
        /// </summary>
        /// <param name="seed">seed value</param>
        public void SeedProvider(int seed)
        {
            Random.InitState(seed);
        }
    }
}
