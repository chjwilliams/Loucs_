using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChrsUtils
{
    namespace ChrsEventSystem
    { 
        namespace GameEvents
        {
            /*--------------------------------------------------------------------------------------*/
			/*																						*/
			/*	GameEvent: Abstract class for Game Events for the GameEventsManager				    */
			/*																						*/
			/*--------------------------------------------------------------------------------------*/
            public abstract class GameEvent 
            {
                public delegate void Handler(GameEvent e);      //  Delegate for GameEvents
            }
        }
    }
}