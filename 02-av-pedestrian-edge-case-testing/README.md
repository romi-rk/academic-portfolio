# Designing Critical Edge Case Scenarios for Pedestrian Road Safety

A research case study on testing autonomous vehicle (AV) pedestrian-detection systems under critical edge conditions - snow, rain, and fog - where Automated Emergency Braking (AEB) systems are most likely to fail. Written for the Cooperative and Autonomous Systems case study at Deggendorf Institute of Technology.

`Autonomous Vehicles` `AEB` `YOLO` `Object Detection` `IEEE Paper`

---

## What's in this repo

- **`individual-paper.pdf`** - "Robust Testing in Automotive Safety: Designing Critical Edge Case Scenarios," the six-page IEEE-format paper I wrote independently (a course requirement: everyone first researches and writes solo before any group work). Focuses on snowy-condition edge cases.
- **`group-paper.pdf`** - "Designing Critical Edge Case Scenarios for Pedestrian Road Safety," a broader joint paper with two teammates extending the same investigation to rain and fog, plus a millimeter-wave radar literature survey.
- **`group-presentation.pptx`** - the slide deck for the group paper.

## The problem

Pedestrian accidents account for roughly 20% of road fatalities in Europe. Euro NCAP's AEB test protocol defines standardized pedestrian-detection scenarios, but those standards are built from a limited, predefined set of test cases - they don't capture the long tail of rare, unpredictable "edge case" conditions (adverse weather, poor lighting, occlusion) that real-world driving actually includes. There's no universal way to validate an AV's pedestrian detection against that long tail.

## Approach

Both papers center on the same experiment: running the YOLO (You Only Look Once) object detection algorithm against pedestrian imagery captured under degraded conditions, then measuring how detection confidence degrades as conditions worsen.

- **Testing scenarios** - derived from the Euro NCAP 2016 AEB pedestrian test protocol (nearside/farside crossings, obstructed crossings, varying target speeds), extended with adverse-weather variants.
- **Detection measurement** - pedestrian-detection confidence scores from YOLO were compared across baseline vs. degraded-visibility conditions (e.g., snow), using two distance-estimation methods (bounding-box position estimation and Euclidean point distance) to cross-check results.
- **Results** - detection confidence dropped sharply under low-light/adverse-weather conditions (down to 0.36–0.42 in the worst cases tested), which is the core evidence for the paper's argument that current AEB validation standards under-test these conditions.
- **Literature survey** (group paper only) - reviews complementary approaches, including millimeter-wave radar imaging as a sensor-fusion option for low-visibility pedestrian detection.

## Context

The individual paper was solo-authored coursework. The group paper and presentation were produced with two teammates, credited as co-authors on the paper and title slide (their emails and student IDs also appear there, as required by the course's submission format). Course-provided grading/guideline documents and peer-review feedback aren't included here - only the two authored papers and the slide deck.

## License / Use

Course project. Shared here for portfolio purposes.
