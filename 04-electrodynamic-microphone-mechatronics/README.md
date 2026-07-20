# Electrodynamic Microphones — Mechatronic Systems Case Study

A study of electrodynamic (dynamic) microphones — how they convert sound into an electrical signal via Faraday's Law of electromagnetic induction — plus a MATLAB simulation of a microphone diaphragm's electromechanical response to an incoming sound wave. Written for the Mechatronic Systems case study at Deggendorf Institute of Technology, advised by Prof. Dr. Dmitry Rychkov.

`MATLAB` `Electromagnetic Induction` `Mechatronics` `Signal Simulation`

---

## What's in this repo

- **`diaphragm_response_simulation.m`** — the group's MATLAB script: an animated simulation of a microphone diaphragm's physical response to sound.
- **`diaphragm_response_visualisation.mp4`** — the rendered output of that simulation.
- **`web-paper.pdf`** — a short web paper on electrodynamic microphones: working principle, advantages/disadvantages, and applications.
- **`literature-review.pdf`** — a longer literature review covering microphone history, comparisons with condenser/ribbon microphones, and recent developments (e.g. MEMS electrodynamic microphones).
- **`presentation.pdf`** — the team's survey presentation slides.

## The simulation

Electrodynamic microphones work by moving a coil (attached to a diaphragm) through a magnetic field; the diaphragm's displacement induces a proportional voltage. The MATLAB script models this chain end-to-end:

1. **Input** — a sinusoidal sound pressure wave (1 kHz) with added random noise, standing in for a realistic acoustic signal.
2. **Diaphragm mechanics** — treats the diaphragm as a mass-spring system, computing its natural frequency from mass and spring constant, then derives displacement from the input pressure and velocity by numerical differentiation.
3. **Electromagnetic induction** — computes the induced output voltage from `V = B·L·v` (magnetic flux density × coil length × diaphragm velocity — the same relationship the web paper and literature review describe conceptually).
4. **Animation** — renders all three signals (input pressure, diaphragm displacement, output voltage) as a synchronized, frame-by-frame animation, which is what `diaphragm_response_visualisation.mp4` captures.

## Context

Group project (6 members). I'm flagging that I couldn't confirm from the files alone which teammate wrote the MATLAB script specifically — worth double-checking before presenting this as your individual contribution. Teammates' names and student IDs appear as co-authors on the literature review and presentation title slides.

## License / Use

Course project. Shared here for portfolio purposes.
