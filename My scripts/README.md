# VR Active Safety Simulator

A Unity/VR simulation built to train pedestrian road-safety awareness. The player navigates a campus-like environment on foot (or drives a car in dedicated levels) while AI-controlled traffic follows waypoints, obeys traffic lights, and reacts to collisions — teaching players to read traffic cues and cross safely under a time limit.

This repository contains the gameplay scripts only, organized by system for readability. It does not include the full Unity project (scenes, meshes, prefabs, audio assets, or `.meta` files) — just the C# source, as a code showcase.

## Systems

**Vehicle** — `CarController` drives the player's car using WheelColliders, gamepad/keyboard input, and a physically simulated steering wheel. `AICarController` drives background traffic along waypoints, yields at red lights, and raycasts ahead to avoid rear-ending other cars.

**TrafficSystem** — `TrafficLight` renders one intersection light's color; `TrafficLightManager` runs a 4-way signal cycle (through-traffic and protected left turns) that `AICarController` reads to decide when to stop.

**NPC** — `NPCController` patrols a NavMesh, plays idle/walk animations, and falls down and recovers if hit by a car. `NPCSpawner` keeps a population of NPCs alive around the player and despawns ones that wander too far (spawns pedestrian prefabs, not included in this repo).

**Navigation** — `WaypointLoopAgent` moves a NavMeshAgent through an ordered loop of waypoints (used for scripted routes). `Waypoint` defines a linked-list waypoint node with a lane width, used by the custom waypoint editor.

**Editor** — `WaypointEditor` draws Scene-view gizmos for waypoint chains; `WaypointManagerWindow` is a custom EditorWindow (Tools > Waypoint Editor) for creating/inserting/removing waypoints without manual GameObject wrangling.

**CollisionFeedback** — Player-facing reactions to being hit by a car: `PlayerCollisionDetector` shows a warning message on a UI Text for a few seconds, `ActivateOnPlayerCarCollision` is a generic "activate this GameObject on hit" trigger, `SpawnPrefabAndText` spawns a VFX prefab plus a text/audio cue on trigger, and `RagdollToggle` switches a character into ragdoll physics on demand.

**LevelManagement** — Scene flow: `LevelFinish` handles end-of-level dialogue, fade transitions, and advancing to the next scene (or a game-over screen after the last level); `LevelSwitcher`/`LevelTransitionAnimation` and `ResetLevel`/`LevelResetAnimation` trigger animated scene transitions on player input; `SceneController` is a small static flag guarding against overlapping transitions; `GameOver` reloads on a fatal collision; `MainMenu`/`LevelMenu` handle menu navigation.

**UI** — `Timer` is a singleton countdown clock (persists across scenes) with a "you're late" fail state; `SceneSetup` rebinds the Timer's text field after a scene reload; `TypewriterEffect` and `TypingTextOnTrigger` reveal dialogue text character-by-character, optionally synced to voice-over audio.

**Audio** — `ProximityAudioTrigger` plays a one-shot audio cue when the player enters a trigger radius (used for ambient/vending-machine sound cues).

**Utility** — `DisableAfterDelay` deactivates a GameObject after a delay; `SphereFollower` is a simple homing/follow behavior.

## Notes on this repo

This is a curated snapshot of my working scripts folder, cleaned up for portfolio presentation. A few near-duplicate prototypes (multiple experimental NPC patrol scripts, an alternate audio trigger, alternate "hit by car" UI variants) were consolidated down to the most complete version of each system. Filenames were also corrected to match their class names where the originals didn't (Unity is lenient about this, but it matters for a clean repo).

## Requirements

- Unity 2022 LTS or later
- Unity AI Navigation package (NavMesh)
- TextMeshPro
- New Input System (used by `CarController`)
