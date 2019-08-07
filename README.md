# Maumer
A collection of editor tools I use across projects in Unity

### Install
#### Option 1: Git clone or download zip
#### Option 2: [Git Submodule](https://git-scm.com/book/en/v2/Git-Tools-Submodules)
Run `git submodule add -b [branchName] git@github.com:sichenn/Maumer.git path/to/folder`, 
e.g. `git submodule add git@github.com:sichenn/Maumer.git Assets/Plugins/Maumer`
#### Option 3: [Unity Git Package](https://neogeek.dev/creating-custom-packages-for-unity-2018.3/)
Import Kore by adding `"com.sichenn.maumer": "https://github.com/sichenn/Maumer.git"` to `Packages/manifest.json`

### Features
#### Cubemap Generator (In development)

#### Mesh Tool
##### Description
Visualizes mesh data such as normals and positions
##### Todo
* Optimization.

#### Motion Tracker (In development)
##### Description
Tracks object motion. 

![MotionTracker_resized](https://user-images.githubusercontent.com/20757517/54280265-480ea700-4554-11e9-8418-e6005fe7a214.gif)

#### Raycast Debugger
##### Description
Helps visualize raycasts in 2D

![RaycastDebugger_resized](https://user-images.githubusercontent.com/20757517/54280327-6e344700-4554-11e9-94e1-5bb153aa7205.gif)

#### Ruler
##### Description
Displays a ruler and position information in Scene View

![Ruler_resized](https://user-images.githubusercontent.com/20757517/54280532-f9154180-4554-11e9-8704-cdb6b9794b18.gif)

##### Todo
* Add guides functionality

#### Visual Debug (In development)
![VisualDebugger](https://user-images.githubusercontent.com/20757517/62638415-fe45b380-b96f-11e9-8bf2-f9cea4413bd4.gif)

##### Description
Helps visualize step-by-step game logic.

##### Todo
* Testing
* More features
* Add example code

### License

This project is licensed under the [MIT License](LICENSE)
