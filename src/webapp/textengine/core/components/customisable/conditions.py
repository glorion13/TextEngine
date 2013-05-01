import operator

class ConditionFunctions:

	conditionDict = {
		'equals'			: operator.eq,
		'less than'			: operator.lt,
		'greater than'		: operator.gt,
		'less or equal'		: operator.le,
		'greater or equal'	: operator.le
	}