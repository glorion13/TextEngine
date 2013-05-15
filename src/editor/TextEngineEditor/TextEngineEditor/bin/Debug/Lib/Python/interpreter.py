import core
import dataParser

# Load Data
parser = dataParser.GameParser()
game = parser.loadXMLGameData("game.xml")
# Load game

# Print game information
print("------------------------")
print("Game: "  + game.name)
print("Author: "+ game.author)
print("------------------------")
print("")
print("")

# Start game
game.effectDict['Go to scene'](game, game.startingScene)

# Core game loop
while (True):
	# Perform passive actions
	for action in [gAction for gAction in game.globalActions if gAction.active == False and gAction.enabled == True]:
		action.perform()
	for action in [lAction for lAction in game.currentScene.actions if lAction.active == False and lAction.enabled == True]:
		action.perform()
	# Display narrative
	for narr in game.narrative:
		print(narr)
	game.narrative = []
	# Display visible actions
	gActs = [gAction.name for gAction in game.globalActions if gAction.active == True and gAction.enabled == True and gAction.visible == True]
	lActs = [lAction.name for lAction in game.currentScene.actions if lAction.active == True and lAction.enabled == True and lAction.visible == True]
	if not (len(gActs) == 0):
		print(gActs)
	if not (len(lActs) == 0):
		print(lActs)
	# Wait for user action
	# Make input() function work on both Python 2.x and 3.x
	try:
		input = raw_input
	except:
		pass
	userInput = input('\n>')
	# Evaluate user action
	lActions = [action for action in game.currentScene.actions if action.enabled == True and action.active == True]
	gActions = [action for action in game.globalActions if action.enabled == True and action.active == True]
	allActions = lActions + gActions
	for action in allActions:
		if (action.name == userInput) or (userInput in action.keywords):
			action.perform()
			break