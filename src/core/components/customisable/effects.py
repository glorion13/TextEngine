class EffectFunctions:
    def cmdOutputText(self, text):
        print text
    def goToScene(self, scene):
    	self.currentScene = scene
    	self.cmdOutputText(scene.description)
    def outputVisibleSceneActions(self, scene):
    	possibleActions = [ action.name for action in scene.actions if action.visible ]
    	if not possibleActions == []:
    		self.cmdOutputText(str(possibleActions))
    def addGlobalResource(self, text, primitive):
        self.addResource(text, primitive)
    def editGlobalResource(self, resource, primitive):
        res = self.getResourceByName(resource)
        res.value = primitive
    def deleteGlobalResource(self, resource):
        res = self.getResourceByName(resource)
        self.removeResource(res)
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

    effectDict = {
        'Tell player'                   : cmdOutputText,
        'Add global resource'           : addGlobalResource,
        'Edit global resource'          : editGlobalResource,
        'Delete global resource'        : deleteGlobalResource,
        'Enable action'                 : enableAction,
        'Disable action'                : disableAction,
        'Make action visible'           : makeActionVisible,
        'Make action invisible'         : makeActionInvisible,
        'Go to scene'                   : goToScene,
        'Display all visible actions'   : outputVisibleSceneActions,
        'Display all actions'           : outputAllActions
    }