using System.Collections.Generic;
using System.Linq;
using copiers;
using Lean.Touch;
using loaders;
using models.sp;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform swapParent;
    public Transform sparePartsParent;
    public Camera activeCamera;
    
    // private ObjectCopier _copier;
    private AssetBundleLoader _loader;
    
    // Start is called before the first frame update
    private void Awake()
    {
        // initialize helpers
        _loader = new AssetBundleLoader();
        // _copier = ScriptableObject.CreateInstance<ObjectCopier>();
    }

    private void Start()
    {
        var arr = FillSpareParts();
        foreach (var sp in arr)
        {
            // print($"spp: {sp.initProperties}");
        }
    }

    private IEnumerable<SpNew> FillSpareParts()
    {
        var res = Enumerable.Empty<SpNew>().ToList();
        var sparePartsSrc = _loader.GetAssetByName("dino/island1.stego", "stego_model_exploded");
        var lSwap = swapParent.GetComponent<LeanSwap>();
        
        foreach (var spTmp  in sparePartsSrc.GetComponentsInChildren<Transform>())
        {
            if (sparePartsSrc.name.Equals(spTmp.name)) continue;
            var spNew = ObjectCopier.CopySpNew(spTmp, sparePartsParent);

            // positioning 
            var spNewGo = spNew.gameObject;
            spNewGo.layer = 14;
            spNew.renderingCamera = activeCamera;
            var spNewTransform = spNew.transform;
            spNewTransform.localPosition = Vector3.zero;
            lSwap.Prefabs.Add(spNewTransform);
            
            res.Add(spNew);
        }

        return res;
    }
    
    
    
}
