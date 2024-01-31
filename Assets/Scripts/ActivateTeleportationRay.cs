using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.OpenXR;

public class ActivateTeleportationRay : MonoBehaviour
{
    public GameObject leftTeleportation;
    public GameObject rightTeleportation;

    public InputActionProperty leftActivate;
    public InputActionProperty rightActivate;

    public InputActionProperty leftSelect;
    public InputActionProperty rightSelect;

    //public XRRayInteractor leftGrabRay;//Le pasamos el left Grab Ray
    //public XRRayInteractor rightGrabRay;//Le pasamos el right Grab Ray

    // Update is called once per frame
    void Update()
    {
        //TryGetHitInfo me permite obtener información sobre el punto de impacto, la normal, el número de objeto golpeado y si es válido o no.
        //Esta información se guarda en las variables "leftPos", "leftNormal", "leftNumber" y "leftValid" respectivamente.

        //Si "TryGetHitInfo()" devuelve "true", significa que leftGrabRay está actualmente sobre algún objeto interactivo de la escena.
        //bool isLeftRayHovering = leftGrabRay.TryGetHitInfo(out Vector3 leftPos, out Vector3 leftNormal, out int leftNumber, out bool leftValid);
        //bool isRightRayHovering = rightGrabRay.TryGetHitInfo(out Vector3 rightPos, out Vector3 rightNormal, out int rightNumber, out bool rightValid);

        //Si el valor de la acción de entrada "leftCancel" es 0 (que no tenemos pulsado el botón izquierdo de grip)
        //y el valor de la acción de entrada "leftActivate" es mayor que 0.1f
        //y el leftGrabRay no está sobre un objeto interactable
        //el rayo "leftTeleportation" se activa
        //leftTeleportation.SetActive(!isLeftRayHovering && leftSelect.action.ReadValue<float>() == 0 && leftActivate.action.ReadValue<float>() > 0.1f);

        //Si el valor de la acción de entrada "rightCancel" es 0 (que no tenemos pulsado el botón derecho de grip)
        //y el valor de la acción de entrada "rightActivate" es mayor que 0.1f
        //y el rightGrabRay no está sobre un objeto interactable
        //el rayo "rightTeleportation" se activa
        //rightTeleportation.SetActive(!isRightRayHovering && rightSelect.action.ReadValue<float>() == 0 && rightActivate.action.ReadValue<float>()>0.1f);
    }
}
