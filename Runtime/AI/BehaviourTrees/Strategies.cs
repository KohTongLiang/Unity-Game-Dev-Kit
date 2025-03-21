using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace GameCore {
    /// <summary>
    /// Interface representing a strategy that can be processed and reset.
    /// </summary>
    public interface IStrategy {
        /// <summary>
        /// Process the strategy and return the status of the node.
        /// </summary>
        /// <returns>Status of the node after processing.</returns>
        Node.Status Process();

        /// <summary>
        /// Reset the strategy to its initial state.
        /// </summary>
        void Reset() {
            // Noop
        }
    }

    /// <summary>
    /// A strategy that executes a specified action.
    /// </summary>
    public class ActionStrategy : IStrategy {
        readonly Action doSomething;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionStrategy"/> class.
        /// </summary>
        /// <param name="doSomething">The action to execute.</param>
        public ActionStrategy(Action doSomething) {
            this.doSomething = doSomething;
        }

        /// <summary>
        /// Executes the action and returns success.
        /// </summary>
        /// <returns>Status.Success after executing the action.</returns>
        public Node.Status Process() {
            doSomething();
            return Node.Status.Success;
        }
    }

    /// <summary>
    /// A strategy that evaluates a predicate to determine success or failure.
    /// </summary>
    public class Condition : IStrategy {
        readonly Func<bool> predicate;

        /// <summary>
        /// Initializes a new instance of the <see cref="Condition"/> class.
        /// </summary>
        /// <param name="predicate">The predicate to evaluate.</param>
        public Condition(Func<bool> predicate) {
            this.predicate = predicate;
        }

        /// <summary>
        /// Evaluates the predicate and returns success or failure.
        /// </summary>
        /// <returns>Status.Success if predicate is true, otherwise Status.Failure.</returns>
        public Node.Status Process() => predicate() ? Node.Status.Success : Node.Status.Failure;
    }

    /// <summary>
    /// A strategy for patrolling between multiple points.
    /// </summary>
    public class PatrolWaypointsStrategy : IStrategy {
        readonly Transform entity;
        readonly NavMeshAgent agent;
        readonly List<Transform> patrolPoints;
        readonly float patrolSpeed;
        int currentIndex;
        bool isPathCalculated;

        /// <summary>
        /// Initializes a new instance of the <see cref="PatrolStrategy"/> class.
        /// </summary>
        /// <param name="entity">The entity to move.</param>
        /// <param name="agent">The NavMeshAgent for pathfinding.</param>
        /// <param name="patrolPoints">The list of patrol points.</param>
        /// <param name="patrolSpeed">The speed of patrol.</param>
        public PatrolWaypointsStrategy(Transform entity, NavMeshAgent agent, List<Transform> patrolPoints, float patrolSpeed = 2f) {
            this.entity = entity;
            this.agent = agent;
            this.patrolPoints = patrolPoints;
            this.patrolSpeed = patrolSpeed;
        }


        /// <summary>
        /// Processes the patrol strategy and updates the entity's path.
        /// </summary>
        /// <returns>Status.Running during patrol, Status.Success when completed.</returns>
        public Node.Status Process() {
            if (currentIndex == patrolPoints.Count) return Node.Status.Success;

            var target = patrolPoints[currentIndex];
            agent.SetDestination(target.position);
            entity.LookAt(target.position.With(y: entity.position.y));

            if (isPathCalculated && agent.remainingDistance < 0.1f) {
                currentIndex++;
                isPathCalculated = false;
            }

            if (agent.pathPending) {
                isPathCalculated = true;
            }

            return Node.Status.Running;
        }

        /// <summary>
        /// Resets the patrol strategy to the initial state.
        /// </summary>
        public void Reset() => currentIndex = 0;
    }
}