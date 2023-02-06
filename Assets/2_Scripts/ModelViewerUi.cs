using UnityEngine;

public class ModelViewerUi : MonoBehaviour
{
    [SerializeField] LevelData levelData;
    [SerializeField] GameObject[] skins;
    GameObject localSkin;
    // Start is called before the first frame update
    void Start()
    {
        localSkin = Instantiate(skins[levelData.GetSkinSelected()], transform.position, transform.rotation);
        localSkin.transform.parent = gameObject.transform;
        localSkin.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
    }

}
