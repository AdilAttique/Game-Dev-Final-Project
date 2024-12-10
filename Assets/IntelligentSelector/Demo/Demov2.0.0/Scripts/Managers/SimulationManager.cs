using Assets.IntelligentSelector.Demo.Demov2._0._0.Scripts.Entities;
using Assets.IntelligentSelector.Scripts.Entities;
using Assets.IntelligentSelector.Scripts.Enums;
using Assets.IntelligentSelector.Scripts.Interfaces;
using Assets.IntelligentSelector.Scripts.RandomizationProviders;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.IntelligentSelector.Demo.Demov2._0._0.Scripts.Managers
{
    public class SimulationManager : MonoBehaviour
    {
        #region Inspector Variables

        /// <summary>
        /// A reference to the current selection text
        /// </summary>
        [SerializeField]
        Text _textCurrentSelection;

        /// <summary>
        /// A reference to the selection algorithm in use
        /// </summary>
        [SerializeField]
        Text _textSelectionAlgorithm;

        /// <summary>
        /// A reference to the total iterations count
        /// </summary>
        [SerializeField]
        Text _textTotalIterations;

        /// <summary>
        /// A reference to the default selection algorithm
        /// </summary>
        [SerializeField]
        SelectionAlgorithm _defaultSelectionAlgorithm;

        /// <summary>
        /// A reference to the Mono Options
        /// </summary>
        [SerializeField]
        List<OptionData> _monoOptionsData;

        #endregion

        #region Intelligent Selector Variables

        /// <summary>
        /// A reference to the randomization provider
        /// </summary>
        IRandomizationProvider _provider;

        /// <summary>
        /// A reference to the selector
        /// </summary>
        Selector<MonoOption> _selector;

        /// <summary>
        /// A reference to the options
        /// </summary>
        List<IOption<MonoOption>> _options = new List<IOption<MonoOption>>();

        #endregion

        #region Other Variables

        bool _isSimulating = false;

        int _prevSelection = 0;

        MonoOption _prevMonoOption;

        #endregion

        private void Awake()
        {
            //Create the provider
            _provider = new UnityRandomizationProvider();
            _provider.SeedProvider(DateTime.Now.Second);    //seed the provider with some random value to produce a more random result 

            //loop through the mono options array and create an option of each entity
            _monoOptionsData.ForEach(mO => _options.Add(new Option<MonoOption>(mO.Id,
                mO.Weight,
                mO.Value)));
        }

        /// <summary>
        /// Called every once when this instance is created
        /// </summary>
        private void Start()
        {
            //create the selector(nothing funcy, just seperating concerns
            _selector = new Selector<MonoOption>(_provider,
                _defaultSelectionAlgorithm,
                _options);
        }

        void Update()
        {
            if (_selector.SelectionAlgorithm == SelectionAlgorithm.DetermenisticProbability)
            {
                //print the algorithm in use
                _textSelectionAlgorithm.text = "Deterministic Probability";

                //print total iterations
                _textTotalIterations.text = _selector.TotalIterations.ToString();
            }
            else
            {
                //print the algorithm in use
                _textSelectionAlgorithm.text = "Random Probability";

                //print total iterations
                _textTotalIterations.text = "unavailable";
            }

            //print the name of the current selection
            if (_prevMonoOption == null)
                _textCurrentSelection.text = "unavailable";
            else
                _textCurrentSelection.text = _prevMonoOption.name;
        }

        /// <summary>
        /// Get the next option from the selector
        /// </summary>
        /// <returns></returns>
        private void GetNextOption()
        {
            //get the decision from the selector
            _prevMonoOption = _selector.GetNext();

            //set the select
            _prevMonoOption.OnSelect();
        }

        private void Reset()
        {
            if(_prevMonoOption != null)
                _prevMonoOption.OnDeSelect();
        }

        private IEnumerator SimulateConsideration()
        {
            //set i
            int i = 10;
           
            //loop a few times 
            do
            {
                //wait a few seconds and deconsider
                yield return new WaitForSeconds(0.3f);

                //start the consider coroutine
                StartCoroutine(Consider());

                //increment 
                --i;
            }
            while (i > 0);
        }

        /// <summary>
        /// Simulates the process of selection
        /// </summary>
        public void Simulate()
        {
            //return if is simulating
            if (_isSimulating)
                return;

            //simulation selection
            StartCoroutine(SimulateSelection());
        }

        /// <summary>
        /// Switches between the various selection algorithms
        /// </summary>
        public void SwitchAlgorithm()
        {
            if (_selector.SelectionAlgorithm == SelectionAlgorithm.DetermenisticProbability)
                _selector.SetSelectionType(SelectionAlgorithm.RandomProbability);
            else
                _selector.SetSelectionType(SelectionAlgorithm.DetermenisticProbability);
        }

        public IEnumerator SimulateSelection()
        {
            //reset
            Reset();

            //wait a bit before simulating consideration
            yield return new WaitForSeconds(0.55f);
            
            //simulation consideration
            StartCoroutine(SimulateConsideration());
            
            //wait a little bit before showing option
            yield return new WaitForSeconds(3.5f);

            //make choice
            GetNextOption();

            //reset is simulating
            _isSimulating = false;
        }

        public IEnumerator Consider()
        {
            //randomly select a number
            int selection = UnityEngine.Random.Range(0, _selector.Options.Count);

            //make sure we don't select the previous selection
            while(_prevSelection == selection)
            {
                selection = UnityEngine.Random.Range(0, _selector.Options.Count);
            }

            //record the previous selection
            _prevSelection = selection;

            //consider the current choice
            _selector.Options[selection].Value.OnConsider();

            //wait a few seconds and deconsider
            yield return new WaitForSeconds(0.25f);

            //unconsider the current choice
            _selector.Options[selection].Value.OnUnConsider();
        }
    }

    [Serializable]
    public struct OptionData
    {
        #region Properties

        /// <summary>
        /// A reference to the id of this instance
        /// </summary>
        [SerializeField]
        int _id;

        /// <summary>
        /// A reference to the weight of this instance
        /// </summary>
        [SerializeField]
        int _weight;

        /// <summary>
        /// A reference to the value of this instance
        /// </summary>
        [SerializeField]
        MonoOption _value;

        #endregion

        #region Properties

        /// <summary>
        /// Sets and gets the Id
        /// </summary>
        public int Id
        {
            get
            {
                return _id;
            }

            set
            {
                _id = value;
            }
        }

        /// <summary>
        /// Sets and gets the weight
        /// </summary>
        public int Weight
        {
            get
            {
                return _weight;
            }

            set
            {
                _weight = value;
            }
        }

        /// <summary>
        /// A reference to the value
        /// </summary>
        public MonoOption Value
        {
            get
            {
                return _value;
            }

            set
            {
                _value = value;
            }
        }

        #endregion
    }
}
