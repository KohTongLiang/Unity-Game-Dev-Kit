using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GameCore
{
    /// <summary>
    /// A decorator node that repeatedly runs its child node until it returns Failure.
    /// Useful for tasks that should keep attempting until they fail.
    /// </summary>
    public class UntilFail : Node
    {
        public UntilFail(string name) : base(name)
        {
        }

        public override Status Process()
        {
            // Process the child node; if it fails, reset and return Failure.
            if (children[0].Process() == Status.Failure)
            {
                Reset();
                return Status.Failure;
            }

            // Continue running as long as the child does not fail.
            return Status.Running;
        }
    }

    /// <summary>
    /// A decorator node that repeatedly runs its child node until it returns Success.
    /// Useful for tasks that should keep attempting until they succeed.
    /// </summary>
    public class UntilSuccess : Node
    {
        public UntilSuccess(string name) : base(name)
        {
        }

        public override Status Process()
        {
            // Process the child node; if it succeeds, reset and return Success.
            if (children[0].Process() == Status.Success)
            {
                Reset();
                return Status.Success;
            }

            // Continue running until the child returns Success.
            return Status.Running;
        }
    }

    /// <summary>
    /// A decorator node that inverts the result of its child node.
    /// If the child returns Success, it returns Failure and vice versa.
    /// Useful for negating conditions or creating alternate behavior.
    /// </summary>
    public class Inverter : Node
    {
        public Inverter(string name) : base(name)
        {
        }

        public override Status Process()
        {
            // Process the child and invert the status.
            switch (children[0].Process())
            {
                case Status.Running:
                    return Status.Running; // Running remains unchanged.
                case Status.Failure:
                    return Status.Success; // Invert Failure to Success.
                default:
                    return Status.Failure; // Invert Success to Failure.
            }
        }
    }

    /// <summary>
    /// A selector node that runs each child node in random order.
    /// Continues executing children until one succeeds or all fail.
    /// Useful for randomizing behavior within a priority selection.
    /// </summary>
    public class RandomSelector : PrioritySelector
    {
        protected override List<Node> SortChildren()
        {
            return children.Shuffle().ToList();
        }

        public RandomSelector(string name, int priority = 0) : base(name, priority)
        {
        }
    }

    /// <summary>
    /// Selector that runs each child node exclusively based on priority.
    /// Only one child node is active at a time, and it must complete before switching to another.
    /// </summary>
   public class ExclusiveSelector : Node
   {
       private int activeChildIndex = -1; // Tracks the currently active child node
       private List<Node> sortedChildren; // List of children sorted by priority

       public ExclusiveSelector(string name, int priority = 0) : base(name, priority)
       {
           SortChildrenByPriority();
       }

       // Sort children based on priority in descending order (higher priority first)
       private void SortChildrenByPriority()
       {
           sortedChildren = children.OrderByDescending(child => child.priority).ToList();
       }

       public override void AddChild(Node child)
       {
           base.AddChild(child);
           SortChildrenByPriority(); // Re-sort whenever a new child is added
       }

       public override Status Process()
       {
           // Check if any higher-priority node can start running
           for (var i = 0; i < sortedChildren.Count; i++)
           {
               // Skip the currently active child if it's lower priority
               if (activeChildIndex != -1 && i >= activeChildIndex) break; // Stop checking higher-priority nodes

               var higherPriorityStatus = sortedChildren[i].Process();

               if (higherPriorityStatus == Status.Running)
               {
                   // If a higher-priority node can run, reset the current active child
                   if (activeChildIndex != -1) sortedChildren[activeChildIndex].Reset();
                   activeChildIndex = i; // Set the higher-priority node as active
                   return Status.Running;
               }

               if (higherPriorityStatus == Status.Success)
               {
                   Reset();
                   return Status.Success; // If it completes successfully, return Success
               }
           }

           // If no higher-priority nodes are eligible, continue with the current active child
           if (activeChildIndex != -1)
           {
               var activeStatus = sortedChildren[activeChildIndex].Process();
               if (activeStatus != Status.Running)
               {
                   sortedChildren[activeChildIndex].Reset();
                   activeChildIndex = -1; // Clear the active child if it completes
               }

               return activeStatus;
           }

           // If no child is running, return Failure
           Reset();
           return Status.Failure;
       }

       public override void Reset()
       {
           base.Reset();
           activeChildIndex = -1; // Reset the active child index
       }
   }

    /// <summary>
    /// A selector node that runs child nodes based on their priority.
    /// Runs each child in order of priority until one succeeds.
    /// </summary>
    public class PrioritySelector : Selector
    {
        private List<Node> sortedChildren;
        private List<Node> SortedChildren => sortedChildren ??= SortChildren();

        // Sorts children by priority in descending order.
        protected virtual List<Node> SortChildren()
        {
            return children.OrderByDescending(child => child.priority).ToList();
        }

        public PrioritySelector(string name, int priority = 0) : base(name, priority)
        {
        }

        public override void Reset()
        {
            base.Reset();
            sortedChildren = null; // Reset the sorted list on Reset.
        }

        public override Status Process()
        {
            // Iterate over sorted children based on priority.
            foreach (var child in SortedChildren)
                switch (child.Process())
                {
                    case Status.Running:
                        return Status.Running; // Return if any child is still running.
                    case Status.Success:
                        Reset();
                        return Status.Success; // Success on first successful child.
                    default:
                        continue; // Continue to the next child on Failure.
                }

            Reset();
            return Status.Failure; // Fail if all children fail.
        }
    }

    /// <summary>
    /// A selector node that runs each child in sequence until one succeeds.
    /// If no children succeed, returns Failure.
    /// </summary>
    public class Selector : Node
    {
        public Selector(string name, int priority = 0) : base(name, priority)
        {
        }

        public override Status Process()
        {
            if (currentChild < children.Count)
                // Process each child; move to the next on Failure.
                switch (children[currentChild].Process())
                {
                    case Status.Running:
                        return Status.Running; // Keep running if child is in progress.
                    case Status.Success:
                        Reset();
                        return Status.Success; // Succeed on the first successful child.
                    default:
                        currentChild++;
                        return Status.Running; // Move to the next child on Failure.
                }

            Reset();
            return Status.Failure; // Fail if all children fail.
        }
    }

    /// <summary>
    /// A sequence node that runs each child in order until all succeed.
    /// If any child fails, the entire sequence fails.
    /// Useful for chaining tasks that require completion in order.
    /// </summary>
    public class Sequence : Node
    {
        public Sequence(string name, int priority = 0) : base(name, priority)
        {
        }

        public override Status Process()
        {
            if (currentChild < children.Count)
                // Process each child in sequence.
                switch (children[currentChild].Process())
                {
                    case Status.Running:
                        return Status.Running; // Continue running if child is in progress.
                    case Status.Failure:
                        currentChild = 0; // Reset on Failure.
                        return Status.Failure; // Fail the sequence if any child fails.
                    default:
                        currentChild++; // Move to the next child on Success.
                        return currentChild == children.Count ? Status.Success : Status.Running;
                }

            Reset();
            return Status.Success; // Succeed if all children succeed.
        }
    }

    /// <summary>
    /// A leaf node that executes a specific strategy.
    /// It represents the actual action or behavior in the tree.
    /// </summary>
    public class Leaf : Node
    {
        private readonly IStrategy strategy;

        public Leaf(string name, IStrategy strategy, int priority = 0) : base(name, priority)
        {
            this.strategy = strategy;
        }

        public override Status Process()
        {
            return strategy.Process();
            // Execute the strategy's Process method.
        }

        public override void Reset()
        {
            strategy.Reset();
            // Reset the strategy when required.
        }
    }

    /// <summary>
    /// Base class for all nodes in the behavior tree.
    /// Defines the structure and behavior for individual tree nodes.
    /// </summary>
    public class Node
    {
        public enum Status
        {
            Success,
            Failure,
            Running
        } // Statuses a node can return.

        public readonly string name;
        public readonly int priority;

        public readonly List<Node> children = new();
        protected int currentChild;

        public Node(string name = "Node", int priority = 0)
        {
            this.name = name;
            this.priority = priority;
        }

        public virtual void AddChild(Node child)
        {
            children.Add(child);
            // Add a child node.
        }

        public virtual Status Process()
        {
            return children[currentChild].Process();
            // Default Process.
        }

        public virtual void Reset()
        {
            currentChild = 0; // Reset the current child index.
            foreach (var child in children) child.Reset(); // Reset each child node.
        }
    }

    /// <summary>
    /// An interface defining the behavior policy for when a node should stop.
    /// Allows customization of behavior exit conditions.
    /// </summary>
    public interface IPolicy
    {
        bool ShouldReturn(Node.Status status); // Defines when to exit based on status.
    }

    /// <summary>
    /// Static class that contains various policy implementations for node behavior.
    /// Allows for configurable tree behavior based on status.
    /// </summary>
    public static class Policies
    {
        public static readonly IPolicy RunForever = new RunForeverPolicy();
        public static readonly IPolicy RunUntilSuccess = new RunUntilSuccessPolicy();
        public static readonly IPolicy RunUntilFailure = new RunUntilFailurePolicy();

        private class RunForeverPolicy : IPolicy
        {
            public bool ShouldReturn(Node.Status status)
            {
                return false;
                // Never exit.
            }
        }

        private class RunUntilSuccessPolicy : IPolicy
        {
            public bool ShouldReturn(Node.Status status)
            {
                return status == Node.Status.Success;
                // Exit on Success.
            }
        }

        private class RunUntilFailurePolicy : IPolicy
        {
            public bool ShouldReturn(Node.Status status)
            {
                return status == Node.Status.Failure;
                // Exit on Failure.
            }
        }
    }

    /// <summary>
    /// A class that represents a behaviour tree.
    /// </summary>
    public class BehaviourTree : Node
    {
        private readonly IPolicy policy;
        public Node CurrentChild => children[currentChild];

        public BehaviourTree(string name, IPolicy policy = null) : base(name)
        {
            this.policy = policy ?? Policies.RunForever;
        }

        public override Status Process()
        {
            var status = children[currentChild].Process();
            if (policy.ShouldReturn(status)) return status;

            currentChild = (currentChild + 1) % children.Count;
            return Status.Running;
        }

        public void PrintTree()
        {
            var sb = new StringBuilder();
            PrintNode(this, 0, sb);
            Debug.Log(sb.ToString());
        }

        private static void PrintNode(Node node, int indentLevel, StringBuilder sb)
        {
            sb.Append(' ', indentLevel * 2).AppendLine(node.name);
            foreach (var child in node.children) PrintNode(child, indentLevel + 1, sb);
        }
    }
}