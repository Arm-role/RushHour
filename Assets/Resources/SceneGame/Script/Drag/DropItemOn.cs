using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOnTrash: IDropItemTo
{
    public void Execute(DragManager dragManager)
    {
        ParticleManager.instance.CreateParticle("SmokeParticle", dragManager.currentItem.transform.position);
        dragManager.Self_Destruct();
    }
}
public class PlateOnTrash : IDropItemTo
{
    public void Execute(DragManager dragManager)
    {
        KitchenWare kitchenWare = dragManager.GetComponent<KitchenWare>();

        kitchenWare.ClearIntregients();
        ParticleManager.instance.CreateParticle("SmokeParticle", dragManager.currentItem.transform.position);
    }
}
public class OnArrowLeft : IDropItemTo
{
    public void Execute(DragManager dragManager)
    {
        ParticleManager.instance.CreateParticle("SmokeParticle", dragManager.currentItem.transform.position);

        Item item = MonoBehaviour.Instantiate(dragManager.currentHandle.Item);

        PlayerNetwork player = DIPlayerContain.Instance.LocalPlayerNetwork();
        PlayerNetwork playerNetwork = PlayerManager.Instance.GetNearPlayer(0);
        player.TransportItem(playerNetwork, item);

        dragManager.Self_Destruct();
    }
}
public class OnArrowRight : IDropItemTo
{
    public void Execute(DragManager dragManager)
    {
        ParticleManager.instance.CreateParticle("SmokeParticle", dragManager.currentItem.transform.position);

        Item item = MonoBehaviour.Instantiate(dragManager.currentHandle.Item);

        PlayerNetwork player = DIPlayerContain.Instance.LocalPlayerNetwork();
        PlayerNetwork playerNetwork = PlayerManager.Instance.GetNearPlayer(1);
        player.TransportItem(playerNetwork, item);

        dragManager.Self_Destruct();
    }
}
public class OnCounter : IDropItemTo
{
    Collider2D collider;
    public OnCounter(Collider2D colider)
    {
        collider = colider;
    }
    public void Execute(DragManager dragManager)
    {
        KitchenWare kitchenWare = dragManager.currentItem.GetComponent<KitchenWare>();
        ServeManager serveManager = collider.GetComponent<ServeManager>();

        serveManager.AddToServe(kitchenWare.ClearIntregients);
        serveManager.SetFoodOnWare(kitchenWare.GetFoodOnWare());
        ParticleManager.instance.CreateParticle("SmokeParticle", dragManager.currentItem.transform.position);
    }
}
public class OnWare : IDropItemTo
{
    Collider2D collider;
    public OnWare(Collider2D colider)
    {
        collider = colider;
    }

    public void Execute(DragManager dragManager)
    {
        if (collider.TryGetComponent<KitchenWare>(out var kitchenWere))
        {
            dragManager.PushItem(kitchenWere.PickUpItem);
            ParticleManager.instance.CreateParticle("SmokeParticle", dragManager.currentItem.transform.position);
        }
    }
}
public class OnTool : IDropItemTo
{
    Collider2D collider;
    public OnTool(Collider2D colider)
    {
        collider = colider;
    }

    public void Execute(DragManager dragManager)
    {
        if (collider.TryGetComponent<KitchenTool>(out var kitchenTool))
        {
            dragManager.PushItem(kitchenTool.PickUpItem);
            ParticleManager.instance.CreateParticle("SmokeParticle", dragManager.currentItem.transform.position);
        }
    }
}