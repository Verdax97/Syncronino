# Syncronino
### What is it?
This is a simple college project used to create simple animation and coordinate moviments/actions of arduino actuators.
## Goals
* Easily create animation for arduino based robot/projects.
* Easily test components.
## How it works
1. Upload the arduino sketch on your board.
2. Disconnect and reconnect the cable. (this is necessary to use the seria comunication with the program)
3. Connect the arduino to the program choosing the correct COM port.
4. Create an actuator chosing the type.
5. Insert pin number(s).
6. You can chose between:
    * Play the actuator animation.
    * Set a single value.
    * Play all the actuators animations.
7. Enjoy?

## Supported types
* Analog
* Digital
* Servo motors
* Buzzer
* RGB led
## Features
* Timeline for each actuator with the possibility to choose the fade type between each keyframe and the rate at wich the fade is applicated.
* Divisor that let's you play at the same time all the actuators animation until it finds another divisor or there are no more actuators.
* Save/Load animations to json files.
## To Do
* Add more support for more actuator.