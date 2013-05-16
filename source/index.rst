.. Copyright (c) 2013 ICRL
   See the file license.txt for copying permission.

.. TAINT: Text Adventure and Interactive Novel Toolkit documentation master file, created by
   sphinx-quickstart on Thu Apr 11 13:10:24 2013.
   You can adapt this file completely to your liking, but it should at least
   contain the root `toctree` directive.

Welcome to TAINT's documentation!
===============================================================================

What is TAINT?
-------------------------

`TAINT` (Text Adventure & Interactive Novel Toolkit) is a framework/engine for creating text adventure or interactive novel games. It is created primarily in Python, while an editor is supplied which is written in C# and runs on Windows. Games created with TAINT are cross-platform as there is an interpreter which allows for games to be played directly in a web browser!

If you don't want to use the supplied editor, you're welcome to create your own, building on top of the core framework of TAINT. In fact, TAINT games are just XML so you could even write them directly in XML! It's really very flexible.

Why would I use it?
-------------------------

Did you ever want to create your own interactive novels or text adventures but didn't know where to begin? You don't know any programming but you also don't want to be exposed to super-complicated interfaces? Do you want your games to be cross-platform? Then TAINT is for you! Through a very simple interface, TAINT gives you the potential to create complex, branching experiences!

On the other hand, if you do want to go into the belly of the beast, TAINT is open-source and directly extensible through Python.

How do I get started?
-------------------------

To get started with the basic game model of TAINT as well as to learn how to create your own games, have a look at the :ref:`user_documentation` section. To get started with development or to simply get a deeper under-the-hood understanding of the framework, have a look at the :ref:`developer_documentation`.

.. _user_documentation:

User documentation
===============================================================================

User documentation explains the main ideas behind TAINT and going through it should allow you to start making your own games in no time!

.. toctree::
	:maxdepth: 2

	user_gameModel.rst
	user_gameCreation.rst
	user_gamePlayback.rst
	user_gameData.rst
	user_customise.rst

.. _developer_documentation:

Developer documentation
===============================================================================

Developer documentation contains documentation and source code about what goes on behind the scenes. Reading through it can help you get a real in-depth understanding of the engine as well as get you up and running with the codebase if you want to contribute with its development! It is assumed that you already understand everything in the :ref:`user_documentation` section.

.. toctree::
	:maxdepth: 2
	
	dev_core.rst
	dev_dataParser.rst
	dev_interpreter.rst

Indices and tables
===================

* :ref:`genindex`
* :ref:`modindex`
* :ref:`search`

