using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "ItemLibrary", menuName = "Library/ItemLibrary")]
public class ItemLibrary : CombineLibraryBase<AssetReferenceT<Item>, AssetReferenceGameObject> { }