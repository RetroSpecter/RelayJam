From Jose's stuff, I've added a couple things:

- The "Machine" Type Unit
	- Is an enemy unit, but attacking it activates a Coroutine. See example under MachineExampleTemplate.cs
	- This can be used for effects outside the turn-based system. Attacking the shooter object, for instance, will shoot a bullet every second, regardless of turn order
- Camera
	-WASD Movement for larger maps
- LemonLime


PS. Don't make a machine that rotates the Entire Map otherwise things start to break. But moving it around/giving it a rigidbody is a totally viable thing to do *wink* *wink*.
PPS. Don't remove health from units outside of turn combat otherwise BAD THINGS WILL HAPPEN. The current bullets remove attack/defense
PPPS. Things aren't quite like things. Make things like things.