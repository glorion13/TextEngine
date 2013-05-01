import cgi
import webapp2
import urllib

from google.appengine.api import users
from google.appengine.ext import db

import core
import dataParser


def createActionsHTML(game_id, game):
  actionsHTML = ""
  for action in game.currentScene.actions:
    actionsHTML += """<form action="/game/"""+game_id+"""" method="post"><div><input name="action" type="submit" value=\""""+action.name+"""\"></div></form>"""
  for action in game.globalActions:
    actionsHTML += """<form action="/game/"""+game_id+"""" method="post"><div><input name="action" type="submit" value=\""""+action.name+"""\"></div></form>"""
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

def createMainPageHTML(game_id, game):
  narrativeHTML = createNarrativeHTML(game)
  actionsHTML = createActionsHTML(game_id, game)
  return createCombinedHTML(narrativeHTML, actionsHTML)

class Action(db.Model):
  command = db.StringProperty()

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
    user = users.get_current_user()
    if user:
      global game
      game_id = game_id + ".xml"
      game = parser.loadXMLGameData(game_id)
      game.effectDict['Go to scene'](game, game.startingScene)
      MAIN_PAGE_HTML = createMainPageHTML(game_id, game)
      self.response.write(MAIN_PAGE_HTML)
    else:
      self.redirect(users.create_login_url(self.request.uri))
  def post(self, game_id):
    user = users.get_current_user()
    if user:
      playerInput = cgi.escape(self.request.get('action'))
      if playerInput == "Start again":
        self.resetGame(game_id)
        MAIN_PAGE_HTML = createMainPageHTML(game_id, game)
        self.response.write(MAIN_PAGE_HTML)
      else:
        for action in game.currentScene.actions:
          if action.name == playerInput:
            act = Action()
            act.command = action.name
            act.put()
            action.perform()
            MAIN_PAGE_HTML = createMainPageHTML(game_id, game)
            self.response.write(MAIN_PAGE_HTML)
            break
    else:
      self.redirect(users.create_login_url(self.request.uri))

def main():
  app = webapp2.WSGIApplication([('/', MainPage), ('/game/([^/]+)', GamePage), ('/logout', LogoutPage)], debug=True)
  return app

app = main()