using Assets.IntelligentSelector.Scripts.Interfaces;

namespace Assets.IntelligentSelector.Scripts.Entities
{
    public class Option<T> : IOption<T>
    {
        /// <summary>
        /// Option to create this instance
        /// </summary>
        /// <param name="id">id of the new instance</param>
        /// <param name="weight">the weight of the instance</param>
        /// <param name="value">the value of the instance</param>
        public Option(int id, int weight, T value)
        {
            //create this instance
            Create(id,
                weight,
                value);
        }

        /// <summary>
        /// Method to create this instance
        /// </summary>
        /// <param name="id">id of the new instance</param>
        /// <param name="weight">the weight of the instance</param>
        /// <param name="value">the value of the instance</param>
        public void Create(int id, int weight, T value)
        {
            Id = id;
            Value = value;
            Weight = weight;
        }

        /// <summary>
        /// Property to get or set the adjusted weight
        /// </summary>
        public float AdjustedWeight { get; set; }

        /// <summary>
        /// Property to get or set the appearance probability
        /// </summary>
        public float AppearanceProbability { get; set; }

        /// <summary>
        /// Property to get or set the normalized weight
        /// </summary>
        public float ProbabilityDifference { get; set; }

        /// <summary>
        /// Property to get or set the probability difference
        /// </summary>
        public float NormalizedWeight { get; set; }

        /// <summary>
        /// Property to get or set the id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Property to get or set the weight
        /// </summary>
        public int Weight { get; set; }

        /// <summary>
        /// Property to get or set the total selection
        /// </summary>
        public int TotalSelection { get; set; }

        /// <summary>
        /// Property to get or set the value
        /// </summary>
        public T Value { get; set; }

    }
}
