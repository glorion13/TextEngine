.. _game_model:

What is a TAINT game?
==========================

Games created with the TAINT model have a structure which is very simple and at the same time very powerful! Because it is very abstract, it really is up to you to use it and be creative.

In this section you'll learn exactly what the TAINT model is and how to use it to create your own games!

Main building blocks
---------------------

TAINT has the following core building blocks:
	- `Scene`: Scenes represent a location or a state in the game world.
	- `Resource`: Resources represent anything that needs to be tracked! The player's HP, Mana, the time or date in the game world, whether or not the player has an item etc. One example of a resource can have a name of "Hitpoints" and a value of 10.
	- `Action`: Actions represent happenings in the game world, whether it is something that occurs by itself (passive) or something that is triggered by the player (active). An action consists of `conditions` and `effects`. One list of effects are triggered if the conditions are true and another if the conditions are false.
	- `Condition`: Conditions are ways to give further branching to your game, by checking if certain things are true or not (e.g. does the player have the required item to open the door?). Conditions have a left-hand side and a right-hand side argument and then an operator which compares them. For example, the left-hand side argument might be a `Resource` called "HP" and the right-hand side argument might be the number 0. Using a `Condition`, it is possible to check whether the "HP" has dropped below 0 or not.
	- `Effect`: Effects are the resolutions of an `Action`. Basically, effects can be things like going to a new `Scene`, changing the value of a `Resource` or simply giving some additional information to the player.
	- `Narrative`: The narrative is the story which is presented to the player in the form of text.

These building blocks are then combined in the game structure in order to create interactive experiences!

Game structure
---------------

A game contains a list of `scenes`, a list of `global resources` and a list of `global actions`. `Global resources` are `resources` which are accessible from all scenes of the game. Similarly, `Global actions` are `actions` which are accessible from all scenes. A game also has a `starting scene` which is the scene in which the game begins.

Each `scene` of the game, contains a list of `actions` and `resources`. Those are also called `local actions` and `local resources`, because unlike their `global` counterparts, they are only accessible within the scene to which they belong.

So for example, a `global action` could be an action that tells the player what is in their inventory and it is global because you want the player to be able to check his inventory at all times, no matter where he is. On the other hand, a `local action` (an `action` within a `scene`) could be an action which opens a door in that specific scene. It is clear that there is no reason for this action to be available in any other scene of the game. Actions have three switches which can be toggled between on and off.

The `enabled` switch, sets an action as accessible or entirely not accessible (as if it doesn't exist).

The `active` switch sets the action as passive or active. Active actions can only be triggered by the player (e.g. the player decides to open a door) while passive actions are evaluated in every turn (e.g. always check if the player's "HP" falls below 0 and if it does let them know they are dead).

The `visible` switch makes the action visible to the player.
The player can always see a list of all visible actions (just like many traditional interactive novels). One case where you don't want a scene to be visible is in the case you want the player to input commands rather than choose from a list. Another case would be to include easter eggs!

Types
----------

There are 5 `types` (programmatically speaking) in a TAINT game:
	- `Number`: a number can be an integer like 10 or a float like 0.75.
	- `Text`: text is exactly what you would imagine; it is used for anything written.
	- `Boolean`: boolean can be either ``True`` or ``False`` and can be used for checks like whether or not something exists or whether or not the player has done something.
	- `Resource`: a resource is a type by itself as you can explicitly compare e.g. a `resource` with a `boolean`. A `resource` has the interesting property that its value can be any of the three types `Number`, `Text` or `Boolean`.

Conditions and effects
-----------------------

Conditions and effects are a layer which can be customised to add your own. If you are interested in something like that, read more on the :ref:`scripted_components` section. Having said that, the default list of conditions and effects should cover more than the basic needs of a game.

Here is a list of all the comparisons you can do with the default conditions:
	- `equals`: Checks if two things are equal (e.g. `Resource` "PlayerHasAxe" equals ``True``).
	- `less than`: Checks if the value of one thing is less than the value of another (e.g. `Resource` "HP" is less than 0).
	- `less or equal`: Similar to the above but checks if the value of one thing is less or equal to the value of another.
	- `greater than`: Checks if the value of one thing is greater than the value of another (e.g. `Resource` "ItemsInInventory" is greater than 10).
	- `greater or equal`: Similar to the above but checks if the value of one thing is greater or equal to the value of another.


This is a list of all the effects available by default:
	- `Tell player`: displays text to the player by adding it to the game's `Narrative`.
	- `Go to scene`: takes the player to a new scene.
	- `Add global resource`: adds a global resource to the game.
	- `Edit global resource`: changes the value of an already existing global resource.
	- `Delete global resource`: deletes an already existing global resource.
	- `Add local resource`: adds a local resource to a scene.
	- `Edit local resource`: changes the value of an already existing local resource.
	- `Delete local resource`: deletes an already existing local resource.
	- `Display all actions`: displays all enabled actions to the player.
	- `Display all visible actions`: displays only the actions which are set as `visible`.
	- `Make action visible`: Sets an action as `visible`.
	- `Make action invisible`: Sets an action as not `visible`.
	- `Make action enabled`: Sets an action as `enabled`.
	- `Make action disabled`: Sets an action as not `enabled`.
	- `Make action active`: Sets an action as `active`.
	- `Make action passive`: Sets an action as not `active`.

Resource priority
----------------------

It is generally recommended that you use different names for each resource, even if one is global and the other is local. In the case that multiple resources have the same name, only the first resource found will be handled.

Local resources are searched first before global resources.