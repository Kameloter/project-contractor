using UnityEngine;
using TouchControlsKit;

namespace Examples
{
    public class FirstPersonExample : MonoBehaviour
    {
        public string fireBtn, jumpBtn, moveJoystick, lookTouchpad;
        
        //
        public enum GetAxesMethod
        {
            GetByName = 0,
            GetByType = 1
        }
        public GetAxesMethod axesGetType = GetAxesMethod.GetByName;

        //
        private Transform myTransform, cameraTransform;
        private CharacterController controller = null;
        private float rotation = 0f;
        Vector3 moveDirection = Vector3.zero;
        private bool jump, grounded, prevGrounded;
        private float weapReadyTime = 0f;
        private bool weapReady = true;


        // Awake
        void Awake()
        {
            myTransform = transform;
            cameraTransform = Camera.main.transform;
            controller = this.GetComponent<CharacterController>();
        }
        
        // Update
        void Update()
        {
            if( !weapReady )
            {
                weapReadyTime += Time.deltaTime;
                if( weapReadyTime > .15f )
                {
                    weapReady = true;
                    weapReadyTime = 0f;
                }
            }

            if( TCKInput.GetButton( fireBtn ) )
                PlayerFiring();

            if( TCKInput.GetButtonDown( jumpBtn ) )
                Jumping();
        }

        // FixedUpdate
        void FixedUpdate()
        {
            if( axesGetType == GetAxesMethod.GetByName )
            {
                float AxisX = TCKInput.GetAxis( moveJoystick, "Horizontal" );
                float AxisY = TCKInput.GetAxis( moveJoystick, "Vertical" );
                PlayerMovement( AxisX, AxisY );

                AxisX = TCKInput.GetAxis( lookTouchpad, "Horizontal" );
                AxisY = TCKInput.GetAxis( lookTouchpad, "Vertical" );
                PlayerRotation( AxisX, AxisY );
            }
            else
            {
                float AxisX = TCKInput.GetAxis( moveJoystick, AxisType.X );
                float AxisY = TCKInput.GetAxis( moveJoystick, AxisType.Y );
                PlayerMovement( AxisX, AxisY );

                AxisX = TCKInput.GetAxis( lookTouchpad, AxisType.X );
                AxisY = TCKInput.GetAxis( lookTouchpad, AxisType.Y );
                PlayerRotation( AxisX, AxisY );
            }
        }


        // Jumping
        private void Jumping()
        {
            if( grounded )
                jump = true;
        }

        
        // PlayerMovement
        private void PlayerMovement( float horizontal, float vertical )
        {
            grounded = controller.isGrounded;
            
            moveDirection = myTransform.forward * vertical;
            moveDirection += myTransform.right * horizontal;            

            if( grounded )
            {
                moveDirection *= 7f;
                moveDirection.y = -10f;

                if( jump )
                {
                    jump = false;
                    moveDirection.y = 5f;
                }
            }
            else
            {
                moveDirection += Physics.gravity * 2f * Time.fixedDeltaTime;
            }

            moveDirection.y *= 20f;
            controller.Move( moveDirection * Time.fixedDeltaTime );


            if( !prevGrounded && grounded )
                moveDirection.y = 0f;

            prevGrounded = grounded;
        }

        // PlayerRotation
        private void PlayerRotation( float horizontal, float vertical )
        {
            myTransform.Rotate( 0f, horizontal * 12f, 0f );
            rotation += vertical * 12f;
            rotation = Mathf.Clamp( rotation, -60f, 60f );
            cameraTransform.localEulerAngles = new Vector3( -rotation, cameraTransform.localEulerAngles.y, 0f );
        }

        // PlayerFiring
        private void PlayerFiring()
        {
            if( !weapReady )
                return;

            weapReady = false;

            GameObject sphere = GameObject.CreatePrimitive( PrimitiveType.Sphere );
            sphere.transform.position = ( myTransform.position + myTransform.right );
            sphere.transform.localScale = Vector3.one * .15f;
            Rigidbody rBody = sphere.AddComponent<Rigidbody>();
            Transform camTransform = Camera.main.transform;
            rBody.AddForce( camTransform.forward * Random.Range( 25f, 35f ) + camTransform.right * Random.Range( -2f, 2f ) + camTransform.up * Random.Range( -2f, 2f ), ForceMode.Impulse );
            GameObject.Destroy( sphere, 3.5f );
        }
    }
}