from xml.etree import ElementTree

import core
import searching

def loadXMLGameData(gameDataFile):
	# Load data
	try:
		xmlTree = ElementTree.parse(gameDataFile)
	except:
		"Error loading game file. Make sure the XML data is correct."
	root = xmlTree.getroot()
	# Main game data
	game = core.components.Game()
	gameName = root.find("GameName").text
	gameAuthor = root.find("Author").text
	startingScene = root.find("StartingScene").text
	game.name = gameName
	game.author = gameAuthor
	# Global resources
	globalResourcesNode = root.find("GlobalResources")
	for resource in globalResourcesNode:
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
		game.globalResources.append(resourceObject)
	# Global actions
	globalActionsNode = root.find("GlobalActions")
	for action in globalActionsNode:
		actionObject = createActionObject(action, game)
		game.globalActions.append(actionObject)
	# Scenes
	scenesNode = root.find("Scenes")
	for scene in scenesNode:
		sceneObject = core.components.Scene()
		sceneObject.id = int(scene.find("ID").text)
		sceneObject.name = scene.find("Name").text
		sceneObject.description = scene.find("Description").text
		# TODO: Local resources
		actionsNode = scene.find("Actions")
		for action in actionsNode:
			actionObject = createActionObject(action, game)
			sceneObject.actions.append(actionObject)
		game.scenes.append(sceneObject)
	game.startingScene = searching.getSceneByID(game, int(startingScene))
	return game

# Auxiliary parsing functions
def createActionObject(action, game):
	actionObject = core.components.Action()
	actionObject.name = action.find("Name").text
	actionObject.visible = bool(action.find("IsVisible").text)
	actionObject.conditions = []
	actionObject.effectsIfTrue = []
	actionObject.effectsIfFalse = []		

	conditionsNode = action.find("Conditions")
	for condition in conditionsNode:
		conditionObject = createConditionObject(condition, game)
		actionObject.conditions.append(conditionObject)

	effectsTrueNode = action.find("EffectsIfTrue")
	for effect in effectsTrueNode:
		effectObject = createEffectObject(effect, game)
		actionObject.effectsIfTrue.append(effectObject)

	effectsFalseNode = action.find("EffectsIfFalse")
	for effect in effectsFalseNode:
		effectObject = createEffectObject(effect, game)
		actionObject.effectsIfFalse.append(effectObject)
	return actionObject

def createEffectObject(effect, game):
	argsNode = effect.find("args")
	args = []
	raw = []
	for arg in argsNode:
		argType = arg.attrib.get('Type')
		if argType == "Boolean":
			args.append(lambda(n): bool(n))
		elif argType == "Number":
			args.append(lambda(n): float(n))
		elif argType == "Text":
			args.append(lambda(n): str(n))
		elif argType == "Resource":
			args.append(lambda(n): searching.getResourceByName(game, str(n)))
		elif argType == "Scene":
			args.append(lambda(n): searching.getSceneByID(game, int(n)))
		raw.append(arg.text)
	effectFunction = game.dictionary[effect.find("EffectFunction").text]
	effectObject = core.components.Effect()
	effectObject.args = args
	effectObject.raw = raw
	effectObject.evalArgs.append(game)
	effectObject.effectFunction = effectFunction
	return effectObject

def createConditionObject(condition, game):
	leftHandSide = ""
	rightHandSide = ""
	leftHandSideNode = condition.find("LeftHandSide")
	lhsType = leftHandSideNode.attrib.get('Type')
	if lhsType == "Boolean":
		leftHandSide = lambda(n): bool(n)
	elif lhsType == "Number":
		leftHandSide = lambda(n): float(n)
	elif lhsType == "Text":
		leftHandSide = lambda(n): str(n)
	elif lhsType == "Resource":
		leftHandSide = lambda(n): searching.getResourceByName(game, str(n))
	rightHandSideNode = condition.find("RightHandSide")
	rhsType = rightHandSideNode.attrib.get('Type')
	if rhsType == "Boolean":
		rightHandSide = lambda(n): bool(n)
	elif rhsType == "Number":
		rightHandSide = lambda(n): float(n)
	elif rhsType == "Text":
		rightHandSide = lambda(n): str(n)
	elif rhsType == "Resource":
		rightHandSide = lambda(n): searching.getResourceByName(game, str(n))
	conditionFunction = core.components.customisable.conditions.dictionary[condition.find("ConditionFunction").text]
	conditionObject = core.components.Condition()
	conditionObject.args = [leftHandSide, rightHandSide]
	conditionObject.raw = [leftHandSideNode.text, rightHandSideNode.text]
	conditionObject.conditionFunction = conditionFunction
	return conditionObject