
namespace Assets.IntelligentSelector.Scripts.Interfaces
{
    public interface IOption<T>
    {
        #region Methods

        /// <summary>
        /// Creates an instance of this type
        /// </summary>
        /// <param name="id">the id of this instance</param>
        /// <param name="weight">the weight of the new instance</param>
        /// <param name="value">the value of this instance</param>
        void Create(int id, int weight, T value);

        #endregion

        #region Properties

        /// <summary>
        /// Property to get or set the adjusted weight
        /// </summary>
        float AdjustedWeight { get; set; }

        /// <summary>
        /// Property to get or set the appearance probability
        /// </summary>
        float AppearanceProbability { get; set; }

        /// <summary>
        /// Property to get or set the normalized weight
        /// </summary>
        float NormalizedWeight { get; set; }

        /// <summary>
        /// Property to get or set the probability difference
        /// </summary>
        float ProbabilityDifference { get; set; }

        /// <summary>
        /// Property to get or set the id
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// Property to get or set the weight
        /// </summary>
        int Weight { get; set; }

        /// <summary>
        /// Property to get or set the total selection
        /// </summary>
        int TotalSelection { get; set; }

        /// <summary>
        /// Property to get or set the value
        /// </summary>
        T Value { get; set; }

        #endregion
    }
}
