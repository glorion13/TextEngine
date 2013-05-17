"""
Copyright (c) 2013 ICRL
See the file license.txt for copying permission.
"""

class EffectFunctions:
    """
    This class contains all the customisable functions which are used to represent the output of an action e.g. go to another scene or give information to the player.
    The `effectDict` container is a dictionary containing key-value pairs of a function (e.g. :func:`goToScene`) and its name in a human-readable form, for use in an editor or elsewhere (e.g. 'Go to scene')
    """
    def cmdOutputText(self, text):
        self.narrative += [text]
    def goToScene(self, scene):
        self.currentScene = scene
        self.cmdOutputText(scene.description)
    def outputVisibleSceneActions(self, scene):
        possibleActions = [ action.name for action in scene.actions if action.visible ]
        if not possibleActions == []:
            self.cmdOutputText(str(possibleActions))
    def addGlobalResource(self, text, primitive):
        self.addResource(text, primitive)
    def editGlobalResource(self, gresource, primitive):
        gresource.value = primitive
    def deleteGlobalResource(self, gresource):
        self.removeResource(gresource)
    def addLocalResource(self, text, primitive):
        self.currentScene.addResource(text, primitive)
    def editLocalResource(self, lresource, primitive):
        lresource.value = primitive
    def deleteLocalResource(self, lresource):
        self.currentScene.removeResource(lresource)
    #def enableGlobalAction(self, gaction):
    #    return 0
    #def disableGlobalAction(self, gaction):
    #    return 0
    #def makeGlobalActionVisible(self, gaction):
    #    return 0
    #def makeGlobalActionInvisible(self, gaction):
    #    return 0
    #def enableLocalAction(self, laction):
    #    return 0
    #def disableLocalAction(self, laction):
    #    return 0
    #def makeLocalActionVisible(self, laction):
    #    return 0
    #def makeLocalActionInvisible(self, laction):
    #    return 0

    effectDict = {
        'Tell player'                   : cmdOutputText,
        'Add global resource'           : addGlobalResource,
        'Edit global resource'          : editGlobalResource,
        'Delete global resource'        : deleteGlobalResource,
        'Add local resource'            : addLocalResource,
        'Edit local resource'           : editLocalResource,
        'Delete local resource'         : deleteLocalResource,
        #'Enable global action'          : enableGlobalAction,
        #'Disable global action'         : disableGlobalAction,
        #'Make global action visible'    : makeGlobalActionVisible,
        #'Make global action invisible'  : makeGlobalActionInvisible,
        #'Enable local action'           : enableLocalAction,
        #'Disable local action'          : disableLocalAction,
        #'Make local action visible'     : makeLocalActionVisible,
        #'Make local action invisible'   : makeLocalActionInvisible,
        'Go to scene'                   : goToScene
    }