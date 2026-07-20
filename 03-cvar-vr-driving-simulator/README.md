# VR Active Safety Driving Simulator

Unity/C# scripts from a VR driving simulator built to recreate Euro NCAP pedestrian AEB (Automated Emergency Braking) test scenarios, developed for the VR/AR in System Engineering case study at Deggendorf Institute of Technology.

`Meta Quest 3` · `Unity 6000.0.49f1` · `C#` · `XR Interaction Toolkit`

📄 [Full paper (IEEE format, PDF)](Research%20Paper.pdf) · 🎥 [Video presentation](https://youtu.be/4PwmDM-cRB8)

---

## What's in this repo

This repo contains the scripts I authored for the project, not the full Unity project (asset packs, scenes, and build files are several GB and available on request). It's meant to show the logic behind two of the simulator's core systems:

```
Assets/Scripts/
├── Narrative/
│   ├── LevelManager.cs        # scene transitions, level triggers, SceneManager flow
│   └── IntroSequence.cs       # scripted audio/text briefing at game start
└── Pedestrians/
    ├── PedestrianStateMachine.cs   # idle / walk / run / cross state logic
    └── HazardTrigger.cs            # timing + placement of pedestrian hazard events
```

## Pedestrian State Machine

The core of the hazard-perception system. Pedestrians transition through four states (`Idle`, `Walk`, `Run`, `Cross`) with transition timing and placement modeled directly on Euro NCAP's pedestrian AEB test protocol (nearside/farside crossings, sudden runs, obstructed crossings, varying target speeds).

Three difficulty tiers, one per level:
- **Level 1:** single pedestrian, predictable nearside crossing. Introduces the mechanic without overwhelming the player.
- **Level 2:** two pedestrians, one steady farside crossing plus one sudden nearside run. Tests divided attention and reaction time.
- **Level 3:** night-time junction crossing under constrained visibility. Combines low light with reaction-time pressure.

## Level / Narrative System

Drives the 3-level story progression (courier delivering an SSD under time pressure) using scripted triggers and `SceneManager.LoadSceneAsync()` for seamless scene transitions. Each level scene independently escalates traffic density, time pressure, and lighting conditions, so difficulty ramps without needing to touch the state machine logic.

## Context

Built by a 5-person team. Hardware/VR integration, environment design, traffic AI, UI, and sound were handled by teammates (full breakdown and citations in the paper). Platform: Unity 6000.0.49f1, Meta Quest 3 via XR Plugin, XR Interaction Toolkit, and Oculus XR, with physical steering wheel and pedal input via Unity's Input System.

## License / Use

Course project. Asset packs used under their respective store licenses. Scripts shared here for portfolio purposes. The paper was developed as part of the curriculum and has not been published anywhere.
