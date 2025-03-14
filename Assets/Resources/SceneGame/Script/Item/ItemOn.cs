using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOnTrash : IDropTo
{
    public void ItemOn(ItemDrag itemDrag)
    {
        ParticleManager.instance.CreateParticle("SmokeParticle", itemDrag.transform.position);
        itemDrag.Self_Destruct();
    }
}
public class PlateOnTrash : IDropTo
{
    public void ItemOn(ItemDrag itemDrag)
    {
        KitchenWare kitchenWare = itemDrag.GetComponent<KitchenWare>();

        kitchenWare.ClearIntregients();
        ParticleManager.instance.CreateParticle("SmokeParticle", itemDrag.transform.position);
    }
}
public class OnArrowLeft : IDropTo
{
    public void ItemOn(ItemDrag itemDrag)
    {
        ParticleManager.instance.CreateParticle("SmokeParticle", itemDrag.transform.position);

        PlayerNetwork player = DIPlayerContain.Instance.LocalPlayerNetwork();
        ItemHandle itemHandle = itemDrag.GetComponent<ItemHandle>();
        Item item = MonoBehaviour.Instantiate(itemHandle.Item);

        PlayerNetwork playerNetwork = PlayerManager.Instance.GetNearPlayer(0);
        player.TransportItem(playerNetwork, item);
        itemDrag.Self_Destruct();
    }
}
public class OnArrowRight : IDropTo
{
    public void ItemOn(ItemDrag itemDrag)
    {
        ParticleManager.instance.CreateParticle("SmokeParticle", itemDrag.transform.position);

        ItemHandle itemHandle = itemDrag.GetComponent<ItemHandle>();
        Item item = MonoBehaviour.Instantiate(itemHandle.Item);

        PlayerNetwork player = DIPlayerContain.Instance.LocalPlayerNetwork();
        PlayerNetwork playerNetwork = PlayerManager.Instance.GetNearPlayer(1);
        player.TransportItem(playerNetwork, item);

        itemDrag.Self_Destruct();
    }
}
public class OnCounter : IDropTo
{
    Collider2D collider;
    public OnCounter(Collider2D colider)
    {
        collider = colider;
    }
    public void ItemOn(ItemDrag itemDrag)
    {
        KitchenWare kitchenWare = itemDrag.GetComponent<KitchenWare>();
        ServeManager serveManager = collider.GetComponent<ServeManager>();

        serveManager.AddToServe(kitchenWare.ClearIntregients);
        serveManager.SetFoodOnWare(kitchenWare.GetFoodOnWare());
        ParticleManager.instance.CreateParticle("SmokeParticle", itemDrag.transform.position);
    }
}
public class OnWare : IDropTo
{
    Collider2D collider;
    public OnWare(Collider2D colider)
    {
        collider = colider;
    }

    public void ItemOn(ItemDrag itemDrag)
    {
        KitchenWare kitchenWere = null;
        bool ware = collider.TryGetComponent<KitchenWare>(out kitchenWere);

        if (ware)
        {
            itemDrag.SendGamObject += kitchenWere.GetOb;
            itemDrag.PushItem();
            ParticleManager.instance.CreateParticle("SmokeParticle", itemDrag.transform.position);
        }
    }
}
public class OnTool : IDropTo
{
    Collider2D collider;
    public OnTool(Collider2D colider)
    {
        collider = colider;
    }

    public void ItemOn(ItemDrag itemDrag)
    {
        KitchenTool kitchenTool = null;

        bool tool = collider.TryGetComponent<KitchenTool>(out kitchenTool);

        if (tool)
        {
            itemDrag.SendGamObject += kitchenTool.GetOb;
            itemDrag.PushItem();
            ParticleManager.instance.CreateParticle("SmokeParticle", itemDrag.transform.position);

        }
    }
}