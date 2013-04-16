from Tkinter import *

class App:
    def __init__(self, master):

        frame = Frame(master)
        frame.pack()

        self.button = Button(frame, text="Exit", command=frame.quit)
        self.button.pack(side=LEFT)

        self.hi = Button(frame, text="Speak", command=self.say_hi)
        self.hi.pack(side=LEFT)

    def say_hi(self):
        print "Hello, World!"

root = Tk()
root.tk.eval('package require dde; dde servername Tkinter')
app = App(root)
root.mainloop()