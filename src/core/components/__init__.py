# Author: Alexandros Gouvatsos <alexandros@gouvatsos.com>
# Copyright: This module has been placed in the public domain.

"""
The core of the system, which is used as the basis for both the Python-based editor and the
Python-based interpreter. This module defines the following classes:

- `Hierarchy`, which represents the hierarchical structure of the game (interactive novel or text adventure).
- `Resource`, which represents global information, such as the date or the weather in the fictional world, or values like the health state of the player.
- `Scene`, which represents a location or a state in the fictional world.
- `Action`, which represents an action that can be triggered by the player. An Action consists of a `Condition` and an `Effect`. The `Effect` is only resolved if the condition is True. An action can be visible (in the case of an interactive novel) or invisible (in the case of a text adventure). In fact, within the same game there might be cases when Actions are visible and cases when actions are invisible.
- `Condition`, which The Condition class represents a logical evaluation which can be part of an Action. When evaluated it returns a value of True or False.
- `Effect`, which represents the outcome of a given Action that the player performs. An Effect can be anything from changing the scene, interacting with the world or even losing the game.
"""

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
	def __init__(self, name='default action', effectsIfTrue=[], effectsIfFalse=[], conditions=[], visible=True):
		self.name = name
		self.conditions = conditions
		self.effectsIfTrue = effectsIfTrue
		self.effectsIfFalse = effectsIfFalse
		self.visible = visible
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
		return self.conditionType(self.leftHandSide, self.rightHandSide)

class Effect:
	def __init__(self, effectFunction, *args):
		self.effectFunction = effectFunction
		self.args = args
	def resolve(self):
		self.effectFunction(*self.args)