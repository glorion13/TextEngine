class EffectFunctions:
    def cmdOutputText(self, text):
        print text
    def editResource(self, resource, value):
        resource.value = value
    def goToScene(self, scene):
    	self.currentScene = scene
    	self.cmdOutputText(scene.description)
    def outputVisibleSceneActions(self, scene):
    	possibleActions = [ action.name for action in scene.actions if action.visible ]
    	if not possibleActions == []:
    		self.cmdOutputText(str(possibleActions))
    def addResource(self):
        return 0
    def editResource(self):
        return 0
    def deleteResource(self):
        return 0
    def enableAction(self):
        return 0
    def disableAction(self):
        return 0
    def makeActionVisible(self):
        return 0
    def makeActionInvisible(self):
        return 0
    def outputAllActions(self):
        return 0

    dictionary = {
        'Tell player'                   : cmdOutputText,
        'Add resource'                  : addResource,
        'Edit resource'                 : editResource,
        'Delete resource'               : deleteResource,
        'Enable action'                 : enableAction,
        'Disable action'                : disableAction,
        'Make action visible'           : makeActionVisible,
        'Make action invisible'         : makeActionInvisible,
        'Go to scene'                   : goToScene,
        'Display all visible actions'   : outputVisibleSceneActions,
        'Display all actions'           : outputAllActions
    }