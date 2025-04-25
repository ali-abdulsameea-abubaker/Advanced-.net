/***
 *Report Summary (for Problems 1 and 2)
Problem 1: Children's Toys
Pattern Used: Factory Method Pattern
Reason for Choice: The Factory Method pattern was chosen because we need to create different types of toys that share a common interface. Each toy type has its own specific properties and methods, but they all inherit from the same base Toy class. The factory method allows us to encapsulate the creation logic for each toy type while maintaining flexibility to add new toy types in the future.

Challenges Faced:

Determining which properties should be in the base class vs. specific toy classes

Ensuring each toy class had at least 3 specific properties and 2 methods that were truly unique to that toy type

Designing the factory classes to produce properly initialized toy objects

References:

Design Patterns: Elements of Reusable Object-Oriented Software by Gamma et al.

https://www.dofactory.com/net/factory-method-design-pattern

https://refactoring.guru/design-patterns/factory-method

 * 
 */




namespace Problem1
{
    /// <summary>
    /// Abstract base class representing a general toy.
    /// </summary>
    public abstract class Toy
    {
        public decimal Cost { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string ManufacturingCompany { get; set; }
        public int YearOfManufacture { get; set; }
        public int MinimumAgeLimit { get; set; }
        public int MaximumAgeLimit { get; set; }
        public bool HasChokingHazard { get; set; }
        public double Weight { get; set; }

        /// <summary>Defines how the toy is played with.</summary>
        public abstract string Play();

        /// <summary>Defines how the toy is assembled.</summary>
        public abstract string Assemble();
    }

    /// <summary>
    /// Represents a toy car.
    /// </summary>
    public class Car : Toy
    {
        public int WheelCount { get; set; }
        public bool IsRemoteControlled { get; set; }
        public string Color { get; set; }

        public override string Play()
        {
            return $"Driving the {Color} car with {WheelCount} wheels!";
        }

        public override string Assemble()
        {
            return $"Attaching {WheelCount} wheels to the car body.";
        }
    }

    /// <summary>
    /// Represents a dollhouse toy.
    /// </summary>
    public class Dollhouse : Toy
    {
        public int RoomCount { get; set; }
        public bool HasFurniture { get; set; }
        public bool IsMultiStory { get; set; }

        public override string Play()
        {
            return $"Playing with the {(IsMultiStory ? "multi-story" : "single-story")} dollhouse with {RoomCount} rooms.";
        }

        public override string Assemble()
        {
            return $"Building the dollhouse with {RoomCount} rooms and {(HasFurniture ? "with" : "without")} furniture.";
        }
    }

    /// <summary>
    /// Abstract factory class for creating toys.
    /// </summary>
    public abstract class ToyFactory
    {
        /// <summary>Creates a toy instance.</summary>
        public abstract Toy CreateToy();
    }

    /// <summary>
    /// Factory class for creating Car toys.
    /// </summary>
    public class CarFactory : ToyFactory
    {
        public override Toy CreateToy()
        {
            return new Car
            {
                Name = "Race Car",
                Description = "A fast racing car toy",
                ManufacturingCompany = "ToyMakers Inc.",
                YearOfManufacture = 2023,
                MinimumAgeLimit = 3,
                MaximumAgeLimit = 10,
                HasChokingHazard = true,
                Weight = 0.5,
                Cost = 19.99m,
                WheelCount = 4,
                IsRemoteControlled = false,
                Color = "Red"
            };
        }
    }

    /// <summary>
    /// Factory class for creating Dollhouse toys.
    /// </summary>
    public class DollhouseFactory : ToyFactory
    {
        public override Toy CreateToy()
        {
            return new Dollhouse
            {
                Name = "Dream House",
                Description = "A beautiful dollhouse for creative play",
                ManufacturingCompany = "DreamToys LLC",
                YearOfManufacture = 2023,
                MinimumAgeLimit = 4,
                MaximumAgeLimit = 12,
                HasChokingHazard = true,
                Weight = 3.2,
                Cost = 49.99m,
                RoomCount = 6,
                HasFurniture = true,
                IsMultiStory = true
            };
        }
    }
}
