/*******************************************************
 * 													   *
 * Asset:		 Touch Controls Kit         		   *
 * Script:		 Enums.cs                              *
 * 													   *
 * Copyright(c): Victor Klepikov					   *
 * Support: 	 http://bit.ly/vk-Support			   *
 * 													   *
 * mySite:       http://vkdemos.ucoz.org			   *
 * myAssets:     http://u3d.as/5Fb                     *
 * myTwitter:	 http://twitter.com/VictorKlepikov	   *
 * 													   *
 *******************************************************/


namespace TouchControlsKit
{
    // Used for identification axis by type X or Y.
    public enum AxisType
    {
        X = 0,
        Y = 1
    }

    // Used for select update method of delegates and events invoking.
    public enum UpdateType
    {
        Update = 0,
        LateUpdate = 2,
        FixedUpdate = 3,
        OFF = 4
    }

    // Describes phase of a finger touch.
    public enum TCKTouchPhase
    {
        Began = 0,
        Moved = 1,
        Stationary = 2,
        Ended = 3,
        NoTouch = 4
    }
}
