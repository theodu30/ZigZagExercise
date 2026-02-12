# ZigZag
This project is an attempt at recreating ZigZag by Ketchapp.  
It was done in 3.5 hours as a class exercise.  

## Features
This game has a basic controller with the ball going right or left matching mouse button press.  
The level is infinite and randomly generated with gems sometimes spawning on the trail.  
All blocks fall when too far from the player (you can see it at the bottom of the screen).  
The color of the whole trail changes every 50 points - the hue shifts slightly with a lerp.  
You gain points by switching direction (1 point) and picking up gems (5 points).  
You also have data about: your last score (if a game was played since launch), your best score  
and the number of games played.

This project only has one scene combining menu, game and defeat.  
Camera is pixelated for style to match the UI assets and font.  
> This effect is done by rendering the camera onto a render texture, drawn in the UI.

## Limitations
You can't toggle off the pixelated effect.  

## Known Issues
When you launch the game, if you start by pressing SPACE, the game is in an  
undefined state. The level does not generate and you can't fall.  
Since there is no way to stop playing during a game, you are stuck  
in this state.

#
> Disclaimer:  
*This was a project made in class.  
It's an exercise to help us understand how to make good prototypes in a short time.  
This project is now on GitHub for accessibility and visibility.*
