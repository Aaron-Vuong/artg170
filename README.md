# ARTG170

Welcome to the ... project!

## Setting up in Unity
The project is currently being built with Unity 2022.3.17f1 and is packaged in the below ARTG170 folder.

To set up in Unity, download the correctly versioned Unity editor and perform the following steps.
```
1. Open Unity Hub
2. Add -> Add project from disk
3. Point to the ARTG170 folder beneath the root repository -> Add Project
4. Click on your newly added project.
```
## Scene Organization
There is a Scene enum in SceneChangeManager that should be updated whenever a Scene is added. Put the Scene name in that enum to be able to refer to it properly in other scripts.


There are currently three types of scenes.
```
1. The game scene(s) which are extensible
2. The MainMenu scene
3. The Loading scene
```
The MainMenu scene initializes all managers + persistent systems that will carry over into each game. As such, either load this scene additively to gain access to all of the managers or load this scene first (The managers are marked in folders that DontDestroyOnLoad).

The Loading scene is an in-between scene that will load whatever scene was requested. Code stolen from CodeMonkey.

The game scenes will load any game environments with the player. This is where all levels go and should link together accordingly.