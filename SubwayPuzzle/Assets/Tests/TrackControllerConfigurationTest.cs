using UnityEngine;
using NUnit.Framework;

namespace Tests
{
    public class TrackControllerConfigurationTest
    {
        [Test]
        public void TrainPathOrientationDependsOnDirections()
        {
            var trainPath = new PointPath(new[]
            {
                new Vector3(0, 0),
                new Vector3(1, 1)
            });

            var dir1 = XZDirection.PositiveX;
            var dir2 = XZDirection.NegativeX;

            var config = new TrackControllerConfiguration()
            {
                direction1 = dir1,
                direction2 = dir2,
                serializedTrainPath = SerializedPointPath.From(trainPath),
            };

            config.Deserialize();


			if (XZDirection.PositiveX.ToCardinalDirection()
				== config.InitialTrackPiece.Directions.Item1)
            {
                Assert.AreEqual(trainPath, config.TrainPath);
            } else
            {
                Assert.AreEqual(trainPath.Reversed, config.TrainPath);
            }
		}
    }

}