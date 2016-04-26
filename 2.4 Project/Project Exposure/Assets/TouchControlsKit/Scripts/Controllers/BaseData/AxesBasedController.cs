/*******************************************************
 * 													   *
 * Asset:		 Touch Controls Kit         		   *
 * Script:		 AxesBasedController.cs                *
 * 													   *
 * Copyright(c): Victor Klepikov					   *
 * Support: 	 http://bit.ly/vk-Support			   *
 * 													   *
 * mySite:       http://vkdemos.ucoz.org			   *
 * myAssets:     http://u3d.as/5Fb                     *
 * myTwitter:	 http://twitter.com/VictorKlepikov	   *
 * 													   *
 *******************************************************/


using UnityEngine;

namespace TouchControlsKit
{
    public abstract class AxesBasedController : ControllerBase
    {
        public float sensitivity = 1f;        

        public Axis axisX = new Axis( "Horizontal" );
        public Axis axisY = new Axis( "Vertical" );

        [SerializeField]
        private bool showBaseImage = true;

        protected Vector2 defaultPosition, currentPosition, currentDirection;
        
        // Show TouchZone
        public bool ShowTouchZone
        {
            get { return showBaseImage; }
            set
            {
                if( showBaseImage == value )
                    return;

                showBaseImage = value;
                ShowHideTouchZone();
            }
        }

        // ShowHide TouchZone
        private void ShowHideTouchZone()
        {
            if( showBaseImage )
            {
                baseImage.color = baseImageNativeColor;
            }
            else
            {
                baseImageNativeColor = baseImage.color;
                baseImage.color = ( Color32 )Color.clear;
            }
        }
        
        
        // Set Axis
        protected void SetAxis( float x, float y )
        {
            axisX.SetValue( x );
            axisY.SetValue( y );
        }


        // Control Reset
        protected override void ControlReset()
        {
            base.ControlReset();
            SetAxis( 0f, 0f );
        }
    }
}