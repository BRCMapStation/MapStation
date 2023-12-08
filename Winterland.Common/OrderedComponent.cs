using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Winterland.Common {
    public abstract class OrderedComponent : MonoBehaviour{
        [HideInInspector]
        public int Order = -1;

        public virtual bool IsPeer(Component other) {
            if (other.GetType() == GetType())
                return true;
            return false;
        }

        protected virtual void OnValidate() {
            if (Order < 0 || GetOrderCollision(Order))
                Order = GetHighestOrder() + 1;
        }

        public void MoveUp() {
            var componentAbove = GetFirstComponentAbove(Order);
            if (componentAbove != null) {
                var oldOrder = Order;
                Order = componentAbove.Order;
                componentAbove.Order = oldOrder;
            }
        }

        public void MoveDown() {
            var componentAbove = GetFirstComponentBelow(Order);
            if (componentAbove != null) {
                var oldOrder = Order;
                Order = componentAbove.Order;
                componentAbove.Order = oldOrder;
            }
        }

        private OrderedComponent GetFirstComponentAbove(int order) {
            var components = GetComponentsAbove(order);
            OrderedComponent highestComponent = null;
            foreach(var component in components) {
                if (highestComponent == null) {
                    highestComponent = component;
                }
                else {
                    if (component.Order > highestComponent.Order)
                        highestComponent = component;
                }
            }
            return highestComponent;
        }

        private OrderedComponent GetFirstComponentBelow(int order) {
            var components = GetComponentsBelow(order);
            OrderedComponent lowestComponent = null;
            foreach (var component in components) {
                if (lowestComponent == null) {
                    lowestComponent = component;
                } else {
                    if (component.Order < lowestComponent.Order)
                        lowestComponent = component;
                }
            }
            return lowestComponent;
        }

        private OrderedComponent[] GetComponentsAbove(int order) {
            var orderedComponents = gameObject.GetComponents<OrderedComponent>().Where(x => x.Order < order && IsPeer(x)).ToArray();
            return orderedComponents;
        }

        private OrderedComponent[] GetComponentsBelow(int order) {
            var orderedComponents = gameObject.GetComponents<OrderedComponent>().Where(x => x.Order > order && IsPeer(x)).ToArray();
            return orderedComponents;
        }

        private int GetHighestOrder() {
            var orderedComponents = gameObject.GetComponents<OrderedComponent>().Where(x => IsPeer(x));
            var highestOrder = -1;
            foreach(var component in orderedComponents) {
                if (component.Order > highestOrder)
                    highestOrder = component.Order;
            }
            return highestOrder;
        }

        private bool GetOrderCollision(int order) {
            return gameObject.GetComponents<OrderedComponent>().Any(x => x.Order == order && IsPeer(x) && x != this);
        }

        public static T[] GetComponentsOrdered<T>(GameObject gameObject, Func<T, bool> predicate = null) where T : OrderedComponent {
            var comps = gameObject.GetComponents<T>();
            if (predicate != null)
                comps = comps.Where(predicate).ToArray();
            var compList = comps.ToList();
            compList.Sort((x, y) => {
                return (x.Order - y.Order);
            });
            return compList.ToArray();
        }
    }
}
