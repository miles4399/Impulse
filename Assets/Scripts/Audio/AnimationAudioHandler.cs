using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationAudioHandler : MonoBehaviour
{
    [Header("Physics")]
    [SerializeField, Range(0.01f, 100.00f)] private float RaycastLength = 10.00f;
    [SerializeField] private LayerMask RaycastLayer = 0;

    [Header("Footsteps")] //I could copy this chunk of footstep stuff to make another animation tag type in addition to footsteps
    [SerializeField, FMODUnity.EventRef] private string FootstepEventPath;
    [SerializeField] private string FootstepParamPath;
    //[SerializeField] private Transform objectTransform;
    private FMOD.Studio.EventInstance FootstepEvent;

    [Header("Jump")] 
    [SerializeField, FMODUnity.EventRef] private string JumpEventPath;
    private FMOD.Studio.EventInstance JumpEvent;

    [Header("Land")] 
    [SerializeField, FMODUnity.EventRef] private string LandEventPath;
    private FMOD.Studio.EventInstance LandEvent;

    private Rigidbody objectRigidbody;

    protected void Awake()
    {
        objectRigidbody = GetComponentInParent<Rigidbody>();
        FootstepEvent = FMODUnity.RuntimeManager.CreateInstance(FootstepEventPath);
        JumpEvent = FMODUnity.RuntimeManager.CreateInstance(JumpEventPath);
        LandEvent = FMODUnity.RuntimeManager.CreateInstance(LandEventPath);  
    }

    public void OnFootstep(AnimationEvent evt)
    {
        //RaycastHit hitInfo;
        //Vector3 direction = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - RaycastLength, gameObject.transform.position.z);
        //Physics.Raycast(gameObject.transform.position, direction, out hitInfo, RaycastLength, RaycastLayer);

        //Task 1: Create or identify a layer for surfaces
        //Task 2: Define tags, physics materials, or equivilent for different surfaces
        //Task 3: Use a switch statement below to set FMOD surface parameter accordingly
        //Reference: Metal = Value 0, Concrete = Value 1, Wall = Value = 2

        //FootstepEvent.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(playerObject, objectRigibody));
        if (evt.animatorClipInfo.weight > 0.5f)
        {
          FMODUnity.RuntimeManager.AttachInstanceToGameObject(FootstepEvent, gameObject.transform, objectRigidbody);
          FootstepEvent.start();
        }
       
    }

    public void OnJump()
    {
        //JumpEvent.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(playerObject, objectRigibody));
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(JumpEvent, gameObject.transform, objectRigidbody);
        //JumpEvent.start();
    }

    public void OnLand()
    {
        //LandEvent.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(playerObject, objectRigibody));
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(LandEvent, gameObject.transform, objectRigidbody);
        LandEvent.start();
    }

    //private void Examples() //Do not call this function, example only
    //{
    //    FootstepEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    //    FootstepEvent.setParameterByName(FootstepParamPath, 0); //metal = 0 Concrete = 1 Wall = 2
    //}

}
