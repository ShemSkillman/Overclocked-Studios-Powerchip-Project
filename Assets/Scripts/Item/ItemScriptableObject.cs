using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item", order = 1)]
public class ItemScriptableObject : ScriptableObject
{
    public string itemName;

    public Sprite chipSprite;
    public GameObject chipModel;
    public Material chipMeshMaterial;

    public ChipBuff[] chipBuffs;
}