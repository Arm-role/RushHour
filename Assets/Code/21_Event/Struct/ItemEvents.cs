using System;
using UnityEngine;

namespace ItemEvents
{
    public struct TimeOutEvent { }
    public struct ItemEjected
    {
        public readonly string ItemName;
        public readonly Vector2 Position;

        public ItemEjected(string itemName, Vector2 position)
        {
            ItemName = itemName;
            Position = position;
        }
    }
    public struct ItemIdEjected
    {
        public readonly int ItemId;
        public readonly Vector2 Position;

        public ItemIdEjected(int itemId, Vector2 position)
        {
            ItemId = itemId;
            Position = position;
        }
    }
    public struct ItemIdEjectedLaunch
    {
        public readonly int ItemId;

        public ItemIdEjectedLaunch(int itemId)
        {
            ItemId = itemId;
        }
    }
    public struct ItemNameEjectedLaunch
    {
        public readonly string ItemName;

        public ItemNameEjectedLaunch(string itemName)
        {
            ItemName = itemName;
        }
    }

    public struct OrderIdEjectedAndSetUp
    {
        public readonly int OrderID;
        public readonly int MenuID;

        public OrderIdEjectedAndSetUp(int orderID, int menuID)
        {
            OrderID = orderID;
            MenuID = menuID;
        }
    }

    public struct ItemSpawned
    {
        public readonly IInteractable Interactable;

        public ItemSpawned(IInteractable interactable)
        {
            Interactable = interactable;
        }
    }

    public readonly struct PlateReadyForOrder
    {
        public readonly Guid OrderId;
        public readonly IInteractable Plate;

        public PlateReadyForOrder(Guid orderId, IInteractable plate)
        {
            OrderId = orderId;
            Plate = plate;
        }
    }
    public readonly struct ItemSpawnRequested
    {
        public readonly string ItemName;
        public readonly Vector2 Position;
        public readonly Guid OrderId;

        public ItemSpawnRequested(string itemName, Vector2 position, Guid orderId)
        {
            ItemName = itemName;
            Position = position;
            OrderId = orderId;
        }
    }
   
    public struct ItemTransported
    {
        public readonly int SourceID;
        public readonly int DestinationID;
        public readonly int ItemId;

        public ItemTransported(int sourceID, int destinationID, int itemId)
        {
            SourceID = sourceID;
            DestinationID = destinationID;
            ItemId = itemId;
        }
    }
    public struct ItemsTransported
    {
        public readonly int SourceID;
        public readonly int DestinationID;
        public readonly int[] ItemIds;

        public ItemsTransported(int sourceID, int destinationID, int[] itemIds)
        {
            SourceID = sourceID;
            DestinationID = destinationID;
            ItemIds = itemIds;
        }
    }
}
