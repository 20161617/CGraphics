using UnityEngine;
using System.Collections;
using Photon.Pun;
using UnityStandardAssets.Utility;

namespace Com.MyCompany.MyGame
{
    /// <summary>
    /// Camera work. Follow a target
    /// </summary>
    public class CameraWork : MonoBehaviourPun
    {
        #region Private Fields

        [SerializeField]
        private Transform characterBody;
        [SerializeField]
        private Transform cameraArm;
        [SerializeField]
        private Transform viewObj;
        //public Camera mainCamera;
        public GameObject testCamera;

        Animator animator;


        [Tooltip("The distance in the local x-z plane to the target")]
        [SerializeField]
        private float distance = 7.0f;


        [Tooltip("The height we want the camera to be above the target")]
        [SerializeField]
        private float height = 3.0f;


        [Tooltip("The Smooth time lag for the height of the camera.")]
        [SerializeField]
        private float heightSmoothLag = 0.3f;


        [Tooltip("Allow the camera to be offseted vertically from the target, for example giving more view of the sceneray and less ground.")]
        [SerializeField]
        private Vector3 centerOffset = Vector3.zero;


        [Tooltip("Set this as false if a component of a prefab being instanciated by Photon Network, and manually call OnStartFollowing() when and if needed.")]
        [SerializeField]
        private bool followOnStart = false;


        // cached transform of the target
        Transform cameraTransform;

        private PhotonView pv = null;
        public Transform camPivot;

        // maintain a flag internally to reconnect if target is lost or camera is switched
        private bool isFollowing;


        // Represents the current velocity, this value is modified by SmoothDamp() every time you call it.
        private float heightVelocity;


        // Represents the position we are trying to reach using SmoothDamp()
        private float targetHeight = 100000.0f;


        #endregion


        #region MonoBehaviour CallBacks


        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during initialization phase
        /// </summary>
        void Start()
        {
            pv = GetComponent<PhotonView>();
            if(pv.IsMine)
            {
                cameraTransform = Camera.main.transform;
                isFollowing = true;
            }
            // Start following the target if wanted.
            //if (photonView.IsMine)
            //{           
            //    OnStartFollowing(); 
            //}

        }


        /// <summary>
        /// MonoBehaviour method called after all Update functions have been called. This is useful to order script execution. For example a follow camera should always be implemented in LateUpdate because it tracks objects that might have moved inside Update.
        /// </summary>
        void LateUpdate()
        {
            // The transform target may not destroy on level load,
            // so we need to cover corner cases where the Main Camera is different everytime we load a new scene, and reconnect when that happens
            //if (cameraTransform == null && isFollowing)
            //{
            //    OnStartFollowing();
            //}
            //// only follow is explicitly declared
  
            if (photonView.IsMine)
            {
                
                  
                LookAround();
                Move();
                ViewApplyTest(); //임시 

            }
           
        }


        #endregion


        #region Public Methods


        /// <summary>
        /// Raises the start following event.
        /// Use this when you don't know at the time of editing what to follow, typically instances managed by the photon network.
        /// </summary>
        /// 

        private void LookAround()
        {
            Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            Vector3 camAngle = cameraArm.rotation.eulerAngles;

            float x = camAngle.x - mouseDelta.y;
            float y = camAngle.y + mouseDelta.y;

            if (x < 180f)
            {
                x = Mathf.Clamp(x, -1f, 70f); //x값 -1~ 70도 제한 
            }
            else
            {
                x = Mathf.Clamp(x, 335f, 361f);//335~361제한 
            }
            if (y < 180f)
            {
                y = Mathf.Clamp(y, -1f, 70f); //x값 -1~ 70도 제한 
            }
            else
            {
                y = Mathf.Clamp(y, 335f, 361f);//335~361제한 
            }

            Vector3 prePosition = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));


            Quaternion toRotation = viewObj.rotation * Quaternion.LookRotation(prePosition);

            viewObj.rotation = Quaternion.Slerp(viewObj.rotation,

            toRotation, Time.fixedDeltaTime);
        }

        private void Move()
        {
            Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            bool isMove = moveInput.magnitude != 0;

            if (isMove)
            {
                Vector3 lookForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
                Vector3 lookright = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;
                Vector3 moveDir = lookForward * moveInput.y + lookright * moveInput.x;

                characterBody.forward = lookForward;


                transform.position += moveDir * Time.deltaTime * 5f;

            }

        }

        public void followCamera()
        {
             //Vector3 addCamera = new Vector3(transform.position.x, transform.position.y+1.5f, transform.position.z-2.3f);
            //mainCamera.transform.position = addCamera;
        }

        public void OnStartFollowing()
        {
            cameraTransform = Camera.main.transform;
            isFollowing = true;
            Debug.Log(cameraTransform);
            // we don't smooth anything, we go straight to the right camera shot
            Cut();
        }


        #endregion


        #region Private Methods


        /// <summary>
        /// Follow the target smoothly
        /// </summary>
        void ViewApplyTest()
        {
            Vector3 target = transform.position;
            target.z -= 10;
            target.y += 3;
            cameraTransform.position = new Vector3(target.x, target.y, target.z);
         
        }
        void Apply()
        {
            Vector3 targetCenter = transform.position + centerOffset;
         
            // Calculate the current & target rotation angles
            float originalTargetAngle = transform.eulerAngles.y;
            float currentAngle = cameraTransform.eulerAngles.y;
            // Adjust real target angle when camera is locked
            float targetAngle = originalTargetAngle;
            currentAngle = targetAngle;
            targetHeight = targetCenter.y + height;


            // Damp the height
            float currentHeight = cameraTransform.position.y;
            currentHeight = Mathf.SmoothDamp(currentHeight, targetHeight, ref heightVelocity, heightSmoothLag);
            // Convert the angle into a rotation, by which we then reposition the camera
            Quaternion currentRotation = Quaternion.Euler(0, currentAngle, 0);
            // Set the position of the camera on the x-z plane to:
            // distance meters behind the target
            cameraTransform.position = targetCenter;
            cameraTransform.position += currentRotation * Vector3.back * distance;
            // Set the height of the camera
            cameraTransform.position = new Vector3(cameraTransform.position.x, currentHeight, cameraTransform.position.z);
            // Always look at the target
            SetUpRotation(targetCenter);
        }


        /// <summary>
        /// Directly position the camera to a the specified Target and center.
        /// </summary>
        void Cut()
        {
            float oldHeightSmooth = heightSmoothLag;
            heightSmoothLag = 0.001f;
            Apply();
            heightSmoothLag = oldHeightSmooth;
        }


        /// <summary>
        /// Sets up the rotation of the camera to always be behind the target
        /// </summary>
        /// <param name="centerPos">Center position.</param>
        void SetUpRotation(Vector3 centerPos)
        {
            Vector3 cameraPos = cameraTransform.position;
            Vector3 offsetToCenter = centerPos - cameraPos;
            // Generate base rotation only around y-axis
            Quaternion yRotation = Quaternion.LookRotation(new Vector3(offsetToCenter.x, 0, offsetToCenter.z));
            Vector3 relativeOffset = Vector3.forward * distance + Vector3.down * height;
            cameraTransform.rotation = yRotation * Quaternion.LookRotation(relativeOffset);
        }


        #endregion
    }
}