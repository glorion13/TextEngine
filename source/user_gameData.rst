.. _game_xml:

How is a game stored?
=========================

Games using the TAINT model can be created in whatever way you prefer, as long as they adhere to the game model and are stored as an XML file.

This means that one could potentially build games writing pure XML, although it's probably much simpler to do it using some program with a user interface.

The XML structure is only necessary if you wish to do more advanced stuff rather than just using the given `editor` and `interpreter`.

Generally speaking, the overall idea is broken down into 3 modules: the `model`, the `editor` and the `interpreter`. While we provide an `editor` (a program that allows you to graphically create games) and two `interpreters` (a program that allows you to play the games), anyone is welcome to build or use their own. The only important thing is that they adhere to the `model` specifications as explained in the :ref:`game_model` section.

XML tags
----------

<Game>
^^^^^^^

The main tag which contains all the others is the <Game> tag. It contains the <GameName> the <Author> tags which contain the name of the game (e.g. Escape from the mines) and the name of the author(s). It also contains the <StartingScene> tag which sets the starting scene by name (e.g. Cave 1).

It then contains the <GlobalResources>, <GlobalActions> and <Scenes> tags. The <GlobalResources> contains a number of <Resource> elements which represent the global resources of the game. The <GlobalActions> block contains a number of <Action> elements which represent the global actions of the game. Finally the <Scenes> block contains a block of <Scene> elements.

<Resource>
^^^^^^^^^^^^^

The <Resource> tag requires:
	- A <Name> element which contains the resource's name (e.g. HP).
	- A <Type> element which sets the type of the resource (e.g. Number).
	- A <Value> element which represents the value of said resource (e.g. 20).

For example::

		<Resource>
			<Name>HP</Name>
			<Type>Number</Type>
			<Value>20</Value>
		</Resource>

<Action>
^^^^^^^^^^

The <Action> tag requires:
	- A <Name> element which is the name of the action (e.g. pick up rock).
	- The <Visible>, <Enabled> and <Active> elements, each of which can be set to be either True or False.
	- A <Conditions> block which contains a number of <Condition> elements.
	- An <EffectsIfTrue> block which contains a number of <Effect> elements.
	- An <EffectsIfFalse> block which contains a number of <Effect> elements.

<Condition>
^^^^^^^^^^^^

The <Condition> tag requires:
	- The <ConditionFunction> element which needs to be set to one of the available condition names (e.g. equals).
	- The <LeftHandSide> and <RightHandSide> elements, which requires an attribute `Type` to set the type and then a value of that type to compare.

For example::

	<Condition>
		<ConditionFunction>equals</ConditionFunction>
		<LeftHandSide Type="Resource">HP</LeftHandSide>
		<RightHandSide Type="Number">50</RightHandSide>
	</Condition>

<Effect>
^^^^^^^^^^

The <Effect> tag requires:
	- The <EffectFunction> element which needs to be set to one of the available effect names (e.g. Tell player).
	- The <args> block which contains a number of <arg> elements, representing the arguments passed to the effect function.

Each <arg> element, similarly to the <LeftHandSide> and <RightHandSide> elements of a <Condition>, has a `Type` attribute to set the type of the argument passed to the function and of course a value of that type.

For example::

	<Effect>
		<EffectFunction>Tell player</EffectFunction>
		<args>
			<arg Type="Text">You hear a voice.</arg>
		</args>
	</Effect>

<Scene>
^^^^^^^^^

A <Scene> element contains:
	- A <Name> tag to set the scene's name (e.g. Cave).
	- A <Description> tag to set the scene's narrative when the player enters.
	- A <Resources> block which contains a number of <Resource> elements.
	- An <Actions> block which contains a number of <Action> elements.

Examples
-------------

Example 1
^^^^^^^^^^^

Simple game with 1 scene which contains 1 action and 1 resource::

	<?xml version="1.0" encoding="utf-8"?>
	<Game>
		<GameName>Test Game</GameName>
		<Author>Alex</Author>
		<StartingScene>New Scene</StartingScene>
		<GlobalResources>
		</GlobalResources>
		<GlobalActions>
		</GlobalActions>
		<Scenes>
			<Scene>
				<Name>New Scene</Name>
				<Description>This is a new scene.</Description>
				<Resources>
					<Resource>
						<Name>HP</Name>
						<Type>Number</Type>
						<Value>50</Value>
					</Resource>
				</Resources>
				<Actions>
					<Action>
						<Name>go</Name>
						<Visible>True</Visible>
						<Enabled>True</Enabled>
						<Active>True</Active>
						<Conditions>
							<Condition>
								<ConditionFunction>equals</ConditionFunction>
								<LeftHandSide Type="Resource">HP</LeftHandSide>
								<RightHandSide Type="Number">50</RightHandSide>
							</Condition>
						</Conditions>
						<EffectsIfTrue>
							<Effect>
								<EffectFunction>Tell player</EffectFunction>
								<args>
									<arg Type="Text">Indeed the HP is equal to 50.</arg>
								</args>
							</Effect>
						</EffectsIfTrue>
						<EffectsIfFalse/>
					</Action>
				</Actions>
			</Scene>
		</Scenes>
	</Game>