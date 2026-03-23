# The Herald – 2D Metroidvania

**The Herald** is a dark 2D Metroidvania set deep beneath the earth in a forgotten, decaying mine. The world is illuminated only by glowing mushrooms, bioluminescent plants, and dying lanterns, creating an oppressive and atmospheric underground environment.

Inspired by challenging, skill-based games, The Herald focuses on **precision combat**, **tight platforming**, and **deliberate progression**. Every encounter is designed to test the player’s timing, awareness, and adaptability.

<p float="left">
<img src="Screenshots/Jade Herald 1.png" width="350"/>
<img src="Screenshots/Jade Herald 2.png" width="350"/>
  <img src="Screenshots/Jade Herald 3.png" width="350"/>
  <img src="Screenshots/Jade Herald 4.png" width="350"/>
</p>

## Core Features

### ⚔️ Combat System

* Fast, responsive melee combat
* Directional attacks
* Charged attacks triggered through parry mechanics
* Precise hitboxes for both player and enemies

### 🛡️ Defensive Mechanics

* Real-time blocking with stamina management
* Parry system that rewards perfect timing
* Stagger state when stamina is depleted

### 🧠 Skill-Based Gameplay

* Enemies have unique attack patterns
* Requires learning, timing, and positioning
* Punishing but fair combat

## Movement System

### 🏃 Player Mobility

* Smooth horizontal movement
* Dynamic turning system
* Momentum-based physics

### 🦘 Jump Mechanics

* Variable jump height
* Fall acceleration system (better control mid-air)
* Jump attack integration

### 💨 Dash Ability

* Fast burst movement with cooldown
* Temporary invulnerability through collision ignoring
* Consumes stamina

## Health, Mana & Stamina

### ❤️ Health

* Represents player vitality
* Damage taken from enemies and hazards
* Death triggers respawn system

### 🔵 Mana

* Used for regeneration (healing ability)
* Recharges through combat actions

### 🟡 Stamina

* Consumed by:
  * Blocking
  * Dashing
* Regenerates over time
* Depletion causes stagger

## 🔮 Regeneration System

* Converts mana into health
* Requires player to stand still
* Includes animation and casting delay
* Encourages strategic healing timing

## 🤖 Enemy Systems

### Enemy Health

* Each enemy has:

  * Health points
  * Optional defense/parry behavior
* Death triggers animations and effects
* Persistent state saved using PlayerPrefs

### Attack System

* Enemies deal:

  * Physical damage
  * Magic damage
  * Poise damage
* Shield interaction determines if attack is blocked or not

### AI Behavior (Example: Sprout)

* Switches between:

  * Melee combat
  * Ranged combat
* Faces player dynamically
* Uses attack/cast cycles with cooldowns

## 🎮 Input System

Supports:

* Keyboard input
* Controller input
* Touch input (mobile)

Dynamic UI:

* Shows on-screen buttons when touch input is detected
* Hides UI when controller is used

## 🎥 Visual & Audio Feedback

### Visual

* Particle effects:

  * Landing
  * Damage
  * Dash
  * Death
* Sprite flashing on damage
* Animation-driven state transitions

### Audio

* Contextual sound effects:

  * Sword slashes
  * Shield hits
  * Footsteps
  * Landing variations
  * Death sounds

## 💾 Save & Respawn System

* Player position saved at checkpoints
* Health, mana, stamina preserved
* Scene reload system on death
* Enemy states persist via PlayerPrefs

## 🎯 Design Philosophy

The Herald is built around:

* **Precision over chaos**
* **Punishment with fairness**
* **Learning through failure**

Players are expected to:

* Observe enemy patterns
* Master timing
* Use mechanics intentionally
