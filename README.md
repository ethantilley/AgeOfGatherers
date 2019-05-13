## Age Of Gatherers
A collective of AI agents that explore a procedurally generated environment, collect and store resources based on their dictated role by the AI manager.

## Built With

* [C#](https://docs.microsoft.com/en-us/dotnet/csharp/) - Programming language used
* [A*](https://en.wikipedia.org/wiki/A*_search_algorithm) - Pathfinding algorithim used
* [GOAP](https://medium.com/@vedantchaudhari/goal-oriented-action-planning-34035ed40d0b) - Behaviour system used
* [Unity](http://www.unity.com/) - Game Engine 
* [HackN'Plan](https://hacknplan.com/) - Task Management Softwear
* [SourceTree](https://www.sourcetreeapp.com/) - Repository softwear paired with github
* [VisualStudio](https://visualstudio.microsoft.com/) - For all coding purposes

## Authors

* **Ethan Tilley** - *Solo Developer* - [@EthanTilley_](https://twitter.com/EthanTilley_)

## Use

Clone repo and build it in unity or [check out its itchio](https://ethantilley.itch.io/creaturefeature) and download it there (maybe an old version)

## Why

This project was at first a simple university assignment, but I fell in love with AI and continued it's development as a side-project.

## Features
The Features I've currently implemented into the project include:
*  Goal Oriented Action Planning (GOAP) for AI Behaviours for different roles (Gatherers (Berry & Wood) & Builders).
* A* (A Star) Path Searching Algorithm with my own little twist for different behaviours. (Agents don't like going up slops if it isn't necessary).
* Manager AI that decides for them when to switch to and from roles based on the corresponding storage capacity.
* Procedural terrain generation + generation of path data.
* Death Via: Old age (Age of death base on how well they take care of themselves) & Hunger
* Gravestones with a generated epitaph of agents: name, birth to death date and cause of death.

## Goals With This Program
- [X] Improve Camera movement so its more freeform
- [X] Death system, with gravestones that have an epitaph
- [ ] Agents are uninformed of the environment and its contents until it discovers it (Incomplete information)
- [ ] Builder agents place torches that the gatherers prefer to follow during the night
- [ ] Predator that attacks villagers at night, although, avoids light (torches)
- [ ] Biome regions (Desert, Woodlands, Grasslands, etc...)
- [ ] Seed system - Generate a unique seed that can reload a level through typing the seed in
- [ ] Improve 3D models
- [ ] Builder can build houses that increase the chance of a population increase
- [ ] Pathfinding cache - short term storage of paths. (Agents reuse calculated paths)
- [ ] Hierarchical pathfinding
- [ ] Pathing request system (Simply queues pathing calculations)
- [ ] Detailed display - ie, hovering mouse over an agent will display: village role, age, hunger, goal and name
- [ ] Smarter reproduction system (currently spawns an agent when a death occurs)

## Acknowledgments

* Hat tip to my lecture Iain McManus who facilitated me through this project (back when is was just an assignment) and exposed me to the beauty of AI. <3


