from customisable.effects import dictionary as effectDictionary

def uniquesOnly(seq):
	"""
	function:: uniquesOnly(seq)
	Return the factorial of n, an exact integer >= 0.
    If the result is small enough to fit in an int, return an int.
    Else return a long.
    """
	# Source code from Peter Bengtsson (http://www.peterbe.com/plog/uniqifiers-benchmark)
    seen = set()
    seen_add = seen.add
    return [ x for x in seq if x not in seen and not seen_add(x)]

def getLinkedScenes(scene):
	linkedScenesFromTrue = [effect.args[1] for action in scene.actions for effect in action.effectsIfTrue if effect.effectFunction == effectDictionary.get('goToScene')]
	linkedScenesFromFalse = [effect.args[1] for action in scene.actions for effect in action.effectsIfFalse if effect.effectFunction == effectDictionary.get('goToScene')]
	return uniquesOnly(linkedScenesFromTrue + linkedScenesFromFalse)