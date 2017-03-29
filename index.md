### Overview

A simple program for Windows that shows you a list of the current open windows on your desktop, and allows you to move them to a specific position on your screen without you having to interact with window itself.  I wrote this program because I have multiple monitors connected to my PC that are not always turned on, and sometimes when I open up a program it will open on one of the monitors that's turned off.  Instead of turning the monitor on, I wanted a way to retrieve the window without having to go find it myself.  So I wrote this program to do it for me!

### Features

The listbox shows you a list of the current open and visible windows on your desktop.  To retrieve a window, simply select it in the listbox, and click the "Retrieve" button.

By default, pressing the "Retrieve" button will move a window to the screen coordinates [10,10].  In most Windows environments, this will be the top-left corner of your primary monitor.  If you want the windows to move to different coordinates, click the "Settings" button and enter the coordinates manually, and click the "Save" button.  If you don't know the coordinates of your desired position, simply move your mouse cursor to that position and press the "C" key on your keyboard (just make sure the "Settings" window has focus first).  This will update the "Current Position" label with the coordinates of wherever your mouse is at that instant.

### Notes

If you're trying to retrieve and window and the window does not move or you receive an error message, it could be for the following reasons:

+ **The window longer exists** - try hitting the "refresh" button
+ **The window has a higher user priority setting** - try running WindowRetriever as administrator
+ **The window could be invisible** - certain processes, like "Program Manager" show up in the list even though it has no visibile windows associated with it. I am unsure why this occurs