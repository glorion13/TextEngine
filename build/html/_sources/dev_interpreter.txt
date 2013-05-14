interpreter
================

There are two interpreter implementations provided by default and they are explained in this section.

The first interpreter is a pure Python interpreter which makes use of the :mod:`core` module and the :mod:`dataParser` module to playback a game.

The second interpreter is also Python based, but using Google's AppEngine it runs on any browser. This means that games created with TAINT are cross-platform!

The great thing about how modular the whole system is, is that you can work on an interpreter exclusively without the need to go into the :mod:`core` or the :mod:`dataParser` modules, focusing solely on the presentation and playback of a game.

Python interpreter
--------------------

Here you can see the source code of a very simple interpreter writtein in Python::

	import core
	import dataParser

	# Load Data
	parser = dataParser.GameParser()
	game = parser.loadXMLGameData("game.xml")
	# Load game

	# Print game information
	print("------------------------")
	print("Game: "  + game.name)
	print("Author: "+ game.author)
	print("------------------------")
	print("")
	print("")

	# Start game
	game.effectDict['Go to scene'](game, game.startingScene)

	# Core game loop
	while (True):
		# Perform passive actions
		for action in [gAction for gAction in game.globalActions if gAction.active == False and gAction.enabled == True]:
			action.perform()
		for action in [lAction for lAction in game.currentScene.actions if lAction.active == False and lAction.enabled == True]:
			action.perform()
		# Display narrative
		for narr in game.narrative:
			print narr
		game.narrative = []
		# Display visible actions
		gActs = [gAction.name for gAction in game.globalActions if gAction.active == True and gAction.enabled == True and gAction.visible == True]
		lActs = [lAction.name for lAction in game.currentScene.actions if lAction.active == True and lAction.enabled == True and lAction.visible == True]
		if not (len(gActs) == 0): print gActs
		if not (len(lActs) == 0): print lActs
		# Wait for user action
		try:
			userInput = raw_input('\n>')
		except:
			print("Input error.")
			break
		# Evaluate user action
		lActions = [action for action in game.currentScene.actions if action.enabled == True and action.active == True]
		gActions = [action for action in game.globalActions if action.enabled == True and action.active == True]
		allActions = lActions + gActions
		for action in allActions:
			if (action.name == userInput) or (userInput in action.keywords):
				action.perform()
				break

Web interpreter
--------------------

Using Google's AppEngine, it is similarly simple to create an interpreter which works as a web application! Here you can find the sample source code::

	import cgi
	import webapp2
	import urllib

	from google.appengine.api import users
	from google.appengine.ext import db

	import core
	import dataParser


	def createActionsHTML(game_id, game):
	  actionsHTML = ""
	  # Display local visible actions
	  for action in [lAction for lAction in game.currentScene.actions if lAction.active == True and lAction.enabled == True and lAction.visible == True]:
	    actionsHTML += """<form action="/game/"""+game_id+"""" method="post"><div><input name="action" type="submit" value=\""""+action.name+"""\"></div></form>"""
	  # Display global visible actions
	  for action in [gAction for gAction in game.globalActions if gAction.active == True and gAction.enabled == True and gAction.visible == True]:
	    actionsHTML += """<form action="/game/"""+game_id+"""" method="post"><div><input name="action" type="submit" value=\""""+action.name+"""\"></div></form>"""
	  # Display additional actions (new game, logout etc.)
	  actionsHTML += """<form action="/game/"""+game_id+"""" method="post"><div><input name="action" type="submit" value="Start again"></div></form>"""
	  actionsHTML += """<form action="/logout" method="post"><div><input name="action" type="submit" value="Logout"></div></form>"""
	  return actionsHTML

	def createNarrativeHTML(game):
	  narrativeHTML = ""
	  for narr in game.narrative:
	    narrativeHTML += "<br><p>" + narr + "</p>"
	  return narrativeHTML

	def createCombinedHTML(narrativeHTML, actionsHTML):
	  html = """\
	        <html>
	          <body>
	            <p>"""+ game.name +""" by """+ game.author +"""</p>
	            """+ narrativeHTML +"""
	            """+ actionsHTML +"""
	          </body>
	        </html>"""
	  return html

	def createHTML(game_id, game):
	  narrativeHTML = createNarrativeHTML(game)
	  actionsHTML = createActionsHTML(game_id, game)
	  return createCombinedHTML(narrativeHTML, actionsHTML)

	class LogoutPage(webapp2.RequestHandler):
	  def get(self):
	    self.redirect(users.create_logout_url("/"))
	  def post(self):
	    self.redirect(users.create_logout_url("/"))

	class MainPage(webapp2.RequestHandler):
	  def get(self):
	    self.response.write("""<form action="/" method="post"><div><input name="gameChoice" type="submit" value="Game 1"></div></form>""")
	    self.response.write("""<form action="/" method="post"><div><input name="gameChoice" type="submit" value="Game 5"></div></form>""")
	  def post(self):
	    playerInput = cgi.escape(self.request.get('gameChoice'))
	    self.redirect("/game/"+playerInput+"")

	class GamePage(webapp2.RequestHandler):
	  global game
	  global parser
	  global gameDataUrl
	  parser = dataParser.GameParser()

	  def resetGame(self, game_id):
	    global game
	    game = parser.loadXMLGameData(game_id)
	    game.effectDict['Go to scene'](game, game.startingScene)

	  def get(self, game_id):
	    # Make sure user is logged in
	    user = users.get_current_user()
	    if user:
	      # Load game data
	      global game
	      game_id = game_id + ".xml"
	      game = parser.loadXMLGameData(game_id)
	      game.effectDict['Go to scene'](game, game.startingScene)
	      # Perform passive actions
	      for action in [gAction for gAction in game.globalActions if gAction.active == False and gAction.enabled == True]:
	        action.perform()
	      for action in [lAction for lAction in game.currentScene.actions if lAction.active == False and lAction.enabled == True]:
	        action.perform()
	      # Display narrative
	      HTML = createHTML(game_id, game)
	      self.response.write(HTML)
	    else:
	      # If not logged in redirect to login page
	      self.redirect(users.create_login_url(self.request.uri))

	  def post(self, game_id):
	    # Make sure user is logged in
	    user = users.get_current_user()
	    if user:
	      # Evaluate user action
	      playerInput = cgi.escape(self.request.get('action'))
	      # Side actions (e.g. logout, start new game etc.)
	      if playerInput == "Start again":
	        self.resetGame(game_id)
	        HTML = createHTML(game_id, game)
	        self.response.write(HTML)
	      else:
	        # Game actions
	        for action in game.currentScene.actions:
	          if (action.name == playerInput) or (playerInput in action.keywords):
	            action.perform()
	            break
	        # Perform passive actions
	        for action in [gAction for gAction in game.globalActions if gAction.active == False and gAction.enabled == True]:
	          action.perform()
	        for action in [lAction for lAction in game.currentScene.actions if lAction.active == False and lAction.enabled == True]:
	          action.perform()
	        # Display narrative
	        HTML = createHTML(game_id, game)
	        self.response.write(HTML)
	    else:
	      self.redirect(users.create_login_url(self.request.uri))

	def main():
	  app = webapp2.WSGIApplication([('/', MainPage), ('/game/([^/]+)', GamePage), ('/logout', LogoutPage)], debug=True)
	  return app

	app = main()