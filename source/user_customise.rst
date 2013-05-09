.. _scripted_components:

How to extend TAINT with custom functions?
==============================================

Basic ideas
--------------

It is possible to easily customise effects and conditions for your games by editing the :mod:`core.components.customisable.effects` and :mod:`core.components.customisable.conditions` modules. All you need is some basic Python understanding!

Let's go through some examples of creating custom effects. First of all, open :file:`effects.py` from the :mod:`core.components.customisable.effects` module, in your favourite text editor. You should be able to see a list of Python functions as well as a dictionary called `effectDict`.

The `effectDict` dictionary looks something like this::

	effectDict = {
		'Tell player'		: cmdOutputText,
		'Go to scene'		: goToScene,
		'Add global resource'	: addGlobalResource
	}

What it specifies is an effect function name and its corresponding function. The name is used in the `game model` and therefore is displayed in the default editor.

So, from the above `effectDict` example, we can see that three functions are defined. Indeed, if you look through the functions in :file:`effects.py` you should be able to find :func:`cmdOutputText`, :func:`goToScene` and :func:`addGlobalResource`.

That's all there really is to creating your custom functions! You need to write the function as you normally would in Python::

    def cmdOutputText(self, text):
        self.narrative += [text]

and then you need to make sure that you have added a new entry for this function to the `effectDict`.

Passing parameters to a function
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

For some effects, it might be necessary to pass specific parameters which are related to the game. In order to do this, parameters need to follow a specific naming convention:

	- Global resources need to be called `gresource`.
	- Local resources need to be called `lresource`.
	- Any form of textual information needs to be called `text`.
	- If something can potentially be `text`, `number` or `boolean` then the parameter needs to be called `primitive`.

More advanced
----------------

If you feel comfortable with these ideas, then the next step would be to have a look at the :ref:`developer_documentation` section. It should give you all the information you need to even build your own interpreter or editor - or even change the very core of the system entirely! It's really up to you.