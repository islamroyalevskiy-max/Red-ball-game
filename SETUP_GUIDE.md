# 2D Platformer Game - Setup Guide

## Overview
This is a complete 2D platformer game built for Unity 2022.3.59f1 using the 2D Core template. It includes:
- Player with movement, jumping, and animations (idle, run, jump)
- Health system (3 HP) with UI display
- Two enemy types: Normal (die from jump on top) and Spiked (damage on any contact)
- Two worlds with multiple levels using Tilemap
- Boss fight at the end of World 2
- Level transitions, main menu, sounds, and background music

## Project Structure
```
Assets/
├── Scripts/           # All C# scripts
│   ├── PlayerController.cs    # Player movement, health, animations
│   ├── Enemy.cs               # Base enemy class
│   ├── NormalEnemy.cs         # Regular enemy (dies from jump)
│   ├── SpikedEnemy.cs         # Spiked enemy (damages on contact)
│   ├── Boss.cs                # Boss enemy with health bar
│   ├── LevelManager.cs        # Scene management
│   ├── UIManager.cs           # Health UI, boss health, level complete
│   ├── MenuController.cs      # Main menu and pause menu
│   ├── AudioManager.cs        # Music and SFX management
│   ├── LevelTrigger.cs        # Level transition trigger
│   └── PlayerAnimation.cs     # Animation controller
├── Scenes/            # Game scenes
│   ├── MainMenu       # Main menu scene
│   ├── World1/        # World 1 levels
│   └── World2/        # World 2 levels (includes boss)
├── Prefabs/           # Prefabricated objects
├── Sprites/           # Sprite assets
├── Tilemaps/          # Tilemap tiles
├── Audio/             # Sound effects and music
└── UI/                # UI sprites and prefabs
```

## Setup Instructions

### 1. Import Required Assets
1. Open Unity Hub and create a new project using **2D Core** template
2. Unity version: **2022.3.59f1**
3. Copy all files from this project to your Unity project's Assets folder

### 2. Create Tags and Layers

#### Tags (Edit → Project Settings → Tags and Layers)
Create the following tags:
- `Player`
- `Enemy`
- `Spike`
- `Ground`
- `Boss`

#### Layers
Create the following layers:
- `Player` (Layer 8)
- `Enemies` (Layer 9)
- `Ground` (Layer 10)

### 3. Setup Player Prefab

1. **Create Player GameObject:**
   - Right-click in Hierarchy → 2D Object → Sprites → Square
   - Rename to "Player"
   - Add components:
     - `Rigidbody2D` (Freeze Z Rotation, Mass: 1)
     - `BoxCollider2D` (Size: 0.8 x 0.9)
     - `Animator` (create Animator Controller)
     - `SpriteRenderer`
     - `PlayerController` script

2. **Configure PlayerController:**
   - Move Speed: 5
   - Jump Force: 7
   - Ground Check Point: Create empty child at player feet
   - Ground Layer: Ground
   - Max Health: 3

3. **Setup Animator:**
   - Create Animator Controller named "PlayerAnimator"
   - Create parameters:
     - `IsRunning` (Bool)
     - `IsGrounded` (Bool)
     - `Jump` (Trigger)
     - `Hurt` (Trigger)
   - Create states: Idle, Run, Jump, Hurt
   - Set up transitions between states

4. **Add Audio Clips to PlayerController:**
   - Jump Sound
   - Hurt Sound
   - Death Sound

5. **Save as Prefab:** Drag Player to Prefabs folder

### 4. Setup Enemies

#### Normal Enemy:
1. Create sprite with BoxCollider2D (Is Trigger: false)
2. Add Rigidbody2D (Kinematic or Dynamic with constraints)
3. Add `NormalEnemy` script
4. Add Animator with Death animation
5. Configure patrol points (left and right transforms)
6. Tag as "Enemy"

#### Spiked Enemy:
1. Create sprite with BoxCollider2D
2. Add Rigidbody2D
3. Add `SpikedEnemy` script
4. Tag as "Enemy" and "Spike"
5. Configure patrol points

### 5. Setup Boss

1. Create larger sprite for boss
2. Add components:
   - `Rigidbody2D`
   - `BoxCollider2D`
   - `Animator`
   - `Boss` script
3. Configure:
   - Max Health: 5
   - Move Speed: 3
   - Attack Range: 2
   - Attack Cooldown: 2
   - Damage: 1
4. Setup patrol points
5. Tag as "Boss" and "Enemy"

### 6. Create Tilemap Levels

#### For each level:
1. **Create Tilemap:**
   - Right-click Hierarchy → 2D Object → Tilemap → Rectangular
   - This creates: Grid, Tilemap, Tilemap Renderer, Tilemap Collider 2D
   
2. **Create Tiles:**
   - Open Tile Palette window (Window → 2D → Tile Palette)
   - Create new palette
   - Drag platform sprites to create tiles
   
3. **Paint Level:**
   - Select Tilemap tool
   - Paint platforms, ground, walls
   
4. **Add Collider:**
   - Ensure Tilemap Collider 2D is on Tilemap
   - Add Composite Collider 2D to Grid (if needed)
   - Set Ground layer on tilemap

5. **Place Objects:**
   - Player spawn point (empty GameObject)
   - Enemies (from prefabs)
   - Collectibles
   - Level trigger (for next level)
   - Hazards/spikes

### 7. Setup UI

1. **Create Canvas:**
   - Right-click Hierarchy → UI → Canvas
   - Canvas Scaler: Scale With Screen Size
   
2. **Health Display:**
   - Create 3 Image objects under Canvas
   - Position in top-left corner
   - Assign heart sprites (full and empty)
   
3. **UIManager Setup:**
   - Add `UIManager` script to Canvas
   - Assign health icons array
   - Assign full and empty heart sprites
   - Create Boss Health Panel with Slider
   - Create Level Complete panel

4. **Main Menu:**
   - Create new scene "MainMenu"
   - Add Canvas with UI elements:
     - Title text
     - Start button
     - Quit button
   - Add `MenuController` script
   - Connect button OnClick events

### 8. Setup Audio

1. **Create AudioManager:**
   - Create empty GameObject "AudioManager"
   - Add `AudioManager` script
   - Create Sound array with:
     - Background music (loop: true, isMusic: true)
     - Jump sound
     - Hurt sound
     - Death sound
     - Enemy death sound
     - Boss attack sound

2. **Add Audio Clips:**
   - Import audio files to Audio folder
   - Assign clips to AudioManager

### 9. Scene Configuration

#### Build Settings:
1. File → Build Settings
2. Add all scenes in order:
   - MainMenu
   - World1_Level1
   - World1_Level2
   - World2_Level1
   - World2_Level2 (Boss level)

#### Physics Settings:
1. Edit → Project Settings → Physics 2D
2. Set appropriate gravity (-9.81 or -15 for snappier feel)
3. Configure collision matrix

### 10. Testing Checklist

- [ ] Player can move left/right
- [ ] Player can jump
- [ ] Animations play correctly (idle, run, jump)
- [ ] Health decreases when hit by enemy
- [ ] Health UI updates correctly
- [ ] Normal enemy dies when jumped on
- [ ] Spiked enemy damages player on any contact
- [ ] Boss fights correctly and shows health bar
- [ ] Level transitions work
- [ ] Main menu functions (Start, Quit)
- [ ] Pause menu works (Escape key)
- [ ] Background music plays
- [ ] Sound effects play appropriately
- [ ] Player respawns after death

## Code Quality Notes

All scripts follow these principles:
- Clear XML documentation comments
- Serialized fields for Inspector configuration
- Event-based architecture for decoupling
- Proper component requirements ([RequireComponent])
- Null checks for safety
- Memory management (Destroy with delay, proper event unsubscription)
- Single Responsibility Principle
- DRY (Don't Repeat Yourself)

## Common Issues & Solutions

1. **Player falls through ground:**
   - Check collider sizes and positions
   - Ensure physics layers are configured correctly
   - Increase physics iteration count in Project Settings

2. **Animations not playing:**
   - Verify Animator Controller is assigned
   - Check parameter names match exactly
   - Ensure transitions have correct conditions

3. **Audio not playing:**
   - Check AudioListener exists in scene (usually on Main Camera)
   - Verify audio clips are assigned
   - Check volume settings

4. **UI not updating:**
   - Verify event subscriptions
   - Check references in Inspector
   - Ensure Canvas is configured correctly

## Next Steps

After basic setup, consider adding:
- Particle effects for jumps, deaths, attacks
- More enemy types
- Power-ups and collectibles
- Checkpoints system
- Multiple player skins
- Achievements system
- Mobile controls support
