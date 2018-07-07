﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AillieoUtils
{
    public static class DragDropHelper
    {

        public static bool IsChannelMatch(DragDropTarget target, DragDropItem item)
        {
            if(target.universalMatching || item.universalMatching)
            {
                return true;
            }
            return (target.matchingChannel & item.matchingChannel) != 0;
        }

        public static ScrollRect FindScrollRect(DragDropItem item)
        {
            Transform parent = item.transform.parent;
            ScrollRect ret = null;
            while (parent)
            {
                ret = parent.GetComponent<ScrollRect>();
                if (ret)
                {
                    break;
                }
                parent = parent.parent;
            }
            return ret;
        }

        public static bool TryAttachItem(DragDropItem item, DragDropTarget target)
        {
            item.SetInitialTarget(target);
            return true;
        }


        public static bool TryDetachItem(DragDropItem item, DragDropTarget target)
        {
            if(item.attachedTarget == target && target.HasItemAttached(item))
            {
                DragDropEventData eventData = new DragDropEventData();
                eventData.Reset();
                eventData.external = true;
                eventData.target = target;
                eventData.item = item;
                target.OnItemDetach(eventData);
                item.OnItemDetach(eventData);
                item.OnSetFree(eventData);
                return true;
            }
            else
            {
                return false;
            }
        }


        public static int DetachAllItems(DragDropTarget target)
        {
            var items = target.GetAllAttachedItems();
            DragDropEventData eventData = new DragDropEventData();
            eventData.Reset();
            eventData.external = true;
            eventData.target = target;
            foreach (var item in items)
            {
                eventData.item = item;
                target.OnItemDetach(eventData);
                item.OnItemDetach(eventData);
                item.OnSetFree(eventData);
            }
            return items.Length;
        }
    }

}