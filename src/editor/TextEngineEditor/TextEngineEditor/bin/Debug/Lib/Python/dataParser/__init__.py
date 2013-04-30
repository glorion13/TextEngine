import sys
sys.path.append("C:\\Program Files (x86)\\IronPython 2.7\\Lib")
sys.path.append("Lib\\Python");

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
		"Scene" 	: lambda(n): self.game.getSceneByName(str(n))
		}

	def test(self, file):
		return file

	def loadXMLGameData(self, gameDataFile):
		# Load data
		xmlTree = ElementTree.parse(gameDataFile)
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
		self.game.globalResources = [self.createResourceObject(resource) for resource in globalResourcesNode]
		# Global actions
		globalActionsNode = root.find("GlobalActions")
		self.game.globalActions = [self.createActionObject(action, self.game) for action in globalActionsNode]
		# Scenes
		scenesNode = root.find("Scenes")
		for scene in scenesNode:
			sceneObject = core.components.Scene()
			sceneObject.id = int(scene.find("ID").text)
			sceneObject.name = scene.find("Name").text
			sceneObject.description = scene.find("Description").text
			# Scene resources (local)
			resourcesNode = scene.find("Resources")
			scene.resources = [self.createResourceObject(resource) for resource in resourcesNode]
			# Scene actions (local)
			actionsNode = scene.find("Actions")
			sceneObject.actions = [self.createActionObject(action, self.game) for action in actionsNode]
			self.game.scenes.append(sceneObject)
		self.game.startingScene = self.game.getSceneByName(str(startingScene))
		return self.game

	# Auxiliary parsing functions
	def createActionObject(self, action, game):
		actionObject = core.components.Action(action.find("Name").text, bool(action.find("IsVisible").text))
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