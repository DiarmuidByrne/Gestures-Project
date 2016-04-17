# Gestures-Project
***Diarmuid Byrne***

## Purpose of the Application
This Unity game was made as a project for my Gesture-Based UI module.
The function of the project is to explore cutting edge UI technologies available for developing applications that can be used in situations that would have been limiting if used with mainstream Interfaces, such as a keyboard and mouse.
This application is designed to be fully utilized for the Myo armband and doesn't require keyboard-mouse interaction.  

## Gestures Identified
Using the Myo Armband, a range of gestures become available.
The default gestures that come pre-packaged with the device are:
- Relax arm
- Close fist
- Spread fingers
- Wave in/out
- Pinch (Double tap)<BR>

<img src="https://media.licdn.com/mpr/mpr/shrinknp_800_800/AAEAAQAAAAAAAAC7AAAAJDgxMzg0YzZjLTY3MjQtNGFiNy04ZTNmLWRlNGY2ZjQwYjAwYQ.jpg">

While there is an option to create custom gestures, the standard gestures were sufficient for the application I wanted to create. This was due to the timing nature of the gameplay, and the more natural the gesture is, the easier it would be to make in the middle of a game.
The gestures I used in the game are:
- Spread Fingers (Open)
</BR>Spread fingers is used to navigate the in-game menus. To make selections the user must simply move their arm until the onscreen pointer is hovering over a button and spread their fingers to select the button.
- Close fist
</BR> The close fist gesture is used to activate a powerup when one is available. If the player collects 5 or more fruit in a row, the score will turn red indicating that a powerup is available. The powerup slows down time to make it easier to collect more fruit.
- Double tap
</BR> Double tap can be used to re-center the pointer at any time, making the user's current arm position as the reference point. This was done to overcome any tracking or syncing issues the user may experience.

## Hardware Used
The Myo armband from Thalmic Labs was used to power this project. From their website, the Myo armband is "a wearable gesture control and motion control device" that allows control of computers, phones and tablets "touch-free".[1](Myo HomePage)

The Myo uses a mix of accelerometers and muscle sensors to get an accurate reading of the position of your forearm and hand. The hardware is akin to that of the Leap Motion controller, which senses your hand movement. Unlike the Myo though, Leap Motion does not calculate forearm position. This limits any usage to gestures past the wrist.
However, the Myo is not without its drawbacks, which I have learned in my time with the device. The Motion and gesture detection can be very finicky, and the warmup and calibration times can be inconvenient.

## Solution Architecture
***Project Class Diagram***

<img src="http://i.imgur.com/xWfxzO1.png">

The top 7 classes show are custom scripts I made to develop the application. The rest of the classes consist of the Myo SDK.

The main custom scripts consist of:
- Joint Movement
<BR>The JointMovement script is a customized version of the "JointOrientation" sample script that comes with the Myo Unity package. Instead of rotating an object along a fixed joint, the object now moves along the x and y axis based on the movement detected by the Myo's onboard accelerometer. To utilize this, I made a small spherical gameobject that the JointMovement script attaches to. The sphere then becomes the pointer for playing and navigating the application. The script also handles all gestures used in the game, between activating a special powerup and pressing buttons to navigate the games menus.
- Fruit Spawner
<BR>This script controls the spawning in of the fruit that can then be "sliced" with the pointer. The script was attached to an empty GameObject in Unity. The GameObject had 6 child objects that were placed in different positions just offscreen. While the top 3 spawners allowed the fruit to spawn and fall in and out of the game area, the bottom-spawning fruit required an upwards force to be exerted using Unity's physics engine. The force applied was randomized using the C# Random utility.
- Fruit Slicer script
<BR> This script was attached to each fruit spawned in, and handles the collision detection between the pointer and the fruit. If the fruit is sliced, points are added to the player's score and the fruit is returned to a "fruit pool"  object until it is spawned again.
- Camera script
<BR>The Camera script attaches to the scenes main camera. The script contains multiple public variables that are attached to preset GameObjects before the project is ran. Any other scripts that require these objects get the object reference through the Camera Script.
- Main Menu script
<BR> This script controls all menus and buttons used to navigate the application. It controls the different menus using canvas group components attached to the respective GameObjects to control their visibility.

## Conclusions
The Myo armband provided an interesting change of pace from standard programming and testing. While gestures can allow users to interact with software and hardware in unique ways, Myo is still a ways off a game-changing product. The inconsistent gesture recognition along with warmup times makes for inconvenient usage.

## References
- [1] [Myo Homepage](https://www.myo.com/)
- [2] [Myo SDK](https://developer.thalmic.com/docs/api_reference/platform/index.html)
