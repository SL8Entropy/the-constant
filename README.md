# Game Project- THE CONSTANT - README

## Overview
This is a 2D game developed using Unity, featuring a player character with movement, combat abilities, and interactions with various enemy types. The game includes mechanics such as dashing, bashing, shooting projectiles, and enemy AI behavior.

## Features
- *Player Movement:* Walking, jumping, dashing, and bashing mechanics.
- *Enemy AI:* Different types of enemies with unique behaviors.
- *Combat System:* Player can attack enemies using bashing and projectiles.
- *Camera System:* Smooth camera following the player.
- *Pause & Resume Functionality.*
- *Health System:* Player health with damage and immunity mechanics.
- *Projectile System:* Projectiles for both player and enemy attacks.
- *Game Reset Functionality.*

## File Structure
### *Player System*
- Player.cs - Controls player movement, dashing, and bashing mechanics.
- PlayerHealth.cs - Manages player's health, immunity, and game over state.

### *Enemy System*
- EnemyClass.cs - Base class for all enemy types.
- EnemyDasher.cs - An enemy that dashes toward the player.
- EnemyGrunt.cs - An enemy that shoots projectiles.
- EnemyStatue.cs - A stationary enemy that shoots projectiles.

### *Projectile System*
- Projectile.cs - Handles movement and collision of projectiles.

### *Game Mechanics*
- bashArrow.cs - Manages the visual indicator for the bash mechanic.
- PauseResume.cs - Implements pause and resume functionality.
- cameraFollow.cs - Ensures the camera follows the player smoothly.
- ResetButton.cs - Handles resetting the game by reloading the current scene.

## Installation & Setup
1. Open Unity and load the project.
2. Attach the scripts to the respective GameObjects in the scene.
3. Assign prefabs and dependencies as needed (e.g., player, enemies, projectiles).
4. Play the game in Unity Editor.

## Controls
- *Arrow Keys / WASD:* Move the player.
- *Spacebar:* Jump.
- *Left Mouse Button:* Dash.
- *Right Mouse Button:* Bash.
- *Escape / P:* Pause and Resume the game.
- *Reset Button:* Resets the game by reloading the current scene.

## Future Enhancements
- Additional enemy types with advanced AI.
- More levels with different challenges.
- Power-ups and abilities for the player.

## Credits
Developed by Sudharshan Sambathkumar, Abhijith MI , Abhinav Raghvendra

---
For any issues or feature requests, feel free to reach out!