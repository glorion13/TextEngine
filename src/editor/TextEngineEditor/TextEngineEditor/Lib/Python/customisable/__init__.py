import effects
import conditions

"""
Functions for C# interface
"""

def getEffectsKeys():
    allKeys = []
    for key in effects.dictionary:
        allKeys.append(key)
    return allKeys

def getConditionsKeys():
    allKeys = []
    for key in conditions.dictionary:
        allKeys.append(key)
    return allKeys

def getEffectsKeysLength():
    return len(getEffectsKeys())

def getConditionsKeysLength():
    return len(getConditionsKeys())

def getEffects():
    return effects.dictionary

def getConditions():
    return conditions.dictionary