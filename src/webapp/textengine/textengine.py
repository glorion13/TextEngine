import cgi
import webapp2
import urllib

from google.appengine.api import users
from google.appengine.ext import db

import core
import dataParser

def createMainPageHTML(game):
  formattedNarrative = ""
  for narr in game.narrative:
    formattedNarrative += "<p>" + narr + "</p>"

  formattedActions = ""
  for action in game.currentScene.actions:
    formattedActions += """<form action="/" method="post"><div><input name="action" type="submit" value=\""""+action.name+"""\"></div></form>"""
  for action in game.globalActions:
    formattedActions += """<form action="/" method="post"><div><input name="action" type="submit" value=\""""+action.name+"""\"></div></form>"""

  formattedActions += """<form action="/" method="post"><div><input name="action" type="submit" value="new_game"></div></form>"""

  return """\
  <html>
    <body>
      <p>"""+ game.name +""" by """+ game.author +"""</p>
      """+ formattedNarrative +"""
      """+ formattedActions +"""
    </body>
  </html>"""

class GamePage(webapp2.RequestHandler):

  global parser
  parser = dataParser.GameParser()
  global game
  game = parser.loadXMLGameData("game.xml")
  game.effectDict['Go to scene'](game, game.startingScene)
  print "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"
  print game.name

  def resetGame(self):
    global game
    game = parser.loadXMLGameData("game.xml")
    game.effectDict['Go to scene'](game, game.startingScene)

  def get(self):
    user = users.get_current_user()
    if user:
      # Display in HTML
      MAIN_PAGE_HTML = createMainPageHTML(game)
      self.response.write(MAIN_PAGE_HTML)
    else:
      self.redirect(users.create_login_url(self.request.uri))

  def post(self):
    playerInput = cgi.escape(self.request.get('action'))

    if playerInput == "new_game":
      self.resetGame()
      MAIN_PAGE_HTML = createMainPageHTML(game)
      self.response.write(MAIN_PAGE_HTML)
    else:
      for action in game.currentScene.actions:
        if action.name == playerInput:
          action.perform()
          MAIN_PAGE_HTML = createMainPageHTML(game)
          self.response.write(MAIN_PAGE_HTML)
          break

app = webapp2.WSGIApplication([('/', GamePage)], debug=True)