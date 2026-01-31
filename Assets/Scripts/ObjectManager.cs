using UnityEngine;

sealed class ObjectManager : MonoBehaviour
{
    public GameObject[] layerCollections;
    int[] currentLayerIndex = { 0 };

    public void switchMask(int[] layersToActivate) {
        foreach (int layerIndex in currentLayerIndex) {
            foreach (SpriteRenderer sr in layerCollections[layerIndex].GetComponentsInChildren<SpriteRenderer>())
                sr.enabled = false;
        }
        foreach (int layerIndex in layersToActivate) {
            foreach (SpriteRenderer sr in layerCollections[layerIndex].GetComponentsInChildren<SpriteRenderer>())
                sr.enabled = true;
        }
        currentLayerIndex = layersToActivate;
    }

}
