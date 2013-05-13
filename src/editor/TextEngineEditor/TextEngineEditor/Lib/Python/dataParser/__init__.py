from xml.etree import ElementTree

import core

class GameParser:
	"""
	This class can import games from XML files using the :func:`loadXMLGameData` function.

	:Attributes:
	- `game`: holds all the game data after :func:`loadXMLGameData` is called and
	- `typeConverter`: a dictionary which contains key-value pairs between the different types of the game model (e.g. "Number") and a lambda expression to reconstruct it correctly for the game logic (e.g. ``lambda(n): float(n)``). The reason for using lambda expressions is in order to allow for the evaluation of these objects (e.g. if it is a :class:`core.components.Resource`) to be carried out during run-time instead of during initialisation. This means that everything is evaluated as it should, including objects which are created or edited on the fly during gameplay.
	"""
	def __init__(self):
		"""
		The initialisation function which sets up the typeConverter.
		"""
		self.game = None
		self.typeConverter = {
		"Boolean" 	: lambda(n): n,
		"Number" 	: lambda(n): float(n),
		"Text" 		: lambda(n): str(n),
		"Resource" 	: lambda(n): self.game.getResourceByName(str(n)),
		"Scene" 	: lambda(n): self.game.getSceneByName(str(n))
		}

	def stringToBoolean(self, s):
		"""
		Auxiliary function which converts a string ``s`` to a boolean for the purposes of this engine.
		
		:Returns:
		- ``True`` if string ``s`` equals "True".
		- ``False`` otherwise.
		"""
		return s == "True"

	def loadXMLGameData(self, gameDataFile):
		"""
		The main function of this class, which loads an XML game data file into memory.
		It uses the :class:`ElementTree` class from :mod:`xml.etree` to navigate through the XML tree and
		gather all the information.

		:Parameters:
		- `gameDataFile`: a string to the XML file e.g. "game1.xml".

		:Raises:
		- "Error loading game file." is the file is not found or if the file contains bad XML.

		:Returns:
		- A populated :class:`core.components.Game` object.
		"""
		# Load data
		try:
			xmlTree = ElementTree.parse(gameDataFile)
			root = xmlTree.getroot()
		except:
			print("Error loading game file.")
			raise
		# Main game data
		self.game = core.components.Game()
		gameName = root.find("GameName").text
		if (gameName == None):
			gameName = ""
		gameAuthor = root.find("Author").text
		if (gameAuthor == None):
			gameAuthor = ""
		startingScene = root.find("StartingScene").text
		self.game.name = gameName
		self.game.author = gameAuthor
		# Global resources
		globalResourcesNode = root.find("GlobalResources")
		self.game.globalResources = [self.createResourceObject(resource) for resource in globalResourcesNode]
		# Global actions
		globalActionsNode = root.find("GlobalActions")
		self.game.globalActions = [self.createActionObject(action, self.game) for action in globalActionsNode]
		# Scenes
		scenesNode = root.find("Scenes")
		for scene in scenesNode:
			sceneObject = core.components.Scene()
			sceneObject.name = scene.find("Name").text
			sceneObject.description = scene.find("Description").text
			# Scene resources (local)
			resourcesNode = scene.find("Resources")
			sceneObject.resources = [self.createResourceObject(resource) for resource in resourcesNode]
			# Scene actions (local)
			actionsNode = scene.find("Actions")
			sceneObject.actions = [self.createActionObject(action, self.game) for action in actionsNode]
			self.game.scenes.append(sceneObject)
		self.game.startingScene = self.game.getSceneByName(str(startingScene))
		return self.game

	# Auxiliary parsing functions

	def createActionObject(self, action, game):
		"""
		Auxiliary function used by :func:`loadXMLGameData` which creates a :class:`core.components.Action` object.

		:Parameters:
		- `action`: an <action> XML node from the game data file.
		- `game`: the `game` attribute of :class:`GameParser`.

		:Returns:
		- A populated :class:`core.components.Action`.
		"""
		actionObject = core.components.Action(action.find("Name").text, self.stringToBoolean(action.find("Visible").text), self.stringToBoolean(action.find("Enabled").text), self.stringToBoolean(action.find("Active").text))
		# Conditions
		conditionsNode = action.find("Conditions")
		actionObject.conditions = [self.createConditionObject(condition, game) for condition in conditionsNode]
		# Effects if True
		effectsTrueNode = action.find("EffectsIfTrue")
		actionObject.effectsIfTrue = [self.createEffectObject(effect, game) for effect in effectsTrueNode]
		# Effects if False
		effectsFalseNode = action.find("EffectsIfFalse")
		actionObject.effectsIfFalse = [self.createEffectObject(effect, game) for effect in effectsFalseNode]
		return actionObject

	def createEffectObject(self, effect, game):
		"""
		Auxiliary function used by :func:`createActionObject` which creates a :class:`core.components.Effect` object.

		:Parameters:
		- `effect`: an <effect> XML node from the game data file.
		- `game`: the `game` attribute of :class:`GameParser`.

		:Returns:
		- A populated :class:`core.components.Effect`.
		"""
		argsNode = effect.find("args")
		effectObject = core.components.Effect()
		effectObject.args = [self.typeConverter[arg.attrib.get('Type')] for arg in argsNode]
		effectObject.rawArgs = [arg.text for arg in argsNode]
		effectObject.parent = game
		if effect.find("EffectFunction").text == None:
			effectObject.effectFunction = ""
		else:
			effectObject.effectFunction = game.effectDict[effect.find("EffectFunction").text]
		return effectObject

	def createConditionObject(self, condition, game):
		"""
		Auxiliary function used by :func:`createActionObject` which creates a :class:`core.components.Condition` object.

		:Parameters:
		- `condition`: a <condition> XML node from the game data file.
		- `game`: the `game` attribute of :class:`GameParser`.

		:Returns:
		- A populated :class:`core.components.Condition`.
		"""
		# Left hand side element
		leftHandSideNode = condition.find("LeftHandSide")
		if leftHandSideNode.attrib.get('Type') == None or leftHandSideNode.attrib.get('Type') == '':
			leftHandSide = ""
		else:
			leftHandSide = self.typeConverter[leftHandSideNode.attrib.get('Type')]
		# Right hand side element
		rightHandSideNode = condition.find("RightHandSide")
		if rightHandSideNode.attrib.get('Type') == None  or rightHandSideNode.attrib.get('Type') == '':
			rightHandSide = ""
		else:
			rightHandSide = self.typeConverter[rightHandSideNode.attrib.get('Type')]
		# Create condition object
		conditionObject = core.components.Condition()
		conditionObject.args = [leftHandSide, rightHandSide]
		conditionObject.rawArgs = [leftHandSideNode.text, rightHandSideNode.text]
		if condition.find("ConditionFunction").text == None:
			conditionObject.conditionFunction = ""
		else:
			conditionObject.conditionFunction = game.conditionDict[condition.find("ConditionFunction").text]
		return conditionObject

	def createResourceObject(self, resource):
		"""
		Auxiliary function used by :func:`loadXMLGameData` which creates a :class:`core.components.Resource` object.

		:Parameters:
		- `resource`: a <resource> XML node from the game data file.

		:Returns:
		- A populated :class:`core.components.Resource`.
		"""
		resourceType = resource.find("Type").text
		resourceValue = ""
		if resourceType == "Boolean":
			resourceValue = bool(resource.find("Value").text)
		elif resourceType == "Number":
			resourceValue = float(resource.find("Value").text)
		elif resourceType == "Text":
			resourceValue = str(resource.find("Value").text)
		resourceObject = core.components.Resource(resource.find("Name").text, resourceValue)
		return resourceObject