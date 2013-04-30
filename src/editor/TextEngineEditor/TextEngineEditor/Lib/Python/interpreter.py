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
	# Evaluate global actions
	#for action in game.globalActions:
	#	action.perform()
	# Wait for user action
	try:
		userInput = raw_input('\n>')
	except:
		print("Input error.")
		break
	# Evaluate user action
	if not (userInput in [action.name for action in game.currentScene.actions]) and not (userInput in  [action.name for action in game.globalActions]):
		print("Command not recognised.")
	for action in game.currentScene.actions:
		if action.name == userInput:
			action.perform()
	for action in game.globalActions:
		if action.name == userInput:
			action.perform()