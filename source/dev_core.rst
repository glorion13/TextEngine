.. Copyright (c) 2013 ICRL
   See the file license.txt for copying permission.

core
==============================
The :mod:`core` module contains the following submodules:
	- :mod:`core.components`

.. automodule:: core
   :members:
	
core.components
--------------------------------

	The :mod:`core.components` module contains:

		- The core classes which represent the main game model:

			- :class:`core.components.Game`, which represents the structure of the game (interactive novel or text adventure).
			- :class:`core.components.Resource`, which represents global information, such as the date or the weather in the fictional world, or values like the health state of the player.
			- :class:`core.components.Scene`, which represents a location or a state in the fictional world.
			- :class:`core.components.Action`, which represents an action that can be triggered by the player. A :class:`core.components.Action` consists of a :class:`core.components.Condition` and an :class:`core.components.Effect`. The :class:`core.components.Effect` is only resolved if the condition is ``True``.
			- :class:`core.components.Condition`, which represents a logical evaluation which can be part of an :class:`core.components.Action`. When evaluated it returns a value of ``True`` or ``False``.
			- :class:`core.components.Effect`, which represents the outcome of a given Action that the player performs. An :class:`core.components.Effect` can be anything from changing the scene, interacting with the world or even losing the game.

		- The :mod:`core.components.customisable` module which contains the specific functions used in the :class:`core.components.Effect` and :class:`core.components.Condition` classes.

.. automodule:: core.components
   :members:

core.components.customisable
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
	
	The :mod:`core.components.customisable` module is the layer which can be modified
	by users during the development of games or the development of a game editor.

	It contains the following modules:
		- :mod:`core.components.customisable.effects`, where the functions used in :mod:`core.components.Effect` objects are stored.
		- :mod:`core.components.customisable.conditions`, where the functions used in :mod:`core.components.Condition` objects are stored.

	.. automodule:: core.components.customisable.effects
	   :members:

	.. automodule:: core.components.customisable.conditions
	   :members:

	It also contains functions used to interface with the editor (the default editor is written in C#):
		- :func:`getEffectCount`, returns the count of available functions in :class:`core.components.customisable.effects.EffectFunctions`.
		- :func:`getConditionCount`, returns the count of available functions in :class:`core.components.customisable.conditions.ConditionFunctions`.
		- :func:`getEffectKeys`, returns the keys of available functions in :class:`core.components.customisable.effects.EffectFunctions`.
		- :func:`getConditionKeys`, returns the keys of available functions in :class:`core.components.customisable.conditions.ConditionFunctions`.

	.. automodule:: core.components.customisable
	   :members: