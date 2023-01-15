# BlitMasks
Blittable BitMasks by JUHO JOENSUU

This package provides a pair of blittable bitmasks with the following features:

* BitMask structs in 32bit/64bit variants
* Both masks are blittable with member fields consisting solely of a single UInt32 or UInt64 field
* BitMasks overload + - ^ ~ operators to allow add subtract exclusiveor flags from a bitmask without casting or invoking the helper methods for hot code paths.
*	Includes quite a few constructors , can accept params int[] where each int signify flag to set, internalizes cast from enum to correct medium, casting 32 <=> 64 bit masks is unchecked and flags will be set to false for all missing/extra bits
*	Ships with unit tests for the extension methods
*	Targets .NET 6, i may make a .NET 7 version with generic math at a later date.

Who, why, design?
*	I'm a game developer that likes to reinvent the wheel given half an excuse in need of a fast blittable boolean array for a Very hot path,
	hence my decision to implement this tiny library, my own needs would likely have been satisfied with a few scope limited operators but 
	i wanted this to also be an easy to use bitmask that could be used for purposes other than my specialized needs and that did not carry 
	with it the enum casts at point of use, and provide flexible extension methods for settings flags since bitwise operations are not that 
	familiar to many devs.
*	That said, given my gamedev background and needs, the underlying type and the basic operations need to be fast enough to use 
	tens of thousands of times per frame while being hardly noticeable on performance is the driving goal, any updates will likely prioritize
	performance first and ease of use second.
	
	
Icon included is from flaticon under Flaticon license

https://www.flaticon.com/free-icons/bit" Title:bit icons | Description: Bit icons created by Futuer - Flaticon
