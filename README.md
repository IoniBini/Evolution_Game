# Atomic_Procedural
A procedural system of atoms which have modifiable properties, where the triggers and their outputs can be modularly combined to create emergent game states

Ionatan Biniamin Maghidman - s3845901

The only third party asset used is a free free fly camera controller used in play mode, by Sergey Stafeyev

Controls:

WASD - Movement
E - Up
Q - Down
Hold Shift - Move faster
R - Reset camera to start position
V - Toggle the arena's visibility
Esc - Quit application
Space - Stop time (you cant move while time is frozen)

Summary of what the software is capable of:

This app creates a bunch of atoms which carry their own personal properties inside a scriptable obj.
Most importantly, this scriptable obj has an events tab that takes two arguments, a "trigger" and an "output"
the first tells the atom when to activate a certain event, the second tells the atom what the trigger should be activating when called
in combination, they form events that are in essence a visually representated version of if statements but in inspector. Example:
if "collision with an obj tagged arena", then "increase size of this atom by 2"
this way, there are endless possibilites of what the player can define, and all with a visually friendly inspector that makes sure
to occlude unecessary info when not needed, and show things that are important when they are being used.
This combination is great for emergent gameplay states, especially because you can create any number of atom scriptable objs (right click in assets folder, create, Atom)
It works best when being used in editor mode.
I have added a sample build in the files just as an example of what this can do.