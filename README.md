**Game Report**

Artificial Intelligence for Games

**Game Idea:**

The game I’m willing to create is a three-dimensional shooter where the
character is a hunter and his aim is to kill as many other NPC’s (animals) as
possible in a given amount of time.  
The player will have a fixed position on the top of a hill and will only be able
to see throw his aim (like a classic sniper game).

The other game entities will behave accordingly to the surrounding and
eventually counteract, some of them might chase you, some others might just run
away.  
Actions will be taken depending on both the single agent’s status and the given
circumstances.  
The animals will mainly wander around seeking for random positions and pretend
to graze until something happens (this might be a close shoot or just need to
find water). Some of them will stick together and if a single component of the
flock gets alerted by a noise then all the others will run as well.

**Game Concept:**


Like a classical haunting game, this project will feature the character shooting
animals from a selected and fixed point in the map. Each animal killed will give
the hunter points which will be added up when the game is finished to the number
of kills in the game time. Each game will last a range between 1 and 5 minutes,
after that a high score system might be implemented.

The projectile trajectory might be influenced by wind, and in addition to that
the player will have the classical breathing movement so it will need to focus
to take a clear shoot (might implement a “hold shift to hold your breath”
mechanic).

**Course related topics implemented:**  
  
During the past semester we have been studying steering forces and how to apply
them to character movements in a 3/2 D environment, this meant working a lot
with Vector Math.  
The main principle is that considering an entity in a given coordinates system
we can just move the entity only by applying forces to it instead of adding one
to the x instead of the z. All the forces involved are represented as vectors
and as vectors they not only have a magnitude, but they also have a direction.

Vectors can not only be added, multiplied, subtracted and divided but have also
some other operations that turn useful (Dot, Cross).  
Each steering is the result of a force calculate starting only from the Origin
position a and a given target position, each behaviour processes the force
differently and depending on different factors.  
Depending on the direction and the magnitude of the forces that affect the
original vector, it will result in a final move vector that will specify both
where to go and how fast.  
  
  
  
  
  
  
Out of the topics covered in class the game will feature the following:

-   Seeking behaviour:  
    Every now and then the animals will have some physical or mental needs, like
    getting bored of being in a certain place for too long or needing some
    agent’s known good spots to find water. This will mean for them seeking
    fixed position in the map.

-   Wander behaviour:  
    This will be the main NPC’s movement as they (like animals) will just wander
    around the nature, sometimes stopping for few moments some others staying
    there for a while. This behaviour mainly consists in seeking a random
    position after another without any evident logic.

-   Evading behaviour:  
    The agents will only use this behaviour when something dangerous has been
    detected, as they will run away quickly and with the best guessable
    trajectory.

-   Flocking behaviour:  
    Agents (as animals) will form groups and stick together. This is
    representable by the flocking behaviour where entities always refer their
    position to the one of their neighbourhoods.

-   Finite state machine:  
    All the other character in the world will change state thanks to a State
    Machine sitting inside of the character object.

-   Emotional States:  
    If I spare some time, my plan is to add some proper emotional states (if
    touched by another NPC then engage a fight or make some of them braver
    etc.), this would mean to take in account some other variables like time
    from previous actions and possibly having all different times of reaction.  
    Possibly this will mean having different types of animals as well.

**What has already been implemented:**

Thanks to what has been covered in class some moving scripts have already been
implemented but not optimised. Out of all the steering’s covered the game won’t
need more than a couple.  
To be more clear the NPC will essentially need to wander and to run away, this
doesn’t mean it will just need a wander script and a Flee one, in fact in order
to wander it will need to Seek some points, eventually slow down when getting
there (Arrive behaviour) and it might also aim for some random points a given
point (Follow behaviour). While when running away it will not just flee at full
speed towards the opposite direction of the danger but it will possibly do it
range based and whenever is out of risk it will go back to its normal status.  
Stated this, what has already been implemented is:

-   A range and velocity-based Pursuit behaviour, which will be combined with a
    follow script so that will aim for random local coordinates points around a
    target (a green spot to feed from).

-   A range based Evade script.

-   An optimised Avoid script for obstacle avoidance.

-   A Movements manager which takes care of adding all the inputs together.

-   A State Machine, not optimised for porpoise.

*How:*

**Pursuit behaviour:**  
The *procedure* for most of the moving scripts is quite the same, in my opinion
is more about caring about details that makes them realistic.

To start we need the target object, and the origin object, given as parameters
to the function, as the whole steering system is self-enclosed in a *“library
class”* called Steerings.

The pursuit behaviour so far takes in account the future position of the target
in case there is any and it also increase velocity based on the instant velocity
of the target (the difference between each frame velocity).  
The player velocity is multiplied for a certain number of guessing iterations of
the player (so guess where the player is going to be in 4 frames and seek that
point instead).

Getting the velocity of the player is helpful because the agent won’t try and
guess any iteration ahead of the target, but those iterations will be based on
how fast the target is going.

The direction is always retrieved by doing the *origin position – the target
position*.  
The distance is just the *length* of the *direction vector* and if it is not
zero, then gets check against the safe distance variable, if it is greater than
is safe to go full speed otherwise start applying deceleration based on range.  
Finally always cap to a fixed velocity using the *Mathf.Min()* function (takes
the smaller value between the parsed numbers).  
Of course if the distance is zero than stop.

Although animals would more likely seek random spots in the map which velocity
is going to be zero this won’t affect the agent behaviour, because it only means
they will seek 0 position ahead the point to seek.  
It is still worth keeping the target velocity variable as it might turn useful
when facing the flock behaviour.

Regarding the spots in the map, they will be calculated by taking a single
target spot and creating other spot’s close to it referring to the target point
local coordinates in space, something along these lines:

**Evade behaviour:**

The evade is almost the exact inverse of a pursuit but logically the direction
is taken by doing the *target position – the origin position* and the character
is going to be safe as long is going to be out of a safe distance not inside it.

Velocity is always capped and everything else stays the same. Clearly the
velocity of the object /point where is evading from is not taken in account
anymore.

**Avoid behaviour:**

The avoid behaviour is maybe the thing I’ve been working on the most.  
This is very important especially in the environment I’m aiming to have, as
animals will have to feel free to navigate around without the chance of getting
trapped between trees and rocks.

After looking at many different sources online and from the slides, I tried to
come up with my own avoid script.  
[This
tutorial](https://gamedevelopment.tutsplus.com/tutorials/understanding-steering-behaviors-collision-avoidance--gamedev-7777)
gave me the idea of the “ahead” vector.

First thing first the NPC needs to know if there is any obstacle in front of him
and if that object is an obstacle, to do this I decided to use one array of
collider and a list of all the close ones that are obstacles.  
Iterating through the array of colliders is needed to know whether the object is
an obstacle or something else (like the terrain or the player).  
If it is the case than the position of the closest point of the object’s
collider to the origin position gets added to a list of Vector3.

Out of all the points that are obstacles we need to know which one is in front
of us.  
Is possible to check the angle between two vectors by operating a dot product on
the two forces (in this case the forward vector of the origin, which is
normalised by default, and the direction towards the closest point on the target
collider).  
The dot product between two vectors returns a scalar value that is zero if the
two are perpendicular on either the negative or the positive side. If we so
check when the dot product is bigger than 0.5 that means that the point will be
in 45 degrees range from the forward vector of the origin on both sides (tot 90
degree of “FOV”).

The index of the closest in the *most_threatening* list is then calculated by
comparing all the elements with the current closest.  
For each element is compared the distance of the point against the player
position, the one that will result to be the closest pass to further
calculations.

Once evaluated the closest, the avoid direction is created by instantiating a
new vector having as x value the inverse of the z of the direction vector to the
closest point and as a z the x.  
This is easier to understand with a visual representation thanks to this great
[online vector plotter](https://academo.org/demos/3d-vector-plotter/).  
  
The resulting inversed vector is going to be the one pushing away from the
obstacle (red).  
The only thing remained is to check whether the obstacle is on the right or on
the left. And again, this can be achieved using the dot product between the
right vector of the origin and the direction to the obstacle. If negative the
obstacle will be on the left-hand side, otherwise means it is on the right.  

There is still a problem with this avoidance, if the ahead happens to be after
the obstacle somehow it will not detect it at all.  
Taking the above situation, if the obstacle happened to be right in between the
player and the start of the purple circle (might be because of the player
turning) it will not get considered in the first place.

  
**Movement Manager:**  
  
The movement manager class is just where all the inputs of steering’s get added
together resulting in the final vector. This is done by having a list of
instances of the base class of the steering inputs (Inputs) which contains the
vector 3 variable virtual between all its child’s. Every steering input will
override the getter of that vector to make it the same value as the steering
required by the Steering’s class library.  
Each input in the inputs list is a different steering force:  


**Finite State Machine:**

The finite State machine I’ve implemented so far only plays Red-light
Green-light game and swaps between two status.

Anyway, it has been good to face it as we got to know that C\# has a very useful
tool which is the co-routine.  
Coroutines are kind of like threads in the way that they run parallel to the
program, but differently from threads they do affect the execution time of the
program itself.

A function of type IEnumerator is required to start a coroutine, after having
called StartCoroutine on that function it will start running independently.  
When the yield iterator functionality is called the function returns and only
goes back to be read the next frame.  
So for instance in my FSM example it yields a pause of some seconds and then it
keeps going with the loop.  
Although the while is going to run forever this doesn’t bother the program flow
as it will run parallel.

It is essentially a function declared with a return type of IEnumerator and with
the yield return statement included somewhere in the body. The yield return line
is the point at which execution will pause and be resumed the following
frame.

The same State Machine implemented using coroutines could eventually been
implemented in the update function, there is actually a whole [forum
discussion](https://forum.unity.com/threads/coroutines-vs-update.67856/) about
this.  
I will stick with coroutines and just improve the one I have now which is basic.

**What still needs implementation:**

Flocking behaviour and a proper wandering are missing, but they should not take
long to implement as all the steering’s needed in these two are already set up
and ready.

Further and more improved implementation of the State Machine.

**References:**

\-A neat and free download unity landscape with assets and prefabs for trees
rocks etc..  
(https://assetstore.unity.com/packages/3d/environments/fantasy-landscape-103573).

\-From the Unreal Standard Assets Pack, I’ve imported Ethan (the only “player
mesh”), I have also harvested all the scripts for movements, keeping only the
one that links the move vector to the animations.  
(<https://assetstore.unity.com/packages/essentials/asset-packs/standard-assets-32351>)

\-A very clever tutorial about steering behaviours.  
(<https://gamedevelopment.tutsplus.com/tutorials/understanding-steering-behaviors-collision-avoidance--gamedev-7777>)

\-The Official Unity Manual.  
(<https://docs.unity3d.com/Manual.html>)

\-Some chapter or the Programming AI by example.  
(<https://github.com/wangchen/Programming-Game-AI-by-Example-src/tree/master/Buckland_Chapter3-Steering%20Behaviors>)
