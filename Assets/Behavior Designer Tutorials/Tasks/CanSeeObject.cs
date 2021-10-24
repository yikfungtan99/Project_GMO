using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Tutorials
{
    [TaskCategory("Tutorial")]
    [TaskIcon("Assets/Behavior Designer Tutorials/Tasks/Editor/{SkinColor}CanSeeObjectIcon.png")]
    public class CanSeeObject : Conditional
    {
        [Tooltip("The location of the vision cone origin")]
        [SerializeField] private Transform eyeTransform;

        [Tooltip("The object that we are searching for")]
        [SerializeField] private GameObject targetObject;

        [Tooltip("The field of view angle of the agent (in degrees)")]
        [SerializeField] private float fieldOfViewAngle = 90;

        [Tooltip("The distance that the agent can see")]
        [SerializeField] private float viewDistance = 1000;

        [Tooltip("The object that is within sight")]
        public SharedGameObject returnedObject;

        private GameObject returnedTarget;

        /// <summary>
        /// Returns success if an object was found otherwise failure
        /// </summary>
        /// <returns></returns>
        public override TaskStatus OnUpdate()
        {
            returnedTarget = WithinSight(targetObject, fieldOfViewAngle, viewDistance);
            if (returnedTarget != null) {
                // Return success if an object was found
                returnedObject.Value = returnedTarget;
                return TaskStatus.Success;
            }
            // An object is not within sight so return failure 
            return TaskStatus.Failure;
        }

        /// <summary>
        /// Determines if the targetObject is within sight of the transform.
        /// </summary>
        private GameObject WithinSight(GameObject targetObject, float fieldOfViewAngle, float viewDistance)
        {
            if (targetObject == null) {
                return null;
            }

            var direction = targetObject.transform.position - transform.position;
            direction.y = 0;
            var angle = Vector3.Angle(direction, transform.forward);
            if (direction.magnitude < viewDistance && angle < fieldOfViewAngle * 0.5f) {
                // The hit agent needs to be within view of the current agent
                if (LineOfSight(targetObject)) {
                    return targetObject; // return the target object meaning it is within sight
                }
            }
            return null;
        }

        /// <summary>
        /// Returns true if the target object is within the line of sight.
        /// </summary>
        private bool LineOfSight(GameObject targetObject)
        {
            RaycastHit hit;
            if (Physics.Linecast(transform.position, targetObject.transform.position, out hit)) {
                if (hit.transform.IsChildOf(targetObject.transform) || targetObject.transform.IsChildOf(hit.transform)) {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Draws the line of sight representation
        /// </summary>
        public override void OnDrawGizmos()
        {
#if UNITY_EDITOR
            var oldColor = UnityEditor.Handles.color;
            var color = Color.yellow;
            color.a = 0.1f;
            UnityEditor.Handles.color = color;

            if (eyeTransform)
            {
                var halfFOV = fieldOfViewAngle * 0.5f;
                var beginDirection = Quaternion.AngleAxis(-halfFOV, Vector3.up) * eyeTransform.forward;
                UnityEditor.Handles.DrawSolidArc(eyeTransform.position, eyeTransform.up, beginDirection, fieldOfViewAngle, viewDistance);

                UnityEditor.Handles.color = oldColor;
            }
#endif
        }
    }
}