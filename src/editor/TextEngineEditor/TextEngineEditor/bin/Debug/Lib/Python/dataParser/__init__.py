from xml.etree import ElementTree
import core
import searching
import customisable
def loadXMLGameData(gameDataFile):
	# Load data
	try:
		xmlTree = ElementTree.parse(gameDataFile)
	except:
		"Error loading game file. Make sure the XML data is correct."

	root = xmlTree.getroot()
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
		actionObject = core.components.Action()
		actionObject.name = action.find("Name").text
		actionObject.visible = bool(action.find("IsVisible").text)
		actionObject.conditions = []
		actionObject.effectsIfTrue = []
		actionObject.effectsIfFalse = []
		conditionsNode = action.find("Conditions")
		effectsTrueNode = action.find("EffectsIfTrue")
		effectsFalseNode = action.find("EffectsIfFalse")

		for condition in conditionsNode:
			leftHandSideNode = condition.find("LeftHandSide")
			lhsType = leftHandSideNode.attrib.get('Type')
			if lhsType == "Boolean":
				leftHandSide = bool(leftHandSideNode.text)
			elif lhsType == "Number":
				leftHandSide = float(leftHandSideNode.text)
			elif lhsType == "Text":
				leftHandSide = str(leftHandSideNode.text)
			elif lhsType == "Resource":
				leftHandSide = searching.getResourceByID(game, int(leftHandSideNode.text))
			rightHandSideNode = condition.find("RightHandSide")
			rhsType = rightHandSideNode.attrib.get('Type')
			if rhsType == "Boolean":
				rightHandSide = bool(rightHandSideNode.text)
			elif rhsType == "Number":
				rightHandSide = float(rightHandSideNode.text)
			elif rhsType == "Text":
				rightHandSide = str(rightHandSideNode.text)
			elif rhsType == "Resource":
				rightHandSide = searching.getResourceByID(game, int(rightHandSideNode.text))
			conditionFunction = customisable.conditions.dictionary[condition.find("ConditionFunction").text]
			conditionObject = core.components.Condition()
			conditionObject.args = [leftHandSide, rightHandSide]
			conditionObject.conditionFunction = conditionFunction
			actionObject.conditions.append(conditionObject)

		for effect in effectsTrueNode:
			argsNode = effect.find("args")
			args = []
			for arg in argsNode:
				argType = arg.attrib.get('Type')
				if argType == "Boolean":
					args.append(bool(arg.text))
				elif argType == "Number":
					args.append(float(arg.text))
				elif argType == "Text":
					args.append(str(arg.text))
				elif argType == "Resource":
					args.append(searching.getResourceByID(game, int(arg.text)))
				elif argType == "Scene":
					args.append(searching.getSceneByID(game, int(arg.text)))
			effectFunction = customisable.effects.dictionary[effect.find("EffectFunction").text]
			effectObject = core.components.Effect()
			effectObject.args = args
			effectObject.effectFunction = effectFunction
			actionObject.effectsIfTrue.append(effectObject)

		for effect in effectsFalseNode:
			argsNode = effect.find("args")
			args = []
			for arg in argsNode:
				argType = arg.attrib.get('Type')
				if argType == "Boolean":
					args.append(bool(arg.text))
				elif argType == "Number":
					args.append(float(arg.text))
				elif argType == "Text":
					args.append(str(arg.text))
				elif argType == "Resource":
					args.append(searching.getResourceByID(game, int(arg.text)))
				elif argType == "Scene":
					args.append(searching.getSceneByID(game, int(arg.text)))
			effectFunction = customisable.effects.dictionary[effect.find("EffectFunction").text]
			effectObject = core.components.Effect()
			effectObject.args = args
			effectObject.effectFunction = effectFunction
			actionObject.effectsIfFalse.append(effectObject)

		game.globalActions.append(actionObject)
		actionObject = []

	# Scenes
	scenesNode = root.find("Scenes")
	for scene in scenesNode:
		sceneObject = core.components.Scene()
		sceneObject.name = scene.find("Name").text
		sceneObject.description = scene.find("Description").text
		actionsNode = scene.find("Actions")

		for action in actionsNode:
			actionObject = core.components.Action()
			actionObject.name = action.find("Name").text
			actionObject.visible = bool(action.find("IsVisible").text)
			actionObject.conditions = []
			actionObject.effectsIfTrue = []
			actionObject.effectsIfFalse = []
			# TODO: Local resources

			conditionsNode = action.find("Conditions")
			for condition in conditionsNode:
				leftHandSide = ""
				rightHandSide = ""
				leftHandSideNode = condition.find("LeftHandSide")
				lhsType = leftHandSideNode.attrib.get('Type')
				if lhsType == "Boolean":
					leftHandSide = bool(leftHandSideNode.text)
				elif lhsType == "Number":
					leftHandSide = float(leftHandSideNode.text)
				elif lhsType == "Text":
					leftHandSide = str(leftHandSideNode.text)
				elif lhsType == "Resource":
					leftHandSide = searching.getResourceByID(game, int(leftHandSideNode.text))
				rightHandSideNode = condition.find("RightHandSide")
				rhsType = rightHandSideNode.attrib.get('Type')
				if rhsType == "Boolean":
					rightHandSide = bool(rightHandSideNode.text)
				elif rhsType == "Number":
					rightHandSide = float(rightHandSideNode.text)
				elif rhsType == "Text":
					rightHandSide = str(rightHandSideNode.text)
				elif rhsType == "Resource":
					rightHandSide = searching.getResourceByID(game, int(rightHandSideNode.text))
				conditionFunction = customisable.conditions.dictionary[condition.find("ConditionFunction").text]
				conditionObject = core.components.Condition()
				conditionObject.args = [leftHandSide, rightHandSide]
				conditionObject.conditionFunction = conditionFunction
				actionObject.conditions.append(conditionObject)

			effectsTrueNode = action.find("EffectsIfTrue")
			for effect in effectsTrueNode:
				argsNode = effect.find("args")
				args = []
				for arg in argsNode:
					argType = arg.attrib.get('Type')
					if argType == "Boolean":
						args.append(bool(arg.text))
					elif argType == "Number":
						args.append(float(arg.text))
					elif argType == "Text":
						args.append(str(arg.text))
					elif argType == "Resource":
						args.append(searching.getResourceByID(game, int(arg.text)))
					elif argType == "Scene":
						args.append(searching.getSceneByID(game, int(arg.text)))
				effectFunction = customisable.effects.dictionary[effect.find("EffectFunction").text]
				effectObject = core.components.Effect()
				effectObject.args = args
				effectObject.effectFunction = effectFunction
				actionObject.effectsIfTrue.append(effectObject)

			effectsFalseNode = action.find("EffectsIfFalse")
			for effect in effectsFalseNode:
				argsNode = effect.find("args")
				args = []
				for arg in argsNode:
					argType = arg.attrib.get('Type')
					if argType == "Boolean":
						args.append(bool(arg.text))
					elif argType == "Number":
						args.append(float(arg.text))
					elif argType == "Text":
						args.append(str(arg.text))
					elif argType == "Resource":
						args.append(searching.getResourceByID(game, int(arg.text)))
					elif argType == "Scene":
						args.append(searching.getSceneByID(game, int(arg.text)))
				effectFunction = customisable.effects.dictionary[effect.find("EffectFunction").text]
				effectObject = core.components.Effect()
				effectObject.args = args
				effectObject.effectFunction = effectFunction
				actionObject.effectsIfFalse.append(effectObject)

			sceneObject.actions.append(actionObject)
		game.scenes.append(sceneObject)

	game.startingScene = searching.getSceneByID(game, int(startingScene))
	return game