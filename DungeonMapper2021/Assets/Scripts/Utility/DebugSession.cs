using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DebugSession {

    static int requestIndex = 0;
    public static void DebugSessionInfo(Session currentSession, string customString)
    {
        string newLine = System.Environment.NewLine;
        string debugString = "Debug request index " + requestIndex +"." +  newLine; 
        if (currentSession != null)
        {
            
            debugString += "Session Name: " + currentSession.Name + "." ;

            if (!string.IsNullOrEmpty(customString))
            {
                debugString += newLine + "customString";
            }
            
            debugString += newLine + "Loadsprites: " + currentSession.LoadSpriteList.Count + newLine;

            foreach (LoadSprite sprite in currentSession.LoadSpriteList)
            {
                debugString += newLine + "Foreground Loadsprite: " + sprite.Name + " Contains: " + sprite.LoadTileList.Count + " tiles.";

                
                foreach (LoadTile tile in sprite.LoadTileList)
                {
                    debugString += newLine + "tile pos: " + tile.pos + ", tile Size: " + tile.size + ".";
                }
                
            }

            debugString += newLine + "Background Loadsprites: " + currentSession.BackgroundLoadSpriteList.Count;

            foreach (LoadSprite sprite in currentSession.BackgroundLoadSpriteList)
            {
                debugString += newLine + "Background Loadsprite: " + sprite.Name + " Contains: " + sprite.LoadTileList.Count + " tiles.";

                foreach (LoadTile tile in sprite.LoadTileList)
                {
                    debugString += newLine + "tile pos: " + tile.pos + ", tile Size: " + tile.size + ".";
                }
            }

            
        } else
        {

            debugString += "currentSession is null. ";
            if (!string.IsNullOrEmpty(customString))
            {
                debugString += "Message: " + customString;
            } else
            {
                debugString += "No Message attached";
            }
        }


            Debug.LogError(debugString);

        requestIndex += 1;
        }

    }
	
