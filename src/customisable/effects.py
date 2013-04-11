def cmdOutputText(text):
	print text
def editResource(resource, value):
	resource.value = value
def goToScene(hierarchy, scene):
	hierarchy.currentScene = scene
	cmdOutputText(scene.description)
def outputVisibleSceneActions(scene):
	possibleActions = [ action.name for action in scene.actions if action.visible ]
	if not possibleActions == []:
		cmdOutputText(str(possibleActions))

dictionary = {
	'cmdOutputText'			: cmdOutputText,
	'editResource'			: editResource,
	'goToScene'				: goToScene,
	'outputVisibleSceneActions'	: outputVisibleSceneActions,
}