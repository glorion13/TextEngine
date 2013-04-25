def getSceneByID(game, idV):
	firstScene = [scene for scene in game.scenes if scene.id == idV]
	if len(firstScene) == 0:
		return "No scene found"
	else:
		return firstScene[0]
def getSceneByName(game, name):
	firstScene = [scene for scene in game.scenes if scene.name == name]
	if len(firstScene) == 0:
		return "No scene found"
	else:
		return firstScene[0]

def getResourceByID(game, idV):
	firstResource = [resource for resource in game.globalResources if resource.id == idV]
	if len(firstResource) == 0:
		return "No resource found"
	else:
		return firstResource[0]
def getResourceByName(game, name):
	firstResource = [resource for resource in game.globalResources if resource.name == name]
	if len(firstResource) == 0:
		return "No resource found"
	else:
		return firstResource[0]