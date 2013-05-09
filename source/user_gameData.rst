How is a game stored?
=========================

Games using the TAINT model can be created in whatever way you prefer, as long as they adhere to the game model and are stored as an XML file.

This means that one could potentially build games writing pure XML, although it's probably much simpler to do it using some program with a user interface.

The XML structure is only necessary if you wish to do more advanced stuff rather than just using the given `editor` and `interpreter`.

Generally speaking, the overall idea is broken down into 3 modules: the `model`, the `editor` and the `interpreter`. While we provide an `editor` (a program that allows you to graphically create games) and two `interpreters` (a program that allows you to play the games), anyone is welcome to build or use their own. The only important thing is that they adhere to the `model` specifications as explained in the :ref:`game_model` section.

Examples
-------------

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