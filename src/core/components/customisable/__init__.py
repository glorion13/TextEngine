import effects
import conditions

"""
Functions for C# interface
"""

def getEffectCount():
    return len(effects.EffectFunctions().effectDict)
def getConditionCount():
    return len(conditions.ConditionFunctions().conditionDict)

def getEffectKeys():
    return [entry for entry in effects.EffectFunctions().effectDict]
def getConditionKeys():
    return [entry for entry in conditions.ConditionFunctions().conditionDict]