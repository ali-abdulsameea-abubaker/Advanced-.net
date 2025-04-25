using Problem1;
using Xunit;

namespace Assignment5Tests
{
    /// <summary>
    /// Unit tests for the Problem1 factory method implementation.
    /// </summary>
    public class Problem1Tests
    {
        /// <summary>
        /// Ensures that CarFactory creates a Car with expected default values.
        /// </summary>
        [Fact]
        public void CarFactory_CreatesCarWithCorrectProperties()
        {
            var factory = new CarFactory();
            var car = factory.CreateToy() as Car;

            Assert.NotNull(car);
            Assert.Equal(4, car.WheelCount);
            Assert.Equal("Red", car.Color);
            Assert.Equal(19.99m, car.Cost);
        }

        /// <summary>
        /// Checks that the Car.Play method returns the correct string.
        /// </summary>
        [Fact]
        public void Car_Play_ReturnsCorrectMessage()
        {
            var car = new Car { Color = "Blue", WheelCount = 4 };
            var result = car.Play();

            Assert.Equal("Driving the Blue car with 4 wheels!", result);
        }

        /// <summary>
        /// Ensures that DollhouseFactory creates a Dollhouse with expected default values.
        /// </summary>
        [Fact]
        public void DollhouseFactory_CreatesDollhouseWithCorrectProperties()
        {
            var factory = new DollhouseFactory();
            var dollhouse = factory.CreateToy() as Dollhouse;

            Assert.NotNull(dollhouse);
            Assert.Equal(6, dollhouse.RoomCount);
            Assert.True(dollhouse.IsMultiStory);
            Assert.Equal(49.99m, dollhouse.Cost);
        }

        /// <summary>
        /// Checks that the Dollhouse.Assemble method returns the correct string.
        /// </summary>
        [Fact]
        public void Dollhouse_Assemble_ReturnsCorrectMessage()
        {
            var dollhouse = new Dollhouse { RoomCount = 3, HasFurniture = false };
            var result = dollhouse.Assemble();

            Assert.Equal("Building the dollhouse with 3 rooms and without furniture.", result);
        }
    }
}
