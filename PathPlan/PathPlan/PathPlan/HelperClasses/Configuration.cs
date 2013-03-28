using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using PathPlan.ObstacleNS;

namespace PathPlan.HelperClasses
{
    public class Configuration
    {
        public FloatRectangle CollisionRectangle;
        private float rotation;
        public float Rotation
        {
            get
            {
                return rotation;
            }
            set
            {
                rotation = value % MathHelper.TwoPi;
            }
        }
        public Vector2 Origin;
        public List<Configuration> neighbors = new List<Configuration>();


        #region DijkstraVariables
        public Configuration dijkstraParent = null;
        public float dijkstraDistance = float.MaxValue;
        public bool dijkstraCompleted = false;
        #endregion

        public Configuration(float x, float y, float width, float height, float Rotation)
        {
            this.CollisionRectangle = new FloatRectangle(x, y, width, height);
            this.rotation = Rotation;
            Origin = new Vector2(CollisionRectangle.size.X / 2, CollisionRectangle.size.Y / 2);

        }

        public Configuration(FloatRectangle CollisionRectangle, float Rotation)
        {
            this.CollisionRectangle = CollisionRectangle;
            this.rotation = Rotation;
            Origin = new Vector2(CollisionRectangle.size.X / 2, CollisionRectangle.size.Y / 2);

        }
        /// <summary>
        /// Used for changing the X and Y position of the RotatedRectangle
        /// </summary>
        /// <param name="theXPositionAdjustment"></param>
        /// <param name="theYPositionAdjustment"></param>
        public void ChangePosition(float theXPositionAdjustment, float theYPositionAdjustment)
        {
            CollisionRectangle.position.X += theXPositionAdjustment;
            CollisionRectangle.position.Y += theYPositionAdjustment;
        }

        /// <summary>
        /// This intersects method can be used to check a standard XNA framework Rectangle
        /// object and see if it collides with a Rotated Rectangle object
        /// </summary>
        /// <param name="theRectangle"></param>
        /// <returns></returns>
        public bool Intersects(FloatRectangle theRectangle)
        {
            return Intersects(new Configuration(theRectangle, 0.0f));
        }

        /// <summary>
        /// Check to see if two Rotated Rectangls have collided
        /// </summary>
        /// <param name="theRectangle"></param>
        /// <returns></returns>
        public bool Intersects(Configuration theRectangle)
        {
            //Calculate the Axis we will use to determine if a collision has occurred
            //Since the objects are rectangles, we only have to generate 4 Axis (2 for
            //each rectangle) since we know the other 2 on a rectangle are parallel.
            List<Vector2> aRectangleAxis = new List<Vector2>();
            aRectangleAxis.Add(UpperRightCorner() - UpperLeftCorner());
            aRectangleAxis.Add(UpperRightCorner() - LowerRightCorner());
            aRectangleAxis.Add(theRectangle.UpperLeftCorner() - theRectangle.LowerLeftCorner());
            aRectangleAxis.Add(theRectangle.UpperLeftCorner() - theRectangle.UpperRightCorner());

            //Cycle through all of the Axis we need to check. If a collision does not occur
            //on ALL of the Axis, then a collision is NOT occurring. We can then exit out 
            //immediately and notify the calling function that no collision was detected. If
            //a collision DOES occur on ALL of the Axis, then there is a collision occurring
            //between the rotated rectangles. We know this to be true by the Seperating Axis Theorem
            foreach (Vector2 aAxis in aRectangleAxis)
            {
                if (!IsAxisCollision(theRectangle, aAxis))
                {
                    return false;
                }
            }

            return true;
        }
        
        /// <summary>
        /// Determines if a collision has occurred on an Axis of one of the
        /// planes parallel to the Rectangle
        /// </summary>
        /// <param name="theRectangle"></param>
        /// <param name="aAxis"></param>
        /// <returns></returns>
        private bool IsAxisCollision(Configuration theRectangle, Vector2 aAxis)
        {
            //Project the corners of the Rectangle we are checking on to the Axis and
            //get a scalar value of that project we can then use for comparison
            List<float> aRectangleAScalars = new List<float>();
            aRectangleAScalars.Add(GenerateScalar(theRectangle.UpperLeftCorner(), aAxis));
            aRectangleAScalars.Add(GenerateScalar(theRectangle.UpperRightCorner(), aAxis));
            aRectangleAScalars.Add(GenerateScalar(theRectangle.LowerLeftCorner(), aAxis));
            aRectangleAScalars.Add(GenerateScalar(theRectangle.LowerRightCorner(), aAxis));

            //Project the corners of the current Rectangle on to the Axis and
            //get a scalar value of that project we can then use for comparison
            List<float> aRectangleBScalars = new List<float>();
            aRectangleBScalars.Add(GenerateScalar(UpperLeftCorner(), aAxis));
            aRectangleBScalars.Add(GenerateScalar(UpperRightCorner(), aAxis));
            aRectangleBScalars.Add(GenerateScalar(LowerLeftCorner(), aAxis));
            aRectangleBScalars.Add(GenerateScalar(LowerRightCorner(), aAxis));

            //Get the Maximum and Minium Scalar values for each of the Rectangles
            float aRectangleAMinimum = aRectangleAScalars.Min();
            float aRectangleAMaximum = aRectangleAScalars.Max();
            float aRectangleBMinimum = aRectangleBScalars.Min();
            float aRectangleBMaximum = aRectangleBScalars.Max();

            //If we have overlaps between the Rectangles (i.e. Min of B is less than Max of A)
            //then we are detecting a collision between the rectangles on this Axis
            if (aRectangleBMinimum <= aRectangleAMaximum && aRectangleBMaximum >= aRectangleAMaximum)
            {
                return true;
            }
            else if (aRectangleAMinimum <= aRectangleBMaximum && aRectangleAMaximum >= aRectangleBMaximum)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Generates a scalar value that can be used to compare where corners of 
        /// a rectangle have been projected onto a particular axis. 
        /// </summary>
        /// <param name="theRectangleCorner"></param>
        /// <param name="theAxis"></param>
        /// <returns></returns>
        private float GenerateScalar(Vector2 theRectangleCorner, Vector2 theAxis)
        {
            //Using the formula for Vector projection. Take the corner being passed in
            //and project it onto the given Axis
            float aNumerator = (theRectangleCorner.X * theAxis.X) + (theRectangleCorner.Y * theAxis.Y);
            float aDenominator = (theAxis.X * theAxis.X) + (theAxis.Y * theAxis.Y);
            float aDivisionResult = aNumerator / aDenominator;
            Vector2 aCornerProjected = new Vector2(aDivisionResult * theAxis.X, aDivisionResult * theAxis.Y);
            
            //Now that we have our projected Vector, calculate a scalar of that projection
            //that can be used to more easily do comparisons
            float aScalar = (theAxis.X * aCornerProjected.X) + (theAxis.Y * aCornerProjected.Y);
            return aScalar;
        }

        /// <summary>
        /// Rotate a point from a given location and adjust using the Origin we
        /// are rotating around
        /// </summary>
        /// <param name="thePoint"></param>
        /// <param name="theOrigin"></param>
        /// <param name="theRotation"></param>
        /// <returns></returns>
        private Vector2 RotatePoint(Vector2 thePoint, Vector2 theOrigin, float theRotation)
        {
            Vector2 aTranslatedPoint = new Vector2();
            aTranslatedPoint.X = (float)(theOrigin.X + (thePoint.X - theOrigin.X) * Math.Cos(theRotation)
                - (thePoint.Y - theOrigin.Y) * Math.Sin(theRotation));
            aTranslatedPoint.Y = (float)(theOrigin.Y + (thePoint.Y - theOrigin.Y) * Math.Cos(theRotation)
                + (thePoint.X - theOrigin.X) * Math.Sin(theRotation));
            return aTranslatedPoint;
        }
                
        public Vector2 UpperLeftCorner()
        {
            Vector2 aUpperLeft = new Vector2(CollisionRectangle.Left, CollisionRectangle.Top);
            aUpperLeft = RotatePoint(aUpperLeft, aUpperLeft + Origin, rotation);
            return aUpperLeft;
        }

        public Vector2 UpperRightCorner()
        {
            Vector2 aUpperRight = new Vector2(CollisionRectangle.Right, CollisionRectangle.Top);
            aUpperRight = RotatePoint(aUpperRight, aUpperRight + new Vector2(-Origin.X, Origin.Y), rotation);
            return aUpperRight;
        }

        public Vector2 LowerLeftCorner()
        {
            Vector2 aLowerLeft = new Vector2(CollisionRectangle.Left, CollisionRectangle.Bottom);
            aLowerLeft = RotatePoint(aLowerLeft, aLowerLeft + new Vector2(Origin.X, -Origin.Y), rotation);
            return aLowerLeft;
        }

        public Vector2 LowerRightCorner()
        {
            Vector2 aLowerRight = new Vector2(CollisionRectangle.Right, CollisionRectangle.Bottom);
            aLowerRight = RotatePoint(aLowerRight, aLowerRight + new Vector2(-Origin.X, -Origin.Y), rotation);
            return aLowerRight;
        }

        public float X
        {
            get { return CollisionRectangle.position.X; }
        }

        public float Y
        {
            get { return CollisionRectangle.position.Y; }
        }

        public float Width
        {
            get { return CollisionRectangle.size.X; }
        }

        public float Height
        {
            get { return CollisionRectangle.size.Y; }
        }

        /// <summary>
        /// Bu metod kullanılıyor ise start ve goal arasında hiçbir engelin olmadığı doğrulanmalıdır.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="goal"></param>
        public bool isConnectable(Configuration goalConfig,List<Obstacle> obstacleList)
        {
            Configuration virtualConfig = new Configuration(this.X, this.Y, this.Width, this.Height, this.rotation);
            Vector2 direction;
            float distance, currentDistance, currentAngle, normalizeAngle;
            while (true)
            {
                direction = goalConfig.CollisionRectangle.position - virtualConfig.CollisionRectangle.position;
                direction.Normalize();
                if (float.IsNaN(direction.X))       // Burada bir bug var. İf ile çözüldü düzelt.
                    return false;
                currentDistance = Vector2.Distance(goalConfig.CollisionRectangle.position, virtualConfig.CollisionRectangle.position);
                currentAngle = goalConfig.rotation - virtualConfig.rotation;
                normalizeAngle = currentAngle / currentDistance;

                virtualConfig.ChangePosition(direction.X, direction.Y);
                virtualConfig.rotation += normalizeAngle;

                foreach (Obstacle obs in obstacleList)
                {
                    if (virtualConfig.Intersects(obs.config))
                        return false;
                }
                distance = Vector2.Distance(virtualConfig.CollisionRectangle.position, goalConfig.CollisionRectangle.position);
                if (distance < 1)
                {
                    break;
                }
            }
            return true;
        }

        /// <summary>
        /// Bu metod yapılandırmanın komşuları arasında en uzak olan komşu yapılandırmayı listeden kaldırır. Mesafe için sadece uzaklık değerine bakılır. Açı hesaba katılmaz.
        /// </summary>
        public void removeFarthestNeighborConfiguration()
        {
            float maxDistance = float.MinValue;
            float currentDistance;
            int currentIndex = -1;
            int maxDistanceIndex = -1;
            foreach (Configuration neighborConf in neighbors)
            {
                currentIndex++;
                currentDistance = Vector2.Distance(this.CollisionRectangle.position, neighborConf.CollisionRectangle.position);
                if (maxDistance < currentDistance )
                {
                    maxDistance = currentDistance;
                    maxDistanceIndex = currentIndex;
                }
            }
            neighbors.RemoveAt(maxDistanceIndex);
        }

        public int getFarthestNeighborConfIndex()
        {
            float maxDistance = float.MinValue;
            float currentDistance;
            int currentIndex = -1;
            int maxDistanceIndex = -1;
            foreach (Configuration neighborConf in neighbors)
            {
                currentIndex++;
                //currentDistance = Vector2.Distance(this.CollisionRectangle.position, neighborConf.CollisionRectangle.position);
                currentDistance = distance(neighborConf);
                if (maxDistance < currentDistance)
                {
                    maxDistance = currentDistance;
                    maxDistanceIndex = currentIndex;
                }
            }
            return maxDistanceIndex;
        }

        public bool isOnObstacle(List<Obstacle> obsList)
        {
            foreach (Obstacle obs in obsList)
            {
                if (obs.config.Intersects(this))
                {
                    return true;
                }
            }
            return false;
        }

        public float distance(Configuration to)
        {
            float xyDistance = Vector2.Distance(this.CollisionRectangle.position, to.CollisionRectangle.position);
            float rotationDistance = (float)Math.Pow((double)(to.rotation-this.rotation)%180,2);
            return (float)Math.Sqrt(Math.Pow(xyDistance,2)+Math.Pow(rotationDistance,2));
        }
    }
}