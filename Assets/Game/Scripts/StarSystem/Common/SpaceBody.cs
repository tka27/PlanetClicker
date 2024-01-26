using System.Collections.Generic;
using Game.Scripts.StarSystem.Planets;
using Object = UnityEngine.Object;

namespace Game.Scripts.StarSystem.Common
{
    public abstract class SpaceBody
    {
        protected readonly HashSet<SpaceBody> Satellites = new();

        protected IMotionData MotionData;
        protected SpaceBodyView View;
        protected SpaceBody Parent;

        internal protected int Depth;
        internal protected float Size;

        
        public float SatellitesCount => Satellites.Count;

        public void Update()
        {
            MotionData.Update(Parent.MotionData.Position);
            foreach (var satellite in Satellites)
            {
                satellite.Update();
            }
            View.UpdateTransform(MotionData);
        }

        protected void Init()
        {
            View.Init(MotionData);
            View.SpawnSatelliteRequest += CreateSatellite;
//size, eg...
        }

        private void CreateSatellite()
        {
            Satellites.Add(new Planet(this));
        }
    }
}