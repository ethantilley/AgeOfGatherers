# AgeOfGathers

Unity C# AI simulated environment of wood and berry gatherers... and one builder for storage upgrades.

Implemented Features:

* Goal Oriented Action Planning (GOAP) for AI Behaviours for different roles (Gatherers (Berry & Wood) & Builders).
* A* (A Star) Path Searching Algorithm with my own little twist for different behaviours. (Agents don't like going up slops if it isn't necessary).
* Manager AI that decides for them when to switch to and from roles based on the corresponding storage capacity.
* Procedural terrain generation + generation of path data.
* Death Via: Old age (Age of death base on how well they take care of themselves) & Hunger
* Gravestones with a generated epitaph of agents: name, birth to death date and cause of death.

In-Progress Features:

* Builder torch placement with lighting calculations in nodes for nighttime gathering.
* Reproduction of Gatherers, then builders - Later when there are more behaviours for builders.

# DISCLAIMER
I recommend using my Goal Oriented Action Planning (GOAP) system for first-timers of GOAP in unity with C#. Although I think some debugging systems implemented with this would be advantageous when encountering bugs with behaviours.

This project is, unfortunately, not the most commented code of mine. The code itself, however, is quite good considering it was my first real go at AI (AI behaviours using GOAP, A* pathing algorithm, etc.).
