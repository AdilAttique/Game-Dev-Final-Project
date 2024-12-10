using Assets.IntelligentSelector.Scripts.Entities;
using Assets.IntelligentSelector.Scripts.Enums;
using Assets.IntelligentSelector.Scripts.Interfaces;
using Assets.IntelligentSelector.Scripts.RandomizationProviders;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.IntelligentSelector.Demo.Scripts
{
    public class IntelligentSelectorTest : MonoBehaviour
    {
        //create an instance of the selector
        IRandomizationProvider _provider;
        Selector<int> selector;
        List<IOption<int>> _options;

        private void Start()
        {
            //create the options
            _options = new List<IOption<int>>()
                {
                    new Option<int>(0, 5, 1),
                    new Option<int>(1, 4, 2),
                    new Option<int>(2, 6, 3)
                };

            //create our enties
            _provider = new UnityRandomizationProvider();
            selector = new Selector<int>(_provider,
                                SelectionAlgorithm.DetermenisticProbability,
                                _options);

            //add options
            SimulateSelection();

            //reset stuff and set new selection type
            selector.ResetSelector();
            selector.SetSelectionType(SelectionAlgorithm.RandomProbability);

            //run the simulation selection
            AddOptions();
            SimulateSelection();
        }

        ///=================================================================================================
        /// <summary>   Adds options. </summary>
        ///
        /// <remarks>   Andyblem, 16/2/2019. </remarks>
        ///=================================================================================================

        public void AddOptions()
        {
            selector.AddOption(0, 5, 1);
            selector.AddOption(1, 4, 2);
            selector.AddOption(2, 6, 3);
        }

        ///=================================================================================================
        /// <summary>   Simulate selection. </summary>
        ///
        /// <remarks>   Andyblem, 16/2/2019. </remarks>
        ///=================================================================================================

        public void SimulateSelection()
        {
            //init stuff
            int runs = 15;
            int value = 0;
            int optionOneSelectionCount = 0;
            int optionTwoSelectionCount = 0;
            int optionThreeSelectionCount = 0;

            //run selections
            for (int i = 0; i < runs; i++)
            {
                value = selector.GetNext();
                if (value == 1)
                    ++optionOneSelectionCount;
                else if (value == 2)
                    ++optionTwoSelectionCount;
                else if (value == 3)
                    ++optionThreeSelectionCount;
            }

            //print out info
            string message = string.Format("One:{0}, Two:{1}, Three:{2}", optionOneSelectionCount, optionTwoSelectionCount, optionThreeSelectionCount);
            Debug.Log(message);
        }
    }
}
