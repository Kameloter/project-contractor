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
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.EventSystems;
using TouchControlsKit.Utils;

namespace TouchControlsKit.Inspector
{
    public sealed class PrefabCreator : Editor
    {
        // 
        private const string mainGOName = "_TCKCanvas";
        private const string menuAbbrev = "GameObject/UI/Touch Controls Kit/";
        private const string nameAbbrev = "TouchControlsKit";

        //
        private static GameObject tckGUIobj;
        private static GameObject button, touchpad;
        private static GameObject joystickMain, joystickBackgr, joystickImage;

        private static Color32 defaultColor = new Color32( 255, 255, 255, 150 );


        // CreateTCKInput [MenuItem]
        [MenuItem( menuAbbrev + "TCK Canvas" )]
        private static void CreateTouchManager()
        {
            if( FindObjectOfType<TCKInput>() && !tckGUIobj )
                tckGUIobj = FindObjectOfType<TCKInput>().gameObject;

            CreateEventSystem();

            if( tckGUIobj ) 
                return;

            tckGUIobj = new GameObject( mainGOName, typeof( Canvas ), typeof( GraphicRaycaster ), typeof( CanvasScaler ), typeof( TCKInput ) );
            tckGUIobj.layer = LayerMask.NameToLayer( "UI" );

            GuiCamera.CreateCamera( tckGUIobj.transform, 32, 100f );
            
            tckGUIobj.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;            
            tckGUIobj.GetComponent<Canvas>().worldCamera = GuiCamera.m_Camera;
            tckGUIobj.GetComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        }

        [MenuItem( menuAbbrev + "TCK Canvas", true )]
        private static bool ValidateCreateTouchManager()
        {
            return !FindObjectOfType<TCKInput>();
        }


        // CreateEventSystem
        private static void CreateEventSystem()
        {
            if( !FindObjectOfType<EventSystem>() )
            {
              GameObject esGo = new GameObject( "EventSystem", typeof( EventSystem ), typeof( TouchInputModule ) );
              esGo.GetComponent<TouchInputModule>().forceModuleActive = true;
            }
        }


        // CreateButton [MenuItem]
        [MenuItem( menuAbbrev + "Button" )]
        private static void CreateButton()
        {
            if( !tckGUIobj ) 
                CreateTouchManager();

            SetupController<TCKButton>( ref button, tckGUIobj.transform, "Button" + FindObjectsOfType<TCKButton>().Length.ToString(), true );

            TCKButton btnTemp = button.GetComponent<TCKButton>();
            btnTemp.baseImage = button.GetComponent<Image>();
            btnTemp.baseRect = button.GetComponent<RectTransform>();
            btnTemp.normalSprite = AssetDatabase.LoadAssetAtPath<Sprite>( "Assets/" + nameAbbrev + "/Sprites/ButtonNormal.png" );
            btnTemp.pressedSprite = AssetDatabase.LoadAssetAtPath<Sprite>( "Assets/" + nameAbbrev + "/Sprites/ButtonPressed.png" );            
            btnTemp.normalColor = btnTemp.pressedColor = defaultColor;
            btnTemp.MyName = button.name;
            btnTemp.baseRect.sizeDelta = Vector2.one * 45f;            
            button.transform.localScale = Vector3.one;
            btnTemp.baseRect.anchoredPosition = RandomPos;
        }

        // CreateJoystick [MenuItem]
        [MenuItem( menuAbbrev + "Joystick" )]
        private static void CreateJoystick()
        {
            if( !tckGUIobj ) 
                CreateTouchManager();

            SetupController<TCKJoystick>( ref joystickMain, tckGUIobj.transform, "Joystick" + FindObjectsOfType<TCKJoystick>().Length.ToString(), true );

            TCKJoystick joyTemp = joystickMain.GetComponent<TCKJoystick>();            
            joyTemp.baseImage = joystickMain.GetComponent<Image>();
            joyTemp.baseRect = joystickMain.GetComponent<RectTransform>();
            joyTemp.baseImage.sprite = AssetDatabase.LoadAssetAtPath<Sprite>( "Assets/" + nameAbbrev + "/Sprites/Touchzone.png" );

            SetupController<TCKJoystick>( ref joystickBackgr, joystickMain.transform, "JoystickBack", false );
            SetupController<TCKJoystick>( ref joystickImage, joystickBackgr.transform, "Joystick", false );

            joyTemp.joystickBackgroundImage = joystickBackgr.GetComponent<Image>();
            joyTemp.joystickBackgroundRT = joystickBackgr.GetComponent<RectTransform>();
            joyTemp.joystickBackgroundRT.sizeDelta = Vector2.one * 75f; 

            joyTemp.joystickImage = joystickImage.GetComponent<Image>();            
            joyTemp.joystickRT = joystickImage.GetComponent<RectTransform>();
            joyTemp.joystickRT.anchorMin = Vector2.zero;
            joyTemp.joystickRT.anchorMax = Vector2.one;
            joyTemp.joystickRT.sizeDelta = Vector2.zero;

            joyTemp.joystickBackgroundImage.sprite = AssetDatabase.LoadAssetAtPath<Sprite>( "Assets/" + nameAbbrev + "/Sprites/JoystickBack.png" );
            joyTemp.joystickImage.sprite = AssetDatabase.LoadAssetAtPath<Sprite>( "Assets/" + nameAbbrev + "/Sprites/Joystick.png" );

            joyTemp.baseRect.sizeDelta = new Vector2( 180f, 160f );
            
            joyTemp.MyName = joystickMain.name;

            joystickMain.transform.localScale = Vector3.one;
            joyTemp.baseRect.anchoredPosition = RandomPos;
        }

        // CreateTouchpad [MenuItem]
        [MenuItem( menuAbbrev + "Touchpad" )]
        private static void CreateTouchpad()
        {
            if( !tckGUIobj ) 
                CreateTouchManager();

            SetupController<TCKTouchpad>( ref touchpad, tckGUIobj.transform, "Touchpad" + FindObjectsOfType<TCKTouchpad>().Length.ToString(), true );

            TCKTouchpad tpTemp = touchpad.GetComponent<TCKTouchpad>();
            tpTemp.baseImage = touchpad.GetComponent<Image>();
            tpTemp.baseRect = touchpad.GetComponent<RectTransform>();
            tpTemp.baseImage.sprite = AssetDatabase.LoadAssetAtPath<Sprite>( "Assets/" + nameAbbrev + "/Sprites/Touchzone.png" );
            tpTemp.MyName = touchpad.name;
            tpTemp.baseRect.sizeDelta = new Vector2( 270f, 170f );

            touchpad.transform.localScale = Vector3.one;
            tpTemp.baseRect.anchoredPosition = RandomPos;
        }


        // SetupController<Generic>
        private static void SetupController<TComp>(
            ref GameObject myGO, Transform myParent, string myName, bool needMyComponent ) where TComp : MonoBehaviour
        {
            myGO = new GameObject( myName, typeof( Image ) );
            myGO.GetComponent<Image>().color = defaultColor;
            myGO.layer = LayerMask.NameToLayer( "UI" );
            myGO.transform.localScale = Vector3.one;
            myGO.transform.SetParent( myParent );

            if( needMyComponent )
                myGO.AddComponent<TComp>();
        }
        

        // RandomPos
        private static Vector2 RandomPos { get { return new Vector2( Random.Range( -200f, 200f ), Random.Range( -120f, 120f ) ); } }
    }
}