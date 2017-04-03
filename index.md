### Overview

A simple program for Windows that allows you to retrieve any open windows on your desktop without having to manually interact with them.  Great if you're on a desktop with multiple monitors that may not always be turned on.  Instead of having to turn the monitor on and find the desired window, this app will retrieve it and move it to any location you want.

<div style="text-align:center; padding: 20px">
<img src="http://imgur.com/gysWvz7.jpg" alt="pic1">
</div>

### Download

[Download WindowRetriever for Windows 7/8/10](https://github.com/zgod37/window-retriever/releases)

### Features

The listbox shows you a list of the current open and visible windows on your desktop, the **Refresh** button will update this list.  To retrieve a window, simply select it in the listbox, and click the **Retrieve Window** button.  

**New in v1.1** - You can also have the window be moved to where your cursor is currently located by pressing the "C" key.

#### Settings

By default, the **Retrieve Window** button will move a window to the screen coordinates [10,10].  In most Windows environments, this will be the top-left corner of your primary monitor.  If you want the windows to be moved to different coordinates, click the **Settings** button and enter the X and Y values manually, and click the **Save** button.  

If you don't know the coordinates of your desired position, simply move your mouse cursor to that position and press the "C" key on your keyboard (just make sure the "Settings" window has focus first).  This will update the "Current Position" label with the screen coordinates of wherever your cursor is at that instant.

<div style="text-align:center; padding: 20px">
<img src="http://imgur.com/6ItFRPr.jpg" style="margin-right:20px" alt="pic2">
<img src="http://imgur.com/vRShONj.jpg" style="margin-left:20px" alt="pic3">
</div>

#### Other Notes

If you're trying to retrieve and window and the window does not move or you receive an error message, it could be for the following reasons:

+ **The window longer exists** - try hitting the "refresh" button
+ **The window has a higher user priority setting** - try running WindowRetriever as administrator
+ **The window could be invisible** - certain processes, like "Program Manager" show up in the list even though it has no visibile windows associated with it