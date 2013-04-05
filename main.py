import conditionTypes
import effects

class Hierarchy:
	def __init__(self):
		self.startingScene = None
		self.currentScene = None
		self.scenes = []
		self.globalResources = []
		self.globalActions = []

class Resource:
	def __init__(self, name, value):
		self.name = name
		self.value = value

class Scene:
	def __init__(self, description, name='default room'):
		self.name = name
		self.description = description
		self.actions = []

class Action:
	def __init__(self, name='default action', effectsIfTrue=[], effectsIfFalse=[], conditions=[]):
		self.name = name
		self.conditions = conditions
		self.effectsIfTrue = effectsIfTrue
		self.effectsIfFalse = effectsIfFalse
	def conditionsAreTrue(self):
		booleanCount = sum( [condition.evaluate() for condition in self.conditions] )
		return booleanCount == len(self.conditions)
	def perform(self):
		if self.conditionsAreTrue():
			for effect in self.effectsIfTrue:
				effect.resolve()
		else:
			for effect in self.effectsIfFalse:
				effect.resolve()

class Condition:
	def __init__(self, conditionType, leftHandSide, rightHandSide):
		self.conditionType = conditionType
		self.leftHandSide = leftHandSide
		self.rightHandSide = rightHandSide
	def evaluate(self):
		return conditionTypes.dictionary.get(self.conditionType) \
		(self.leftHandSide, self.rightHandSide)

class Effect:
	def __init__(self, effectFunction):
		self.effectFunction = effectFunction
	def resolve(self):
		exec(self.effectFunction)

game = Hierarchy()