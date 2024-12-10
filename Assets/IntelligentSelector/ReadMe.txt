Intelligent Selector

Intro
------
Intelligent selector is a tool designed to ease the process of selecting an entity 
from a pool of objects by considering the weight assigned to the entity. It utilizes 
either of the two algorithms to select an entity. The two algorithms involved are 
(i) the random probability algorithm and (ii) the deterministic probability algorithm.

The deterministic algorithm considers weight only when selecting an entity from the 
pool. The entity which hasn't appeared for a while is the one which gets selected. 
Such an approach results in a similar sequence of selecting an entity and the entity 
being selected a particular times per number of tries. On the other hand, the 
random probability algorithm adds randomness when it comes to selecting an entity. 
Such an approach results in varied sequences of selection and doesn't guarantee an 
item being selected a particular number of times in a number of tries.

How to install
--------------
Simply import the asset into your project. You can place this asset in any location 
inside your project.

How to use
-----------
Import the asset into the script that needs functionality to select stuff

'using Assets.IntelligentSelector.Scripts.Entities;'

Create an instance of the selector. Set the algorithm to use in the constructor. Pass the required info through the constructor

 //create our entities
 IRandomizationProvider _provider = new UnityRandomizationProvider();
 List<IOption<int>>  = new List<IOption<int>>()
        {
            new Option<int>(0, 5, 1),
            new Option<int>(1, 4, 2),
            new Option<int>(2, 6, 3)
        };

'Selector<int> selector = new Selector<int>(_provider,
                                SelectionAlgorithm.DetermenisticProbability,
                                _options);'

or 'Selector<int> selector = new Selector<int>(_provider,
                                SelectionAlgorithm.RandomProbability,
                                _options);'

You can add new entities to the options pool as show below. Note the first parameter is the id(must be unique), the second parameter is the weight, 
the third parameter is the value.

'selector.AddOption(0, 5, 1);'
'selector.AddOption(1, 4, 2);'
'selector.AddOption(2, 6, 3);'

Select and retrieve the chosen entity

'int item = selector.GetNext();'

For more info, take a look inside the demo folder. Demo v2.0.0 has more sophisticated examples for you

Thank you for using Intelligent Selector. You are free to add features to this package and if you do
please share them so that I also share with the rest of the world.

Here is some of my assets
Robust FSM(free) - https://assetstore.unity.com/packages/tools/ai/robust-fsm-125927
Super Goalie Basic(free) - https://assetstore.unity.com/packages/templates/systems/super-goalie-basic-144535
Soccer AI($50) - https://assetstore.unity.com/packages/templates/soccer-ai-63743

Contact
Developer: Andrew Blessing Manyore
Company: Wasu Studio
Email: andyblem@gmail.com		
Mobile: +263733888022