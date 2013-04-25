from xml.etree import ElementTree

import core

class GameParser:
	def __init__(self):
		self.game = None
		self.typeConverter = {
		"Boolean" 	: lambda(n): bool(n),
		"Number" 	: lambda(n): float(n),
		"Text" 		: lambda(n): str(n),
		"Resource" 	: lambda(n): self.game.getResourceByName(str(n)),
		"Scene" 	: lambda(n): self.game.getSceneByID(int(n))
		}

	def loadXMLGameData(self, gameDataFile):
		# Load data
		try:
			xmlTree = ElementTree.parse(gameDataFile)
		except:
			"Error loading game file. Make sure the XML data is correct."
		root = xmlTree.getroot()
		# Main game data
		self.game = core.components.Game()
		gameName = root.find("GameName").text
		gameAuthor = root.find("Author").text
		startingScene = root.find("StartingScene").text
		self.game.name = gameName
		self.game.author = gameAuthor
		# Global resources
		globalResourcesNode = root.find("GlobalResources")
		for resource in globalResourcesNode:
			resourceObject = self.createResourceObject(resource)
			self.game.globalResources.append(resourceObject)
		# Global actions
		globalActionsNode = root.find("GlobalActions")
		for action in globalActionsNode:
			actionObject = self.createActionObject(action, self.game)
			self.game.globalActions.append(actionObject)
		# Scenes
		scenesNode = root.find("Scenes")
		for scene in scenesNode:
			sceneObject = core.components.Scene()
			sceneObject.id = int(scene.find("ID").text)
			sceneObject.name = scene.find("Name").text
			sceneObject.description = scene.find("Description").text
			sceneObject.actions = []
			sceneObject.resources = []
			# Scene resources (local)
			#resourcesNode = scene.find("Resources")
			#for resource in resourcesNode:
			#	resourceObject = self.createResourceObject(resource)
			#	scene.resources.append(resourceObject)
			# Scene actions (local)
			actionsNode = scene.find("Actions")
			for action in actionsNode:
				actionObject = self.createActionObject(action, self.game)
				sceneObject.actions.append(actionObject)
			self.game.scenes.append(sceneObject)
		self.game.startingScene = self.game.getSceneByID(int(startingScene))
		return self.game

	# Auxiliary parsing functions
	def createActionObject(self, action, game):
		actionObject = core.components.Action()
		actionObject.name = action.find("Name").text
		actionObject.visible = bool(action.find("IsVisible").text)
		actionObject.conditions = []
		actionObject.effectsIfTrue = []
		actionObject.effectsIfFalse = []
		# Conditions
		conditionsNode = action.find("Conditions")
		for condition in conditionsNode:
			conditionObject = self.createConditionObject(condition, game)
			actionObject.conditions.append(conditionObject)
		# Effects if True
		effectsTrueNode = action.find("EffectsIfTrue")
		for effect in effectsTrueNode:
			effectObject = self.createEffectObject(effect, game)
			actionObject.effectsIfTrue.append(effectObject)
		# Effects if False
		effectsFalseNode = action.find("EffectsIfFalse")
		for effect in effectsFalseNode:
			effectObject = self.createEffectObject(effect, game)
			actionObject.effectsIfFalse.append(effectObject)
		return actionObject

	def createEffectObject(self, effect, game):
		argsNode = effect.find("args")
		args = []
		raw = []
		for arg in argsNode:
			argType = arg.attrib.get('Type')
			args.append(self.typeConverter[argType])
			raw.append(arg.text)
		effectObject = core.components.Effect()
		effectObject.args = args
		effectObject.rawArgs = raw
		effectObject.parent = game
		effectObject.effectFunction = game.dictionary[effect.find("EffectFunction").text]
		return effectObject

	def createConditionObject(self, condition, game):
		leftHandSide = ""
		rightHandSide = ""
		leftHandSideNode = condition.find("LeftHandSide")
		leftHandSide = self.typeConverter[leftHandSideNode.attrib.get('Type')]
		rightHandSideNode = condition.find("RightHandSide")
		rightHandSide = self.typeConverter[rightHandSideNode.attrib.get('Type')]
		conditionObject = core.components.Condition()
		conditionObject.args = [leftHandSide, rightHandSide]
		conditionObject.rawArgs = [leftHandSideNode.text, rightHandSideNode.text]
		conditionObject.conditionFunction = core.components.customisable.conditions.dictionary[condition.find("ConditionFunction").text]
		return conditionObject

	def createResourceObject(self, resource):
		resourceName = resource.find("Name").text
		resourceType = resource.find("Type").text
		resourceValue = ""
		if resourceType == "Boolean":
			resourceValue = bool(resource.find("Value").text)
		elif resourceType == "Number":
			resourceValue = float(resource.find("Value").text)
		elif resourceType == "Text":
			resourceValue = str(resource.find("Value").text)
		resourceObject = core.components.Resource(resourceName, resourceValue)
		return resourceObject