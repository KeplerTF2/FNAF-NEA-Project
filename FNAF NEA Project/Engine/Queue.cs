using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable enable

namespace NEA_Project.Engine
{
    public class Queue
    {
        private class QueueItem
        {
            public dynamic item;
            public QueueItem? nextItem = null;

            public QueueItem(dynamic item) 
            {
                this.item = item;
            }

            public QueueItem GetTail()
            {
                QueueItem item = this;
                while (item.nextItem != null) {
                    item = item.nextItem;
                }
                return item;
            }
        }

        private QueueItem? head;
        private int length = 0;

        public int Length { get { return length; } }

        public Queue() { }

        // Adds an item to the end of the queue
        public void Enqueue(dynamic item)
        {
            length++;

            QueueItem newQueueItem = new QueueItem(item);

            if (head != null) { head.GetTail().nextItem = newQueueItem; }
            else { head = newQueueItem; }
        }

        // Removes and returns an item from the front of the queue
        public dynamic? Dequeue()
        {
            if (head == null) { return null; }
            else 
            {
                length--;

                dynamic? returnItem = head.item;
                head = head.nextItem;

                return returnItem;
            }
        }
    }
}
