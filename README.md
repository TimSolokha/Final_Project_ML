# Final_Project_ML
## Overview:
Using MLAgents and Unity we are going to create a brain that when attached to a ball can solve a randomly generated maze using rays/vectors shot out to determine a path. Penalties for dead ends and reward for getting closer to end. Ultimately, I want a human player to be racing the trained agent to see who can complete the maze faster.

## Machine Learning Elements:
-	Deep Reinforcement learning
-	Rewards and penalties
-	Ray/Vector based, not necessarily based off transform position
- Both elements may be needed to prevent backtracking
## Additional Required Elements:
-	3D Generated Maze
-	Checkpoint asset
-	Path asset
-	Cube and Sphere materials/shaders

## Milestone 1:
-	Scene done with a static maze
-	An agent that we move through the maze 
-	Play with rays and cameras for the trained agents
-	Picking assets and deciding on maze size

## Milestone 2:
-	Final decision on rays and cameras
-	Somewhat well trained, can get through the maze (even if slowly)

## Milestone 3:
-	Update maze generator to inlcude spawn and endpoints
-	Using ml-agents cumulative training with the maze generator
-	Randomize end marker loactions to test left, right, up, and down.

## Milestone 4:
-	Inlcude positive rewards for visiting new tiles
-	Final train to solve as big as we can get it to
-	Add player to compete with

**NOTE**: For the final milestone, we ran Tom's brain during the presentation.
