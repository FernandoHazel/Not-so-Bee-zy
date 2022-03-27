using UnityEngine;

public class HoneyComb : MonoBehaviour
{
    public ParticleSystem grabHoney;
    //[SerializeField] AudioClip honeyCombClip;

    public int honeycombIndex; //cual honeycomb es este

    [SerializeField] LevelData levelData; //el scriptableobject que guarda la info
    [SerializeField] LevelDataGameManager levelDataGm;

    [SerializeField] Material honeycombMaterial;
    [SerializeField] Material honeycombMaterialTransparent;
    private void Start()
    {
        if (levelData.GetSpecificHoneycombs(levelDataGm.levelIndex, honeycombIndex)) //aqui es donde se cambiará material al transparente
        {
            //gameObject.SetActive(false);
            for (int x = 0; x < 3; x++)
            {
                gameObject.transform.GetChild(x).GetComponent<MeshRenderer>().material = honeycombMaterialTransparent;
            }
        }

        else 
        {
            for (int x = 0; x < 3; x++)
            {
                gameObject.transform.GetChild(x).GetComponent<MeshRenderer>().material = honeycombMaterial;
            }
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //grabHoney.Play();
            //AudioManager.Instance.PlaySfxOnce(honeyCombClip, transform);

            /*
            if (levelData.GetSpecificHoneycombs(levelDataGm.levelIndex, honeycombIndex)) //si ya se agarró, no pasa nada
            {
                return;
            }

            else //cuando lo agarra, cambia su specific honeycomb y también agregalo a global honeycomb
            {
                levelData.SetSpecificHoneycombs(levelDataGm.levelIndex, honeycombIndex);
                levelData.AddHoneycombsGlobal();
            }
            */
        }
    }

    public bool CheckHoneyhomb() 
    {
        return levelData.GetSpecificHoneycombs(levelDataGm.levelIndex, honeycombIndex);
    }

    public void UpdateInLevelData() 
    {
        levelData.SetSpecificHoneycombs(levelDataGm.levelIndex, honeycombIndex);
        levelData.AddHoneycombsGlobal();
    }
}
