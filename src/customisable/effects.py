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
def addResource():
    return 0
def editResource():
    return 0
def deleteResource():
    return 0
def enableAction():
    return 0
def disableAction():
    return 0
def makeActionVisible():
    return 0
def makeActionInvisible():
    return 0
def outputAllActions():
    return 0

dictionary = {
	'Tell player'			        : cmdOutputText,
    'Add resource'                  : addResource,
	'Edit resource'			        : editResource,
    'Delete resource'               : deleteResource,
    'Enable action'                 : enableAction,
    'Disable action'                : disableAction,
    'Make action visible'           : makeActionVisible,
    'Make action invisible'         : makeActionInvisible,
	'Go to scene'				    : goToScene,
	'Display all visible actions'	: outputVisibleSceneActions,
    'Display all actions'           : outputAllActions
}