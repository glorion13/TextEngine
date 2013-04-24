import types

"""
The :mod:`core.components` module contains the following classes:

   - :class:`Game`, which represents the structure of the game (interactive novel or text adventure).
   - :class:`Resource`, which represents global information, such as the date or the weather in the fictional world, or values like the health state of the player.
   - :class:`Scene`, which represents a location or a state in the fictional world.
   - :class:`Action`, which represents an action that can be triggered by the player. An :class:`Action` consists of a :class:`Condition` and an :class:`Effect`. The :class:`Effect` is only resolved if the condition is ``True``.
   - :class:`Condition`, which represents a logical evaluation which can be part of an :class:`Action`. When evaluated it returns a value of ``True`` or ``False``.
   - :class:`Effect`, which represents the outcome of a given Action that the player performs. An :class:`Effect` can be anything from changing the scene, interacting with the world or even losing the game.

"""

class Game:
	"""
	The :class:`Game` class basically represents the structure of the game (interactive novel or
	text adventure). Therefore it contains the various :class:`Scene`, global :class:`Resource` and
	global :class:`Action` instances of the game. It also keeps track of the starting scene and the
	current scene.

	:Attributes:
		- `startingScene` A :class:`Scene` object which is the first scene that the game will load.
		- `currentScene` A :class:`Scene` object which is the scene the player is currently in.
		- `scenes` A :class:`list` of :class:`Scene` objects which holds all of the scenes of the game, along with their corresponding attributes.
		- `globalResources` A :class:`list` of :class:`Resource` for storing global resources, such as player HP.
		- `globalActions` A :class:`list` of :class:`Action` for storing global actions, such as actions which are	always available to the player (e.g. `'help'`) or actions such as checking if the player's HP falls under a certain	threshold.

	"""
	def __init__(self):
		"""Initialise a :class:`Game` object."""
		self.name = None
		self.author = None
		self.startingScene = None
		self.currentScene = None
		self.scenes = []
		self.globalResources = []
		self.globalActions = []

class Resource:
	"""
	The :class:`Resource` class represents global information, such as the date or the weather
	in the fictional world, or values like the health state of the player. It is basically a tuple,
	containing a `name` attribute and a `value` attribute.

	:Attributes:
		- `name`: A :class:`string` which holds the name of the :class:`Resource`, e.g. 'HP'.
		- `value`: This attribute holds the actual value of the resource, which can be of any type.	For example, if the resource represents the player's HP, the `value` of the :class:`Resource` can be an :class:`int` of value 10.
	"""
	def __init__(self, name, value):
		"""Initialise a :class:`Resource` object."""
		self.name = name
		self.value = value
	def __eq__(self, other):
		return self.value == other
	def __lt__(self, other):
		return self.value < other
	def __gt__(self, other):
		return self.value > other
	def __le__(self, other):
		return self.value <= other
	def __ge__(self, other):
		return self.value >= other

class Scene:
	"""
	An action can be visible (in the case of an interactive novel) or invisible (in the case of a text
	adventure), by toggling the . In fact, even within the same game there might be cases when actions
	are visible and cases when actions are invisible.
	"""
	def __init__(self, description='default description', name='default room', idV=0):
		"""Initialise a :class:`Scene` object."""
		self.name = name
		self.id = idV
		self.description = description
		self.actions = []

class Action:
	"""
	:class:`Action` object.
	"""
	def __init__(self, name='default action', effectsIfTrue=[], effectsIfFalse=[], conditions=[], visible=True):
		"""Initialise an :class:`Action` object."""
		self.name = name
		self.conditions = conditions
		self.effectsIfTrue = effectsIfTrue
		self.effectsIfFalse = effectsIfFalse
		self.visible = visible
	def conditionsAreTrue(self):
		"""
		Glorious function.

		:params: self
		"""
		booleanCount = sum( [condition.evaluate() for condition in self.conditions] )
		return booleanCount == len(self.conditions)
	def perform(self):
		if self.conditionsAreTrue():
			for effect in self.effectsIfTrue:
				effect.resolve()
		else:
			for effect in self.effectsIfFalse:
				effect.resolve()

class Condition:
	"""
	A :class:`Condition`.
	"""
	def __init__(self, conditionFunction=None, *args):
		"""Initialise a :class:`Condition` object."""
		self.conditionFunction = conditionFunction
		self.args = args
		self.raw = []
	def evaluate(self):
		evalArgs = [arg(n) for arg,n in zip(self.args, self.raw)]
		return self.conditionFunction(*evalArgs)

class Effect:
	"""
	An :class:`Effect`.
	"""
	def __init__(self, effectFunction=None, *args):
		""" Initialise an :class:`Effect` object."""
		self.effectFunction = effectFunction
		self.args = args
		self.raw = []
	def resolve(self):
		evalArgs = [arg(n) for arg,n in zip(self.args, self.raw)]
		self.effectFunction(*evalArgs)