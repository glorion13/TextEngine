import core
import customisable
import dataParser

# Load Data
game = dataParser.loadXMLGameData("game.xml")
# Load game

# Print game information
print "------------------------"
print "Game: "  + game.name
print "Author: "+ game.author
print "------------------------"
print ""
print ""

# Start game
game.currentScene = game.startingScene
customisable.effects.dictionary['Tell player'](game.currentScene.description)

#for res in game.globalResources:
#	if res.name == "HP":
#		res.value = 49
game.globalResources.append(core.components.Resource("Hax", 5))

# Core game loop
while (True):
	# Evaluate global actions
	#for action in game.globalActions:
	#	action.perform()
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
	for action in game.globalActions:
		if action.name == userInput:
			action.perform()