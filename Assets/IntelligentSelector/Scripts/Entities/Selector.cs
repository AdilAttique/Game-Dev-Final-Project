using Assets.IntelligentSelector.Scripts.Enums;
using Assets.IntelligentSelector.Scripts.Interfaces;
using Assets.IntelligentSelector.Scripts.RandomizationProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.IntelligentSelector.Scripts.Entities
{
    public class Selector<T> : ISelector<T>
    {
        /// <summary>
        /// Property to get or set is adjusted
        /// </summary>
        public bool IsAdjusted { get; set; }

        /// <summary>
        /// Property to get or set the total weight
        /// </summary>
        public float TotalWeight { get; set; }

        /// <summary>
        /// Property to get or set total iterations
        /// </summary>
        public int TotalIterations { get; set; }

        /// <summary>
        /// Property to get or set the provider
        /// </summary>
        public IRandomizationProvider Provider { get; set; }

        /// <summary>
        /// Property to get or set the selection type
        /// </summary>
        public SelectionAlgorithm SelectionAlgorithm { get; private set; }

        /// <summary>
        /// Property to get or set options list
        /// </summary>
        public List<IOption<T>> Options { get; set; }

        /// <summary>
        /// Resets the selector to initial point
        /// </summary>
        public Selector(IRandomizationProvider provider, SelectionAlgorithm selectionType, List<IOption<T>> options)
        {
            //init some attributes
            SelectionAlgorithm = selectionType;
            Provider = provider;
            Options = options;
        }

        ///=================================================================================================
        /// <summary>   Adjust options. </summary>
        ///
        /// <remarks>   Andyblem, 16/2/2019. </remarks>
        ///=================================================================================================

        public void AdjustOptions()
        {
            //loop through the options and normalize the weight of each child
            for(int i = 0; i < Options.Count; i++)
            {
                if (i == 0)
                    Options[i].AdjustedWeight = Options[i].NormalizedWeight;
                else
                    Options[i].AdjustedWeight = Options[i - 1].AdjustedWeight + Options[i].NormalizedWeight;
            }
        }

        /// <summary>
        /// Resets the selector to initial point
        /// Note-deletes all the options, you will need to add the options
        /// </summary>
        public void ResetSelector()
        {
            //reset everything
            IsAdjusted = false;
            Options.Clear();
            TotalWeight = 0f;
        }

        ///=================================================================================================
        /// <summary>   Order options by weight in ascending order. </summary>
        ///
        /// <remarks>   Andyblem, 16/2/2019. </remarks>
        ///=================================================================================================

        public void OrderOptionsByWeightAscending()
        {
            //order the options according to normalized weight
            Options = Options.OrderBy(option => option.NormalizedWeight).ToList();
        }

        ///=================================================================================================
        /// <summary>   Normalize options. </summary>
        ///
        /// <remarks>   Andyblem, 16/2/2019. </remarks>
        ///=================================================================================================

        public void NormalizeOptions()
        {
            //find the total weight
            Options.ForEach(o => TotalWeight += o.Weight);

            //set the normalized weight for each option
            Options.ForEach(option => option.NormalizedWeight = option.Weight / TotalWeight);
        }

        /// <summary>
        /// Prepares the options to be ready for selection
        /// </summary>
        public void PrepareOptions()
        {
            //return if adjusted
            if (IsAdjusted)
                return;

            //normalize the weights of the options
            NormalizeOptions();

            //order options by weight in ascending order
            OrderOptionsByWeightAscending();

            //Adjust the weights
            AdjustOptions();

            //set to adjusted
            IsAdjusted = true;
        }

        /// <summary>
        /// Removes an option form the options list
        /// </summary>
        /// <param name="value">value of option to remove</param>
        public void RemoveOption(T value)
        {
            //check if option with this value already exists
            IOption<T> exists = Options.FirstOrDefault(option => object.Equals(option.Value, value));

            if(exists!= null)
            {
                //set adjusted to false
                IsAdjusted = false;

                //remove the option from the collection
                Options.Remove(exists);
            }
        }

        /// <summary>
        /// Adds an option to the options list
        /// </summary>
        /// <param name="id">unique id of the option</param>
        /// <param name="weight">weight of the option</param>
        /// <param name="value">value of the option</param>
        public void AddOption(int id, int weight, T value)
        {
            //make sure the passed object has a weight
            if (weight <= 0)
                throw new ArgumentException("Weight cannot have a chance <= 0%.");

            //option exists
            bool exists = Options.Any(option => option.Id == id);
            if(exists)
                throw new ArgumentException(String.Format("Option with this id already{0} exists", id));

            //add the option
            IOption<T> newOption = new Option<T>(id, weight, value);
            Options.Add(newOption);

            //adjust stuff
            IsAdjusted = false;
        }

        /// <summary>
        /// Sets the selection type
        /// </summary>
        /// <param name="type"></param>
        public void SetSelectionType(SelectionAlgorithm type)
        {
            SelectionAlgorithm = type;
        }

        /// <summary>
        /// Updates an option
        /// </summary>
        /// <param name="id">id of the option to find</param>
        /// <param name="weight">the new weight</param>
        public void UpdateOption(int id, int weight)
        {
            //make sure the passed object has a weight
            if (weight <= 0)
                throw new ArgumentException("weight cannot have a chance <= 0%.");

            IOption<T> option = Options.FirstOrDefault(op => op.Id == id);
            if (option != null)
            {
                //update the option
                option.Weight = weight;

                //recaluculate weights
                IsAdjusted = false;
            }
        }

        /// <summary>
        /// Updates an option
        /// </summary>
        /// <param name="id">id of the option to find</param>
        /// <param name="value">the new value</param>
        public void UpdateOption(int id, T value)
        {
            //make sure the passed object has a weight
            if (value == null)
                throw new ArgumentException("The value can't be null");

            IOption<T> option = Options.FirstOrDefault(op => op.Id == id);
            if (option != null)
            {
                //update the option
                option.Value = value;

                //recaluculate weights
                IsAdjusted = false;
            }
        }

        /// <summary>
        /// Updates an option
        /// </summary>
        /// <param name="id">id of the option to find</param>
        /// <param name="weight">the new weight</param>
        /// <param name="value">the new value</param>
        public void UpdateOption(int id, int weight, T value)
        {
            //make sure the passed object has a weight
            if (weight <= 0)
                throw new ArgumentException("weight cannot have a chance <= 0%.");

            IOption<T> option = Options.FirstOrDefault(op => op.Id == id);
            if(option != null)
            {
                //update the option
                option.Value = value;
                option.Weight = weight;

                //recaluculate weights
                IsAdjusted = false;
            }
        }

        /// <summary>
        /// Gets the next option by using the deterministic approach
        /// </summary>
        /// <returns>T</returns>
        public T DeterministicGetNext()
        {
            //adjust weights if not adjusted
            if (!IsAdjusted)
            {
                IsAdjusted = true;
                NormalizeOptions();
            }

            //order the items with difference to appearance probality
            Options.ForEach(o => o.ProbabilityDifference = Mathf.Clamp(o.NormalizedWeight - o.AppearanceProbability, 0f, o.NormalizedWeight));
            Options = Options.OrderByDescending(o => o.ProbabilityDifference).ToList();

            //get the top most element
            int value = Random.Range(0, Options.Count);
            var item = Options[0];

            //increment the number of iterations
            ++TotalIterations;

            //update some data on the item
            item.TotalSelection += 1;
            Options.ForEach(o => o.AppearanceProbability = ((float)o.TotalSelection / (float)TotalIterations));

            //return the item
            return item.Value;
        }

        /// <summary>
        /// Gets the next option by using the either the deterministic approach
        /// or the random approach
        /// </summary>
        /// <returns>T</returns>
        public T GetNext()
        {
            if (SelectionAlgorithm == SelectionAlgorithm.DetermenisticProbability)
                return DeterministicGetNext();
            else
                return RandomlyGetNext();
        }

        /// <summary>
        /// Gets the next option by using the randomly approach
        /// </summary>
        /// <returns>T</returns>
        public T RandomlyGetNext()
        {
            //adjust weights if not adjusted
            if (!IsAdjusted)
                PrepareOptions();

            //get the random number
            float number = Provider.GetRandomValue();

            //get the first option with a probability less to the random value
            var item = Options.FirstOrDefault(o => number <= o.AdjustedWeight);

            //return the value of the selected item
            return item.Value;
        }
    }
}
