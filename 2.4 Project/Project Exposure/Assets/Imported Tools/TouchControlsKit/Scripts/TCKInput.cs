/*******************************************************
 * 													   *
 * Asset:		 Touch Controls Kit         		   *
 * Script:		 TCKInput.cs                           *
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
    public sealed class TCKInput : MonoBehaviour
    {
        private static ControllerBase[] controllers = null;
        private static AxesBasedController[] abControllers = null;
        private static TCKButton[] buttons = null;

        private static TCKInput instance = null;

        private static bool active = true;

        /// <summary>
        /// The local active state of all controllers. (Read Only)
        /// </summary>
        public static bool isActive
        {
            get { return active; }
        }


        // Awake
        void Awake()
        {
            instance = this;
            SetActive( true );
        }


        /// <summary>
        /// Activates/Deactivates all controllers in scene.
        /// </summary>
        /// <param name="value"></param>
        public static void SetActive( bool value )
        {
            active = value;
            instance.enabled = value;
            instance.gameObject.SetActive( value );

            if( value )
            {
                controllers = instance.gameObject.GetComponentsInChildren<ControllerBase>();
                abControllers = instance.gameObject.GetComponentsInChildren<AxesBasedController>();
                buttons = instance.gameObject.GetComponentsInChildren<TCKButton>();

                foreach( ControllerBase controller in controllers )
                    controller.ControlAwake();
            }
            else
            {
                controllers = null;
                abControllers = null;
                buttons = null;
            }
        }        


        /// <summary>
        /// Returns the controller Enable value identified by controllerName.
        /// </summary>
        /// <param name="controllerName"></param>
        /// <returns></returns>
        public static bool GetControllerEnable( string controllerName )
        {
            if( !active )
                return false;

            foreach( ControllerBase controller in controllers )            
                if( controller.MyName == controllerName )                
                    return controller.isEnable;

            Debug.LogError( "Controller: " + controllerName + " not found!" );
            return false;
        }

        /// <summary>
        /// Sets the controller Enable value identified by controllerName.
        /// </summary>
        /// <param name="controllerName"></param>
        /// <param name="value"></param>
        public static void SetControllerEnable( string controllerName, bool value )
        {
            if( !active )
                return;

            foreach( ControllerBase controller in controllers )
            {
                if( controller.MyName == controllerName )
                {
                    controller.isEnable = value;
                    return;
                }
            }
            Debug.LogError( "Controller: " + controllerName + " not found!" );
        }


        /// <summary>
        /// Returns the controller Active state value identified by controllerName.
        /// </summary>
        /// <param name="controllerName"></param>
        /// <returns></returns>
        public static bool GetControllerActive( string controllerName )
        {
            if( !active )
                return false;

            foreach( ControllerBase controller in controllers )
                if( controller.MyName == controllerName )
                    return controller.isActive;

            Debug.LogError( "Controller: " + controllerName + " not found!" );
            return false;
        }

        /// <summary>
        /// Sets the controller Active state value identified by controllerName.
        /// </summary>
        /// <param name="controllerName"></param>
        /// <param name="value"></param>
        public static void SetControllerActive( string controllerName, bool value )
        {
            if( !active )
                return;

            foreach( ControllerBase controller in controllers )
            {
                if( controller.MyName == controllerName )
                {
                    controller.isActive = value;
                    return;
                }
            }
            Debug.LogError( "Controller: " + controllerName + " not found!" );
        }


        /// <summary>
        /// Returns the controller Visibility value identified by controllerName.
        /// </summary>
        /// <param name="controllerName"></param>
        /// <returns></returns>
        public static bool GetControllerVisible( string controllerName )
        {
            if( !active )
                return false;

            foreach( ControllerBase controller in controllers )
                if( controller.MyName == controllerName )
                    return controller.isVisible;

            Debug.LogError( "Controller: " + controllerName + " not found!" );
            return false;
        }

        /// <summary>
        /// Sets the controller Visibility value identified by controllerName.
        /// </summary>
        /// <param name="controllerName"></param>
        /// <param name="value"></param>
        public static void SetControllerVisible( string controllerName, bool value )
        {
            if( !active )
                return;

            foreach( ControllerBase controller in controllers )
            {
                if( controller.MyName == controllerName )
                {
                    controller.isVisible = value;
                    return;
                }
            }
            Debug.LogError( "Controller: " + controllerName + " not found!" );
        }


        /// <summary>
        /// Returns the Axis value identified by controllerName & axisName.
        /// </summary>
        /// <param name="controllerName"></param>
        /// <param name="axisName"></param>
        /// <returns></returns>
        public static float GetAxis( string controllerName, string axisName )
        {
            if( !active )
                return 0f;

            foreach( AxesBasedController controller in abControllers )
            {
                if( controller.MyName == controllerName )
                {
                    if( axisName == controller.axisX.Name )
                        return controller.axisX.value;
                    else if( axisName == controller.axisY.Name )
                        return controller.axisY.value;

                    Debug.LogError( "Axis: " + axisName + " not found!" );
                    return 0f;
                }
            }
            Debug.LogError( "Controller: " + controllerName + " not found!" );
            return 0f;
        }

        /// <summary>
        /// Returns the Axis value  identified by controllerName & axisType.
        /// </summary>
        /// <param name="controllerName"></param>
        /// <param name="axisType"></param>
        /// <returns></returns>
        public static float GetAxis( string controllerName, AxisType axisType )
        {
            if( !active )
                return 0f;

            foreach( AxesBasedController controller in abControllers )
            {
                if( controller.MyName == controllerName )
                {
                    if( axisType == AxisType.X )
                        return controller.axisX.value;
                    else if( axisType == AxisType.Y )
                        return controller.axisY.value;

                    Debug.LogError( "Axis: " + axisType + " not found!" );
                    return 0f;
                }
            }
            Debug.LogError( "Controller: " + controllerName + " not found!" );
            return 0f;
        }


        /// <summary>
        /// Returns the axis Enable value identified by controllerName & axisName.
        /// </summary>
        /// <param name="controllerName"></param>
        /// <param name="axisName"></param>
        /// <returns></returns>
        public static bool GetAxisEnable( string controllerName, string axisName )
        {
            if( !active )
                return false;

            foreach( AxesBasedController controller in abControllers )
            {
                if( controller.MyName == controllerName )
                {
                    if( axisName == controller.axisX.Name )
                        return controller.axisX.enabled;
                    else if( axisName == controller.axisY.Name )
                        return controller.axisY.enabled;

                    Debug.LogError( "Axis: " + axisName + " not found!" );
                    return false;
                }
            }
            Debug.LogError( "Controller: " + controllerName + " not found!" );
            return false;
        }

        /// <summary>
        /// Returns the axis Enable value identified by controllerName & axisType.
        /// </summary>
        /// <param name="controllerName"></param>
        /// <param name="axisType"></param>
        /// <returns></returns>
        public static bool GetAxisEnable( string controllerName, AxisType axisType )
        {
            if( !active )
                return false;

            foreach( AxesBasedController controller in abControllers )
            {
                if( controller.MyName == controllerName )
                {
                    if( axisType == AxisType.X )
                        return controller.axisX.enabled;
                    else if( axisType == AxisType.Y )
                        return controller.axisY.enabled;

                    Debug.LogError( "Axis: " + axisType + " not found!" );
                    return false;
                }
            }
            Debug.LogError( "Controller: " + controllerName + " not found!" );
            return false;
        }

        /// <summary>
        /// Sets the axis Enable value identified by controllerName & axisName.
        /// </summary>
        /// <param name="controllerName"></param>
        /// <param name="axisName"></param>
        /// <param name="value"></param>
        public static void SetAxisEnable( string controllerName, string axisName, bool value )
        {
            if( !active )
                return;

            foreach( AxesBasedController controller in abControllers )
            {
                if( controller.MyName == controllerName )
                {
                    if( axisName == controller.axisX.Name )
                    {
                        controller.axisX.enabled = value;
                        return;
                    }
                    else if( axisName == controller.axisY.Name )
                    {
                        controller.axisY.enabled = value;
                        return;
                    }
                    Debug.LogError( "Axis: " + axisName + " not found!" );
                    return;
                }
            }
            Debug.LogError( "Controller: " + controllerName + " not found!" );
        }

        /// <summary>
        /// Sets the axis Enable value identified by controllerName & axisType.
        /// </summary>
        /// <param name="controllerName"></param>
        /// <param name="axisType"></param>
        /// <param name="value"></param>
        public static void SetAxisEnable( string controllerName, AxisType axisType, bool value )
        {
            if( !active )
                return;

            foreach( AxesBasedController controller in abControllers )
            {
                if( controller.MyName == controllerName )
                {
                    if( axisType == AxisType.X )
                    {
                        controller.axisX.enabled = value;
                        return;
                    }
                    else if( axisType == AxisType.Y )
                    {
                        controller.axisY.enabled = value;
                        return;
                    }
                    Debug.LogError( "Axis: " + axisType + " not found!" );
                    return;
                }
            }
            Debug.LogError( "Controller: " + controllerName + " not found!" );
        }


        /// <summary>
        /// Returns the axis Inverse value identified by controllerName & axisName.
        /// </summary>
        /// <param name="controllerName"></param>
        /// <param name="axisName"></param>
        /// <returns></returns>
        public static bool GetAxisInverse( string controllerName, string axisName )
        {
            if( !active )
                return false;

            foreach( AxesBasedController controller in abControllers )
            {
                if( controller.MyName == controllerName )
                {
                    if( axisName == controller.axisX.Name )
                        return controller.axisX.inverse;
                    else if( axisName == controller.axisY.Name )
                        return controller.axisY.inverse;

                    Debug.LogError( "Axis: " + axisName + " not found!" );
                    return false;
                }
            }
            Debug.LogError( "Controller: " + controllerName + " not found!" );
            return false;
        }

        /// <summary>
        /// Returns the axis Inverse value identified by controllerName & axisType.
        /// </summary>
        /// <param name="controllerName"></param>
        /// <param name="axisType"></param>
        /// <returns></returns>
        public static bool GetAxisInverse( string controllerName, AxisType axisType )
        {
            if( !active )
                return false;

            foreach( AxesBasedController controller in abControllers )
            {
                if( controller.MyName == controllerName )
                {
                    if( axisType == AxisType.X )
                        return controller.axisX.inverse;
                    else if( axisType == AxisType.Y )
                        return controller.axisY.inverse;

                    Debug.LogError( "Axis: " + axisType + " not found!" );
                    return false;
                }
            }
            Debug.LogError( "Controller: " + controllerName + " not found!" );
            return false;
        }

        /// <summary>
        /// Sets the axis Inverse value identified by controllerName & axisName.
        /// </summary>
        /// <param name="controllerName"></param>
        /// <param name="axisName"></param>
        /// <param name="value"></param>
        public static void SetAxisInverse( string controllerName, string axisName, bool value )
        {
            if( !active )
                return;

            foreach( AxesBasedController controller in abControllers )
            {
                if( controller.MyName == controllerName )
                {
                    if( axisName == controller.axisX.Name )
                    {
                        controller.axisX.inverse = value;
                        return;
                    }
                    else if( axisName == controller.axisY.Name )
                    {
                        controller.axisY.inverse = value;
                        return;
                    }
                    Debug.LogError( "Axis: " + axisName + " not found!" );
                    return;
                }
            }
            Debug.LogError( "Controller: " + controllerName + " not found!" );
        }

        /// <summary>
        /// Sets the axis Inverse value identified by controllerName & axisName.
        /// </summary>
        /// <param name="controllerName"></param>
        /// <param name="axisType"></param>
        /// <param name="value"></param>
        public static void SetAxisInverse( string controllerName, AxisType axisType, bool value )
        {
            if( !active )
                return;

            foreach( AxesBasedController controller in abControllers )
            {
                if( controller.MyName == controllerName )
                {
                    if( axisType == AxisType.X )
                    {
                        controller.axisX.inverse = value;
                        return;
                    }
                    else if( axisType == AxisType.Y )
                    {
                        controller.axisY.inverse = value;
                        return;
                    }
                    Debug.LogError( "Axis: " + axisType + " not found!" );
                    return;
                }
            }
            Debug.LogError( "Controller: " + controllerName + " not found!" );
        }


        /// <summary>
        /// Returns the value of the controller Sensitivity identified by controllerName.
        /// </summary>
        /// <param name="controllerName"></param>
        /// <returns></returns>
        public static float GetSensitivity( string controllerName )
        {
            if( !active )
                return 0f;

            foreach( AxesBasedController controller in abControllers )            
                if( controller.MyName == controllerName )                
                    return controller.sensitivity;                
            
            Debug.LogError( "Controller: " + controllerName + " not found!" );
            return 0f;
        }

        /// <summary>
        /// Sets the Sensitivity value identified by controllerName.
        /// </summary>
        /// <param name="controllerName"></param>
        /// <param name="value"></param>
        public static void SetSensitivity( string controllerName, float value )
        {
            if( !active )
                return;

            foreach( AxesBasedController controller in abControllers )
            {
                if( controller.MyName == controllerName )
                {
                    controller.sensitivity = value;
                    return;
                }
            }
            Debug.LogError( "Controller: " + controllerName + " not found!" );
        }


        /// <summary>
        /// Showing/Hiding touch zone for all controllers in scene.
        /// </summary>
        /// <param name="value"></param>
        public static void ShowingTouchZone( bool value )
        {
            if( !active )
                return;

            foreach( AxesBasedController controller in abControllers )
                controller.ShowTouchZone = value;            
        }


        /// <summary>
        /// Returns true during the frame the user pressed down the touch button identified by buttonName.
        /// </summary>
        /// <param name="buttonName"></param>
        /// <returns></returns>
        public static bool GetButtonDown( string buttonName )
        {
            if( !active )
                return false;

            foreach( TCKButton button in buttons )            
                if( button.MyName == buttonName )                
                    return button.isDOWN;                
            
            Debug.LogError( "Button: " + buttonName + " not found!" );
            return false;
        }

        /// <summary>
        /// Returns whether the given touch button is held down identified by buttonName.
        /// </summary>
        /// <param name="buttonName"></param>
        /// <returns></returns>
        public static bool GetButton( string buttonName )
        {
            if( !active )
                return false;

            foreach( TCKButton button in buttons )            
                if( button.MyName == buttonName )                
                    return button.isPRESSED;                
            
            Debug.LogError( "Button: " + buttonName + " not found!" );
            return false;
        }        

        /// <summary>
        /// Returns true during the frame the user releases the given touch button identified by buttonName.
        /// </summary>
        /// <param name="buttonName"></param>
        /// <returns></returns>
        public static bool GetButtonUp( string buttonName )
        {
            if( !active )
                return false;

            foreach( TCKButton button in buttons )            
                if( button.MyName == buttonName )                
                    return button.isUP;                
            
            Debug.LogError( "Button: " + buttonName + " not found!" );
            return false;
        }

        /// <summary>
        /// Returns true during the frame the user clicked the given touch button identified by buttonName.
        /// </summary>
        /// <param name="buttonName"></param>
        /// <returns></returns>
        public static bool GetButtonClick( string buttonName )
        {
            if( !active )
                return false;

            foreach( TCKButton button in buttons )            
                if( button.MyName == buttonName )                
                    return button.isCLICK;               
            
            Debug.LogError( "Button: " + buttonName + " not found!" );
            return false;
        }


        /// <summary>
        /// Returns the UpdateType enum value identified by controllerName.
        /// </summary>
        /// <param name="controllerName"></param>
        /// <returns></returns>
        public static UpdateType GetUpdateType( string controllerName )
        {
            if( !active )
                return UpdateType.OFF;

            foreach( ControllerBase controller in controllers )
                if( controller.MyName == controllerName )
                    return controller.updateType;

            Debug.LogError( "Controller: " + controllerName + " not found!" );
            return UpdateType.OFF;
        }

        /// <summary>
        /// Sets the UpdateType enum value identified by controllerName.
        /// </summary>
        /// <param name="controllerName"></param>
        /// <param name="updateType"></param>
        public static void SetUpdateType( string controllerName, UpdateType updateType )
        {
            if( !active )
                return;

            foreach( ControllerBase controller in controllers )
                if( controller.MyName == controllerName )
                    controller.updateType = updateType;

            Debug.LogError( "Controller: " + controllerName + " not found!" );
        }


        /// <summary>
        /// Returns describes phase of a finger touch identified by controllerName.
        /// </summary>
        /// <param name="controllerName"></param>
        /// <returns></returns>
        public static TCKTouchPhase GetTouchPhase( string controllerName )
        {
            if( !active )
                return TCKTouchPhase.Ended;

            foreach( ControllerBase controller in controllers )            
                if( controller.MyName == controllerName )                
                    return controller.touchPhase;                
            
            Debug.LogError( "Controller: " + controllerName + " not found!" );
            return TCKTouchPhase.Ended;
        }
    }
}