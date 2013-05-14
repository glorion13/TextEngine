import customisable

class Game(customisable.effects.EffectFunctions, customisable.conditions.ConditionFunctions):
	"""
	The :class:`Game` class basically represents the structure of the game (interactive novel or
	text adventure). Therefore it contains the various :class:`Scene`, global :class:`Resource` and
	global :class:`Action` instances of the game. It also keeps track of the starting scene and the
	current scene.

	:attributes:
	- `name`: A :class:`string` with the name of the game.
	- `author`:  :class:`string` with the author(s) of the game.
	- `startingScene`: A :class:`Scene` object which is the first scene that the game will load.
	- `currentScene`: A :class:`Scene` object which is the scene the player is currently in.
	- `scenes`: A :class:`list` of :class:`Scene` objects which holds all of the scenes of the game, along with their corresponding attributes.
	- `globalResources`: A :class:`list` of :class:`Resource` for storing global resources, such as player HP.
	- `globalActions`: A :class:`list` of :class:`Action` for storing global actions, such as actions which are	always available to the player (e.g. `'help'`) or actions such as checking if the player's HP falls under a certain	threshold.
	- `narrative`: A :class:`list` of narrative elements (e.g. a :class:`string` equal to "You go south.").

	"""
	def __init__(self):
		"""Initialise a :class:`Game` object."""
		self.name = ""
		self.author = ""
		self.startingScene = None
		self.currentScene = None
		self.scenes = []
		self.globalResources = []
		self.globalActions = []
		self.narrative = []
	def addResource(self, name, value):
		"""
		Initialise a :class:`Resource` object and add it to the game's Global Resources :class:`list` `globalResources`.

		:parameters:
		- `name`: The name of the new resource.
		- `value`: The value of the new resource.
		"""
		self.globalResources.append(Resource(name, value))
	def removeResource(self, resource):
		"""
		Remove a :class:`Resource` object from the the game's Global Resources :class:`list` `globalResources`.

		:parameters:
		- `resource`: The :class:`Resource` object to be removed.
		"""
		self.globalResources.remove(resource)
	def getSceneByName(self, name):
		"""
		Finds and returns a :class:`Scene` object given a :class:`string` with the name of the scene.

		:parameters:
		- `name`: A :class:`string` containing the name of the scene.

		:exceptions:
		- "ERROR: Scene not found." if no :class:`Scene` which matches `name` is found.

		:Returns:
		- The first :class:`Scene` object in `scenes` with a name that matches `name`. 
		"""
		firstScene = [scene for scene in self.scenes if scene.name == name]
		if len(firstScene) == 0:
			print("ERROR: Scene '" + name + "' not found.")
			raise
		else:
			return firstScene[0]
	def getResourceByName(self, name):
		"""
		Finds and returns a :class:`Resource` object given a :class:`string` with the name of the resource. It returns the
		first resource which matches the name, starting by searching through the current scene and then through the global resources.

		:parameters:
		- `name`: A :class:`string` containing the name of the resource.

		:exceptions:
		- "ERROR: Resource not found." if no :class:`Resource` which matches `name` is found.

		:Returns:
		- The first :class:`Resource` object in `globalResources` with a name that matches `name`. 
		"""
		firstResource = [resource for resource in self.currentScene.resources if resource.name == name]
		if len(firstResource) == 0:
			firstResource = [resource for resource in self.globalResources if resource.name == name]
		if len(firstResource) == 0:
			return "ERROR: Resource '" + name + "' not found."
		else:
			return firstResource[0]

class Resource:
	"""
	The :class:`Resource` class represents global information, such as the date or the weather
	in the fictional world, or values like the health state of the player. It is basically a tuple,
	containing a `name` attribute and a `value` attribute.

	:attributes:
	- `name`: A :class:`string` which holds the name of the :class:`Resource`, e.g. 'HP'.
	- `value`: This attribute holds the actual value of the resource, which can be of any type.	For example, if the resource represents the player's HP, the `value` of the :class:`Resource` can be an :class:`int` of value 10.
	"""
	def __init__(self, name, value):
		"""
		Initialise a :class:`Resource` object.
		"""
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
	The :class:`Scene` class represents a location or a state in the game. Just like a :class:`Game` object
	contains lists of global resources and global actions, a :class:`Scene` object contains lists of local resources
	and local actions.

	:attributes:
	- `name`: A :class:`string` containing the name of the scene (e.g. "Cave 1").
	- `description`: A :class:`string` containing the description of the scene, which is used in the narrative of the game (e.g. "There are two doors in front of you".)
	- `resources`: A :class:`list` of :class:`Resource` objects known as local resources of a scene.
	- `actions`: A :class:`list` of :class:`Action` objects known as local actions of a scene.
	"""
	def __init__(self, description='default description', name='default room'):
		"""
		Initialise a :class:`Scene` object.
		"""
		self.name = name
		self.description = description
		self.resources = []
		self.actions = []
	def addResource(self, name, value):
		"""
		Initialise a :class:`Resource` object and add it to the scene's local Resources :class:`list` `resources`.

		:parameters:
		- `name`: The name of the new resource.
		- `value`: The value of the new resource.
		"""
		self.resources.append(Resource(name, value))
	def removeResource(self, resource):
		"""
		Remove a :class:`Resource` object from the the scene's local Resources :class:`list` `resources`.

		:parameters:
		- `resource`: The :class:`Resource` object to be removed.
		"""
		self.resources.remove(resource)

class Action:
	"""
	The :class:`Action` class represents an action that either occurs by itself or can be triggered by the player.
	An action has three switches which can be toggled:
	- `Visible`: if an action is visible it means that the player can see it as an available action.
	- `Enabled`: if an action is enabled it means that it can be viewed and triggered.
	- `Active`: if an action is active it means that it needs to be triggered by the player, otherwise it is performed on every frame.

	:attributes:
	- `name`: A :class:`string` containing the name of the action (e.g. "Go south").
	- `conditions`: A :class:`list` containing the :class:`Condition` objects that need to be evaluated before the action is performed.
	- `effectsIfTrue`: A :class:`list` containing the :class:`Effect` objects that will be resolved if all of the `conditions` return ``True``.
	- `effectsIfFalse`: A :class:`list` containing the :class:`Effect` objects that will be resolved if all of the `conditions` do NOT return ``True``.
	- `visible`: A :class:`bool` representing whether the action is visible or not.
	- `enabled`: A :class:`bool` representing whether the action is enabled or not.
	- `active`: A :class:`bool` representing whether the action is active or not.
	"""
	def __init__(self, name='default action', visible=True, enabled=True, active=True, effectsIfTrue=[], effectsIfFalse=[], conditions=[]):
		"""Initialise an :class:`Action` object."""
		self.name = name
		self.keywords = []
		self.conditions = conditions
		self.effectsIfTrue = effectsIfTrue
		self.effectsIfFalse = effectsIfFalse
		self.visible = visible
		self.enabled = enabled
		self.active = active
	def conditionsAreTrue(self):
		"""
		Checks whether all of the :class:`Condition` objects in `condition` are evaluated as ``True``.

		:Returns:
		- ``True`` if all conditions are evaluated as ``True``.
		- ``False`` otherwise.
		"""
		booleanCount = sum( [condition.evaluate() for condition in self.conditions] )
		return booleanCount == len(self.conditions)
	def perform(self):
		"""
		Resolves the :class:`Effect` objects from `effectsIfTrue` if :func:`conditionsAreTrue` returns ``True``.
		Resolves the :class:`Effect` objects from `effectsIfFalse` if :func:`conditionsAreTrue` returns ``False`.
		"""
		if self.conditionsAreTrue():
			for effect in self.effectsIfTrue:
				effect.resolve()
		else:
			for effect in self.effectsIfFalse:
				effect.resolve()

class Condition:
	"""
	The :class:`Condition` class is used in an :class:`Action` object to create conditional evaluation of actions.

	:attributes:
	- `conditionFunction`: A function which is used as an operator to compare `args`.
	- `args`: A :class:`list` containing the arguments for the conditional (usually a left-hand side and a right-hand side element).
	- `rawArgs`: A :class:`list` containing the names of the arguments from `args`.
	- `evalArgs`: A :class:`list` which is populated with the actual values of `args`, which are evaluated during run-time.
	"""
	def __init__(self, conditionFunction=None, *args):
		"""Initialise a :class:`Condition` object."""
		self.conditionFunction = conditionFunction
		self.args = args
		self.rawArgs = []
		self.evalArgs = []
	def evaluate(self):
		"""
		Evaluate the condition.

		:Returns:
		- A :class:`bool`.
		"""
		self.evalArgs = [arg(n) for arg,n in zip(self.args, self.rawArgs)]
		return self.conditionFunction(*self.evalArgs)

class Effect:
	"""
	The :class:`Effect` class is used in an :class:`Action` object to create the output of actions.

	:attributes:
	- `effectFunction`: A function which performs the wanted behaviour of the effect.
	- `args`: A :class:`list` containing the arguments which are passed to `effectFunction`.
	- `rawArgs`: A :class:`list` containing the names of the arguments from `args`.
	- `evalArgs`: A :class:`list` which is populated with the actual values of `args`, which are evaluated during run-time.
	- `parent`: A container of the :class:`Action` object to which the current :class:`Effect` objects belongs.
	"""
	def __init__(self, effectFunction=None, *args):
		""" Initialise an :class:`Effect` object."""
		self.effectFunction = effectFunction
		self.args = args
		self.rawArgs = []
		self.evalArgs = []
		self.parent = None
	def resolve(self):
		"""
		Resolve the effect.
		"""
		self.evalArgs = [self.parent] + [arg(n) for arg,n in zip(self.args, self.rawArgs)]
		self.effectFunction(*self.evalArgs)