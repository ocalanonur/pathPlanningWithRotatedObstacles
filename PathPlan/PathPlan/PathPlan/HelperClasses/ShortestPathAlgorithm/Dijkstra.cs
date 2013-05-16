using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PathPlan.RoadmapNS;
using Microsoft.Xna.Framework;

namespace PathPlan.HelperClasses.ShortestPathAlgorithm
{
    public class Dijkstra
    {
        private Roadmap roadmap;
        public Configuration startConfig;
        public Configuration goalConfig;
        public Queue<Configuration> stations = new Queue<Configuration>();

        public Dijkstra(Roadmap roadmap)
        {
            this.roadmap = roadmap;
        }
        public Dijkstra(Roadmap roadmap,Configuration startConfig,Configuration goalConfig)
        {
            this.roadmap = roadmap;
            this.startConfig = startConfig;
            this.goalConfig = goalConfig;
            addStartAndGoalConfigtoConnectedRoadmapList();
        }

        public void addStartAndGoalConfigtoConnectedRoadmapList()
        {
            int startIndex = -1; float startLen = float.MaxValue;
            int goalIndex = -1; float goalLen = float.MaxValue;

            float tempStartLen,tempGoalLen;
            for (int i = 0; i < roadmap.samples.Count; i++)
            {
                tempStartLen = Vector2.Distance(roadmap.samples[i].CollisionRectangle.position,startConfig.CollisionRectangle.position);
                tempGoalLen = Vector2.Distance(roadmap.samples[i].CollisionRectangle.position, goalConfig.CollisionRectangle.position);
                if (startLen > tempStartLen)
                {
                    startIndex = i;
                    startLen = tempStartLen;
                }
                if (goalLen > tempGoalLen)
                {
                    goalIndex = i;
                    goalLen = tempGoalLen;
                }
            }

            roadmap.samples.Add(startConfig);
            roadmap.samples[roadmap.samples.Count - 1].neighbors.Add(roadmap.samples[startIndex]);
            roadmap.samples.Add(goalConfig);
            roadmap.samples[roadmap.samples.Count - 1].neighbors.Add(roadmap.samples[goalIndex]);
        }

        public void getShortestPath()
        {
            List<Configuration> dijikstraSortedLocations = new List<Configuration>();

            Configuration configuration = roadmap.samples[roadmap.samples.Count - 1];
            configuration.dijkstraDistance = 0;
            configuration.dijkstraParent = null;
            dijikstraSortedLocations.Add(configuration);
            roadmap.samples[roadmap.samples.Count - 2].dijkstraParent = roadmap.samples[roadmap.samples.Count - 2].neighbors[0];

            while (dijikstraSortedLocations.Count > 0)
            {
                dijikstraSortedLocations.Sort(
                    delegate(Configuration c1, Configuration c2)
                    {
                        return c1.dijkstraDistance.CompareTo(c2.dijkstraDistance);
                    }
                );
                Configuration currentConfiguration = dijikstraSortedLocations[0];
                foreach (Configuration c in currentConfiguration.neighbors)
                {
                    float len = Vector2.Distance(c.CollisionRectangle.position, currentConfiguration.CollisionRectangle.position);
                    if ((!c.dijkstraCompleted) && (currentConfiguration.dijkstraDistance + len < c.dijkstraDistance))
                    {
                        c.dijkstraDistance = currentConfiguration.dijkstraDistance + len;
                        c.dijkstraParent = currentConfiguration;
                        dijikstraSortedLocations.Add(c);
                    }
                }
                currentConfiguration.dijkstraCompleted = true;
                dijikstraSortedLocations.RemoveAt(0);
            }

            while (roadmap.samples[roadmap.samples.Count-2].dijkstraParent != null)
            {
                //agent.stations.Enqueue(agent.position.dijkstraParent);
                stations.Enqueue(new Configuration(roadmap.samples[roadmap.samples.Count-2].dijkstraParent.CollisionRectangle,roadmap.samples[roadmap.samples.Count-2].dijkstraParent.Rotation,"dijkstra"));
                roadmap.samples[roadmap.samples.Count-2].dijkstraParent = roadmap.samples[roadmap.samples.Count-2].dijkstraParent.dijkstraParent;
            }
        }
        public void clear()
        {
            startConfig = null;
            goalConfig = null;
            stations.Clear();
        }
    }
}
