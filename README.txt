Dungeon map v1.2
-----------------------------------------
A Unity solution for setting up pen&paper roleplay dungeons
-----------------------------------------
by JungleJ0hn
-----------------------------------------

Description:

Solution for helping game masters set up and manage pen & paper roleplaying sessions digitally, providing the option to save sessions and to hiding information from players via a fog-of-war layer.

-----------------------------------------

Functionality:

Project keeps track of multiple individual pen&paper sessions at a time. Information on individual Sessions are stored in session objects. 

Edit or play each session separately.

Sessions are displayed using multiple images with a relative position, rotation & size. That information is stored in a tileObject.

To instantiate a tile an image must be imported, and named.  Drag & drop import of images and save them as tileCreatorTiles from the import menu.

After being imported the information concerning the image and its name is stored in a 'tileCreatorTile' object. Imported tileCreatorTiles show up in the sidebar-tilemenu.

tiles Can be instantiated by clicking the image of their tileCreatorTile in the sidebar-tilemenu.

Drag, scale and rotate tiles in order to place all characters and environment by clicking the icons that show up On-Hover.

-----------------------------------------------

Additional functionality:

Includes functionality to save sessions.

Includes fogOfWar in order to hide information from players.

Includes functionality to enable an extra connected screen as an extension for when playing. FogOfWar will show up semitransparent on one screen, intended for the game master and fully opaque on the extended screen intended to be shown to the players playing the session.

--------------------------------------------------

Credit:

Unit drag and drop support:
UnityWindowsFileDrag-Drop-master, by 2018 Markus Göbel (Bunny83)