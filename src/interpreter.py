import core
import customisable

# Load data

# Initialise game
game = core.components.Hierarchy()

# Global resources
hp = core.components.Resource('HP', 10)
# Structure global resources
game.globalResources.append(hp)

# Global actions
showVisibleScenesAction = core.components.Action(\
	name="Show visible actions",
	effectsIfTrue=[core.components.Effect(lambda: customisable.effects.outputVisibleSceneActions(game.currentScene))],
	effectsIfFalse=[],
	conditions=[],
	visible=False)
# Structure global actions
game.globalActions.append(showVisibleScenesAction)

# Initialise scene data
scene1 = core.components.Scene(\
	description="You are in a cave. The rocks are glistening. They are glistener rocks.",
	name='A cave')
scene2 = core.components.Scene(\
	description="You're dead.",
	name='Death')
action1 = core.components.Action(\
	name='Go south',
	effectsIfTrue=[core.components.Effect(customisable.effects.goToScene, game, scene2)],
	effectsIfFalse=[core.components.Effect(customisable.effects.cmdOutputText, "You suck!")],
	conditions=[core.components.Condition(conditionType=customisable.conditions.dictionary.get('equals'), leftHandSide=game.globalResources[0].value, rightHandSide=10)])
action2 = core.components.Action(\
	name='Go north',
	effectsIfTrue=[core.components.Effect(customisable.effects.goToScene, game, scene2)],
	effectsIfFalse=[core.components.Effect(customisable.effects.cmdOutputText, "You suck!")],
	conditions=[core.components.Condition(conditionType=customisable.conditions.dictionary.get('equals'), leftHandSide=game.globalResources[0].value, rightHandSide=10)])
# Structure scene data
scene1.actions.append(action1)
scene1.actions.append(action2)
game.startingScene = scene1

# Start up scene
if game.currentScene == None:
	customisable.effects.goToScene(game, game.startingScene)
else:
	customisable.effects.goToScene(game, game.currentScene)

# core game loop
while (True):
	# Evaluate global actions
	for action in game.globalActions:
		action.perform()
	# Wait for user action
	try:
		userInput = raw_input('\n>')
	except:
		print "Input error."
		break
	# Evaluate user action
	if not (userInput in [action.name for action in game.currentScene.actions]) and not (userInput in  [action.name for action in game.globalActions]):
		print "Command not recognised."
	for action in game.currentScene.actions:
		if action.name == userInput:
			action.perform()