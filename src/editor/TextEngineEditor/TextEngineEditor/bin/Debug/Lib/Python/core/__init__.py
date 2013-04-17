import components

"""
Functions for C# interface
"""

def getGame():
    return components.Game();

def getResource(name, value):
    return components.Resource(name, value);

def getScene():
    return components.Scene();

def getScene(description):
    return components.Scene(description, name);

def getScene(description, name):
    return components.Scene(description, name);

def getAction():
    return components.Action();

def getCondition():
    return components.Condition();

def getEffect(effectFunction):
    return components.Effect(effectFunction);