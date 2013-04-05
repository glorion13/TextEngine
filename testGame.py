import main
import effects

# Load data
game = main.game
game.globalResources.append(main.Resource('HP', 10))
scene1 = main.Scene("You are in a cave. The rocks are glistening. They are glistener rocks.", name='A cave')
scene2 = main.Scene("You're dead.", name='Death')
action1 = main.Action('Go south', [main.Effect('effects.goToScene(game, scene2)')])
scene1.actions.append(action1)
game.startingScene = scene1

# Main game loop
while (True):
	# Initialise
	if game.currentScene == None:
		effects.goToScene(game, game.startingScene)
	# Evaluate global actions
	for action in game.globalActions:
		action.perform()
	# Output possible actions
	effects.outputSceneActions(game.currentScene)
	# Wait for user action
	try:
		userInput = raw_input('What do you do?\n')
	except:
		print "Input error."
		break
	# Evaluate user action
	for action in game.currentScene.actions:
		if action.name == userInput:
			action.perform()