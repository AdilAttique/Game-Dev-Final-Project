using Assets.IntelligentSelector.Scripts.Enums;
using System.Collections.Generic;

namespace Assets.IntelligentSelector.Scripts.Interfaces
{
    public interface ISelector<T>
    {
        /// <summary>
        /// Property to get or set is adjusted
        /// </summary>
        bool IsAdjusted { get; set; }

        /// <summary>
        /// Property to get or set the total weight
        /// </summary>
        float TotalWeight { get; set; }

        /// <summary>
        /// Property to get or set total iterations
        /// </summary>
        int TotalIterations { get; set; }

        /// <summary>
        /// Property to get or set the provider
        /// </summary>
        IRandomizationProvider Provider { get; set; }

        /// <summary>
        /// Property to get or set the selection type
        /// </summary>
        SelectionAlgorithm SelectionAlgorithm { get; }

        /// <summary>
        /// Property to get or set options list
        /// </summary>
        List<IOption<T>> Options { get; set; }

        /// <summary>
        /// Resets the selector to initial point
        /// </summary>
        void ResetSelector();

        /// <summary>
        /// Prepares the options to be ready for selection
        /// </summary>
        void PrepareOptions();

        /// <summary>
        /// Removes an option form the options list
        /// </summary>
        /// <param name="value">value of option to remove</param>
        void RemoveOption(T value);

        /// <summary>
        /// Adds an option to the options list
        /// </summary>
        /// <param name="id">unique id of the option</param>
        /// <param name="weight">weight of the option</param>
        /// <param name="value">value of the option</param>
        void AddOption(int id, int weight, T value);

        /// <summary>
        /// Sets the selection type
        /// </summary>
        /// <param name="type"></param>
        void SetSelectionType(SelectionAlgorithm type);

        /// <summary>
        /// Updates an option
        /// </summary>
        /// <param name="id">id of the option to find</param>
        /// <param name="weight">the new weight</param>
        void UpdateOption(int id, int weight);

        /// <summary>
        /// Updates an option
        /// </summary>
        /// <param name="id">id of the option to find</param>
        /// <param name="value">the new value</param>
        void UpdateOption(int id, T value);

        /// <summary>
        /// Updates an option
        /// </summary>
        /// <param name="id">id of the option to find</param>
        /// <param name="weight">the new weight</param>
        /// <param name="value">the new value</param>
        void UpdateOption(int id, int weight, T value);

        /// <summary>
        /// Gets the next option by using the deterministic approach
        /// </summary>
        /// <returns>T</returns>
        T DeterministicGetNext();

        /// <summary>
        /// Gets the next option by using the either the deterministic approach
        /// or the random approach
        /// </summary>
        /// <returns>T</returns>
        T GetNext();

        /// <summary>
        /// Gets the next option by using the randomly approach
        /// </summary>
        /// <returns>T</returns>
        T RandomlyGetNext();
    }
}
