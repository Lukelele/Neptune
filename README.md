![Designer](https://github.com/Lukelele/Neptune/assets/44749665/ecb2f858-ef02-4b4c-b028-52058d61537c)

Neptune
-------
Underwater ecosystem simulation in virtual reality using neural networks and genetic algorithms. The full application code is under Assets/Scripts

Features:
- Beautiful 3D underwater environment designed for VR in low poly art style, visuals include god rays, caustics etc..
- Ecosystem populated with sharks, fish flocks and various plants.
- Evolving creatures in real time using feed forward neural network and genetic algorithms, demonstrating natural selection and evolution.
- Central control panel controlling the simulation such as the spawn parameters and speed, also includes visual panels which displays various information about the simulation such as food over time.


Installation
------------
For Unity project:
1. Download and unzip this repository.
2. Open Unity Hub.
4. Select Add button.
5. Choose the unzipped folder location.
6. Open the project.
7. Done!

For VR set up with meta developer hub:


Visit the oculus developer documentation https://developer.oculus.com/documentation/unity/unity-link/?locale=en_GB

Once the project has been imported to Unity and the VR has been set up, pressing play in Unity should start the game in VR.

Getting Started
---------------
- Follow the installation instruction, make sure the game is running with no errors.
- Connect the virtual reality headset, the game should be visible in the headset, turning the headset should rotate the game view if everything has been set up correctly.
- Make sure the controllers are visible and working in VR.

At launch, the player is spawned in the centre of the control platform which is inside the "fish tank" environment, initially, the tank should be empty of any plants or fish. Various panels are attached to the control platform which contains a number of different interative elements such as sliders and toggles, these can be changed by the player through using the trigger on the VR controller to directly affect various settings of the simulation in real time. Movement is enabled through the joysticks on the controllers, and allows the player to fly around the environment and see entities up close. Below introduces all of the interactive functionalities:

### Main Console
<img src="https://github.com/Lukelele/Neptune/assets/44749665/28450202-65ec-4ede-95ab-c2ae0cb23d24" width="500" height="360">\
The main console contains 2 panels displaying information about the simulation. The main panel is configurable and displays several metrics such as the amount of food or the population of sharks over time, the tracked variable can be changed through the drop down menu. The secondary pie chart displays the population distribution of plants, fish and sharks in the simulation.

### Side Console
<img src="https://github.com/Lukelele/Neptune/assets/44749665/f7425cf2-2aeb-4689-9ab2-0de1576772de" width="480" height="360">\
The side console contains the main simulation parameters, the game speed setting can be changed through the slider on the bottom panel, which dictates the time interval the simulation uses, a higher value means time passes quicker. The floating panel shows the creature spawn settings for plants, fish and sharks, the player can select the minimum number of each to spawn in the simulation, and the toggle ensures the population never falls below the minimum.

Infographic panels are hidden in game view and can be revealed by hovering over the holographic turtles.

### Creature Setting Console
<img src="https://github.com/Lukelele/Neptune/assets/44749665/b3eb5b94-06ad-4d68-ae6b-15f6e5ae0a02" width="580" height="360">\
This allows the player to control various creature settings in real time during the running simulation. The flock setting includes the max flock size which represents the maximum number of flock units that can exist in one flock, the speed corresponds to how fast the entire flock moves, the show cone and show centre settings enable the "debug view" which shows the vision of the flock as well as the general heading of the flock. The flock unit settings controls how each individual unit behaves in a large flock, the avoidance distance and avoidance weight are parameters that determines how well each of the flock unit responds to each other as well as obstacles and predators, the cohesion weight determines how each closely the units flock together, and the minimum and maximum speed determine the speed of individual units.

### Reset Console
<img src="https://github.com/Lukelele/Neptune/assets/44749665/86d9ca8b-e73e-46b6-b84f-afdf4f82e1cb" width="540" height="360">\
This console contains the red button which ends the simulation and allows it to be restarted cleanly again. Hovering over the turtle hologram also displays tips in game, which shows that pressing the R trigger over any flocks or sharks will reveal their neural network weights and display it in the centre of the platform for visualisation.
