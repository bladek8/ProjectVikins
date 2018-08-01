using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MaterialPlayer : RawImage
{
    //Material bMaterial;

    public override Material GetModifiedMaterial(Material bMaterial)
    {
        // Apply the mask.
        Material tmp = base.GetModifiedMaterial(bMaterial);
        //this.bMaterial = tmp;
        // Pass your custom shader parameters.
        tmp.SetColor("_Color", Color.blue);
        // return the material with Mask + Customs applied.
        return tmp;
    }
}
