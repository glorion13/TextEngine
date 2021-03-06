"""
Copyright (c) 2013 ICRL
See the file license.txt for copying permission.
"""

from . import effects
from . import conditions

def getEffectCount():
    return len(effects.EffectFunctions().effectDict)

def getConditionCount():
    return len(conditions.ConditionFunctions().conditionDict)

def getEffectKeys():
    return [entry for entry in effects.EffectFunctions().effectDict]

def getConditionKeys():
    return [entry for entry in conditions.ConditionFunctions().conditionDict]