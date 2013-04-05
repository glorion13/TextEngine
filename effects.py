# External (Output) effects
def cmdOutputText(text):
	print text

# Internal effects
def editResource(resource, value):
	resource.value = value
def goToScene(hierarchy, scene):
	hierarchy.currentScene = scene
	cmdOutputText(scene.description)
def outputSceneActions(scene):
	possibleActions = [ action.name for action in scene.actions ]
	cmdOutputText(str(possibleActions))