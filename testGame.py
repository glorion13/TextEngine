import main
import effects
import conditions

# Load data
game = main.Hierarchy()
game.globalResources.append(main.Resource('HP', 10))
scene1 = main.Scene(\
	description="You are in a cave. The rocks are glistening. They are glistener rocks.",
	name='A cave')
scene2 = main.Scene(\
	description="You're dead.",
	name='Death')
action1 = main.Action(\
	name='Go south',
	effectsIfTrue=[main.Effect(effects.goToScene, game, scene2)],
	effectsIfFalse=[main.Effect(effects.cmdOutputText, "You suck!")],
	conditions=[main.Condition(conditionType=conditions.dictionary.get('equals'), leftHandSide=game.globalResources[0].value, rightHandSide=10)])
scene1.actions.append(action1)
game.startingScene = scene1

# Initialise
if game.currentScene == None:
	effects.goToScene(game, game.startingScene)

# Main game loop
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
	if userInput not in [action.name for action in game.currentScene.actions]:
		print "Command not recognised."
	for action in game.currentScene.actions:
		if action.name == userInput:
			action.perform()

	# Output possible actions
	#effects.outputSceneActions(game.currentScene)