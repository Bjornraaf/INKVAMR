using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class PuzzlePiece : XRGrabInteractable
{
    [SerializeField]
    private Transform[] snapPoints; // Array of snap points on this piece

    [SerializeField]
    private float snapThreshold = 0.1f; // Distance threshold for snapping

    private bool isSnapped = false;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        CheckForSnap();
    }

    private void CheckForSnap()
    {
        if (isSnapped) return;

        // Find all other puzzle pieces in the scene
        PuzzlePiece[] pieces = FindObjectsByType<PuzzlePiece>(FindObjectsSortMode.None);

        foreach (var piece in pieces)
        {
            if (piece == this) continue;

            foreach (Transform mySnapPoint in snapPoints)
            {
                foreach (Transform theirSnapPoint in piece.snapPoints)
                {
                    float distance = Vector3.Distance(mySnapPoint.position, theirSnapPoint.position);

                    if (distance < snapThreshold)
                    {
                        SnapToPiece(mySnapPoint, theirSnapPoint, piece);
                        return;
                    }
                }
            }
        }
    }

private void SnapToPiece(Transform mySnapPoint, Transform theirSnapPoint, PuzzlePiece otherPiece)
{
    // Move the puzzle piece so that mySnapPoint aligns with theirSnapPoint
    Vector3 targetPosition = transform.position + (theirSnapPoint.position - mySnapPoint.position);
    Quaternion targetRotation = transform.rotation * Quaternion.FromToRotation(mySnapPoint.forward, theirSnapPoint.forward);

    // Apply position and rotation
    transform.position = targetPosition;
    transform.rotation = targetRotation;

    // Mark as snapped and disable interaction
    isSnapped = true;
    interactionLayers = 0; // Disable further interaction
    Debug.Log("Piece snapped!");
}

}
