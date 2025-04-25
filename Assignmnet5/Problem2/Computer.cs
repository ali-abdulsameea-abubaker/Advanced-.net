
/*
 * Problem 2: Computer Builder
Pattern Used: Builder Pattern
Reason for Choice: The Builder pattern was chosen because constructing a computer involves many components with complex configurations. The Builder pattern allows us to separate the construction of a computer from its representation, making the code more maintainable and readable. It also provides a clear API for constructing computers step by step and allows for different configurations (like gaming PC vs office PC) to be easily created.

Challenges Faced:

Implementing proper validation for all computer components

Designing a fluent interface for the builder that would be intuitive to use

Deciding which components should be mandatory in the Build() method

Handling the different data types for various properties (double for speeds, int for counts, etc.)

References:

Design Patterns: Elements of Reusable Object-Oriented Software by Gamma et al.

https://www.dofactory.com/net/builder-design-pattern

https://refactoring.guru/design-patterns/builder

https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/integral-types-table
 * **/




namespace Problem2
{
    /// <summary>
    /// Represents a complete computer system with all its components
    /// 
    /// Problem2/Computer.cs
    /// 
    /// </summary>
    public class Computer
    {
        /// <summary>
        /// The computer's hard drive
        /// </summary>
        public HardDrive HardDrive { get; set; }

        /// <summary>
        /// The computer's motherboard
        /// </summary>
        public Motherboard Motherboard { get; set; }

        /// <summary>
        /// The computer's central processing unit
        /// </summary>
        public CPU CPU { get; set; }

        /// <summary>
        /// The computer's memory (RAM)
        /// </summary>
        public Memory Memory { get; set; }

        /// <summary>
        /// The computer's graphics card (GPU)
        /// </summary>
        public GraphicsCard GraphicsCard { get; set; }

        /// <summary>
        /// The computer's physical case
        /// </summary>
        public Case Case { get; set; }

        /// <summary>
        /// Returns a string representation of the computer's specifications
        /// </summary>
        /// <returns>Formatted string with computer specifications</returns>
        public override string ToString()
        {
            return $"Computer with:\n" +
                   $"- {CPU?.Manufacturer} {CPU?.Speed}GHz CPU\n" +
                   $"- {Memory?.Amount}GB {Memory?.Type} Memory\n" +
                   $"- {HardDrive?.Capacity}GB {HardDrive?.Type} Hard Drive\n" +
                   $"- {GraphicsCard?.VideoMemory}GB Graphics Card\n" +
                   $"- {Case?.FormFactor} Case";
        }
    }

    /// <summary>
    /// Represents a computer hard drive
    /// </summary>
    public class HardDrive
    {
        /// <summary>
        /// Storage capacity in gigabytes
        /// </summary>
        public int Capacity { get; set; }

        /// <summary>
        /// Type of hard drive (SSD or HDD)
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Read speed in megabytes per second
        /// </summary>
        public double ReadSpeed { get; set; }

        /// <summary>
        /// Write speed in megabytes per second
        /// </summary>
        public double WriteSpeed { get; set; }

        /// <summary>
        /// Creates a new hard drive instance
        /// </summary>
        /// <param name="capacity">Storage capacity in GB (must be positive)</param>
        /// <param name="type">Type of drive (must be SSD or HDD)</param>
        /// <param name="readSpeed">Read speed in MB/s (must be positive)</param>
        /// <param name="writeSpeed">Write speed in MB/s (must be positive)</param>
        /// <exception cref="ArgumentException">Thrown when invalid parameters are provided</exception>
        public HardDrive(int capacity, string type, double readSpeed, double writeSpeed)
        {
            if (capacity <= 0) throw new ArgumentException("Capacity must be positive");
            if (type != "SSD" && type != "HDD") throw new ArgumentException("Type must be SSD or HDD");
            if (readSpeed <= 0 || writeSpeed <= 0) throw new ArgumentException("Speeds must be positive");

            Capacity = capacity;
            Type = type;
            ReadSpeed = readSpeed;
            WriteSpeed = writeSpeed;
        }
    }

    /// <summary>
    /// Represents a computer motherboard
    /// </summary>
    public class Motherboard
    {
        /// <summary>
        /// Number of memory slots available
        /// </summary>
        public int MemorySlots { get; set; }

        /// <summary>
        /// Power consumption in watts
        /// </summary>
        public double PowerConsumption { get; set; }

        /// <summary>
        /// Number of PCI expansion slots
        /// </summary>
        public int PCISlots { get; set; }

        /// <summary>
        /// Physical form factor (e.g., ATX, MicroATX)
        /// </summary>
        public string FormFactor { get; set; }

        /// <summary>
        /// Maximum number of supported hard drives
        /// </summary>
        public int HardDriveLimit { get; set; }

        /// <summary>
        /// The installed CPU
        /// </summary>
        public CPU CPU { get; set; }

        /// <summary>
        /// The installed memory
        /// </summary>
        public Memory Memory { get; set; }

        /// <summary>
        /// The installed graphics card
        /// </summary>
        public GraphicsCard GraphicsCard { get; set; }

        /// <summary>
        /// Creates a new motherboard instance
        /// </summary>
        /// <param name="memorySlots">Number of memory slots (must be positive)</param>
        /// <param name="powerConsumption">Power consumption in watts (must be positive)</param>
        /// <param name="pciSlots">Number of PCI slots (cannot be negative)</param>
        /// <param name="formFactor">Form factor designation (required)</param>
        /// <param name="hardDriveLimit">Maximum hard drives supported (must be positive)</param>
        /// <exception cref="ArgumentException">Thrown when invalid parameters are provided</exception>
        public Motherboard(int memorySlots, double powerConsumption, int pciSlots,
                          string formFactor, int hardDriveLimit)
        {
            if (memorySlots <= 0) throw new ArgumentException("Memory slots must be positive");
            if (powerConsumption <= 0) throw new ArgumentException("Power consumption must be positive");
            if (pciSlots < 0) throw new ArgumentException("PCI slots cannot be negative");
            if (string.IsNullOrEmpty(formFactor)) throw new ArgumentException("Form factor is required");
            if (hardDriveLimit <= 0) throw new ArgumentException("Hard drive limit must be positive");

            MemorySlots = memorySlots;
            PowerConsumption = powerConsumption;
            PCISlots = pciSlots;
            FormFactor = formFactor;
            HardDriveLimit = hardDriveLimit;
        }
    }

    /// <summary>
    /// Represents a computer central processing unit (CPU)
    /// </summary>
    public class CPU
    {
        /// <summary>
        /// Clock speed in gigahertz
        /// </summary>
        public double Speed { get; set; }

        /// <summary>
        /// Manufacturer name (e.g., Intel, AMD)
        /// </summary>
        public string Manufacturer { get; set; }

        /// <summary>
        /// Socket type for physical installation
        /// </summary>
        public string SocketType { get; set; }

        /// <summary>
        /// Cache size in megabytes
        /// </summary>
        public int CacheSize { get; set; }

        /// <summary>
        /// Number of processing cores
        /// </summary>
        public int CoreCount { get; set; }

        /// <summary>
        /// Creates a new CPU instance
        /// </summary>
        /// <param name="speed">Clock speed in GHz (must be positive)</param>
        /// <param name="manufacturer">Manufacturer name (required)</param>
        /// <param name="socketType">Socket type (required)</param>
        /// <param name="cacheSize">Cache size in MB (must be positive)</param>
        /// <param name="coreCount">Number of cores (must be positive)</param>
        /// <exception cref="ArgumentException">Thrown when invalid parameters are provided</exception>
        public CPU(double speed, string manufacturer, string socketType, int cacheSize, int coreCount)
        {
            if (speed <= 0) throw new ArgumentException("Speed must be positive");
            if (string.IsNullOrEmpty(manufacturer)) throw new ArgumentException("Manufacturer is required");
            if (string.IsNullOrEmpty(socketType)) throw new ArgumentException("Socket type is required");
            if (cacheSize <= 0) throw new ArgumentException("Cache size must be positive");
            if (coreCount <= 0) throw new ArgumentException("Core count must be positive");

            Speed = speed;
            Manufacturer = manufacturer;
            SocketType = socketType;
            CacheSize = cacheSize;
            CoreCount = coreCount;
        }
    }

    /// <summary>
    /// Represents computer memory (RAM)
    /// </summary>
    public class Memory
    {
        /// <summary>
        /// Read speed in megahertz
        /// </summary>
        public double ReadSpeed { get; set; }

        /// <summary>
        /// Write speed in megahertz
        /// </summary>
        public double WriteSpeed { get; set; }

        /// <summary>
        /// Memory type (DDR1-DDR4)
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Total memory amount in gigabytes
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// Creates a new memory instance
        /// </summary>
        /// <param name="readSpeed">Read speed in MHz (must be positive)</param>
        /// <param name="writeSpeed">Write speed in MHz (must be positive)</param>
        /// <param name="type">Memory type (must be DDR1-DDR4)</param>
        /// <param name="amount">Total memory in GB (must be positive)</param>
        /// <exception cref="ArgumentException">Thrown when invalid parameters are provided</exception>
        public Memory(double readSpeed, double writeSpeed, string type, int amount)
        {
            if (readSpeed <= 0 || writeSpeed <= 0) throw new ArgumentException("Speeds must be positive");
            if (!new[] { "DDR1", "DDR2", "DDR3", "DDR4" }.Contains(type))
                throw new ArgumentException("Invalid memory type");
            if (amount <= 0) throw new ArgumentException("Amount must be positive");

            ReadSpeed = readSpeed;
            WriteSpeed = writeSpeed;
            Type = type;
            Amount = amount;
        }
    }

    /// <summary>
    /// Represents a graphics processing unit (GPU)
    /// </summary>
    public class GraphicsCard
    {
        /// <summary>
        /// Number of cooling fans
        /// </summary>
        public int FanCount { get; set; }

        /// <summary>
        /// Clock speed in megahertz
        /// </summary>
        public double Speed { get; set; }

        /// <summary>
        /// Video memory in gigabytes
        /// </summary>
        public int VideoMemory { get; set; }

        /// <summary>
        /// Number of CUDA cores (NVIDIA) or stream processors (AMD)
        /// </summary>
        public int CUDACores { get; set; }

        /// <summary>
        /// Creates a new graphics card instance
        /// </summary>
        /// <param name="fanCount">Number of fans (cannot be negative)</param>
        /// <param name="speed">Clock speed in MHz (must be positive)</param>
        /// <param name="videoMemory">Video memory in GB (must be positive)</param>
        /// <param name="cudaCores">Number of CUDA cores (cannot be negative)</param>
        /// <exception cref="ArgumentException">Thrown when invalid parameters are provided</exception>
        public GraphicsCard(int fanCount, double speed, int videoMemory, int cudaCores)
        {
            if (fanCount < 0) throw new ArgumentException("Fan count cannot be negative");
            if (speed <= 0) throw new ArgumentException("Speed must be positive");
            if (videoMemory <= 0) throw new ArgumentException("Video memory must be positive");
            if (cudaCores < 0) throw new ArgumentException("CUDA cores cannot be negative");

            FanCount = fanCount;
            Speed = speed;
            VideoMemory = videoMemory;
            CUDACores = cudaCores;
        }
    }

    /// <summary>
    /// Represents a computer case/chassis
    /// </summary>
    public class Case
    {
        /// <summary>
        /// Length in centimeters
        /// </summary>
        public double Length { get; set; }

        /// <summary>
        /// Width in centimeters
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// Height in centimeters
        /// </summary>
        public double Height { get; set; }

        /// <summary>
        /// Number of included fans
        /// </summary>
        public int FanCount { get; set; }

        /// <summary>
        /// Number of ventilation openings
        /// </summary>
        public int VentCount { get; set; }

        /// <summary>
        /// Form factor compatibility
        /// </summary>
        public string FormFactor { get; set; }

        /// <summary>
        /// Creates a new computer case instance
        /// </summary>
        /// <param name="length">Length in cm (must be positive)</param>
        /// <param name="width">Width in cm (must be positive)</param>
        /// <param name="height">Height in cm (must be positive)</param>
        /// <param name="fanCount">Number of fans (cannot be negative)</param>
        /// <param name="ventCount">Number of vents (cannot be negative)</param>
        /// <param name="formFactor">Form factor designation (required)</param>
        /// <exception cref="ArgumentException">Thrown when invalid parameters are provided</exception>
        public Case(double length, double width, double height, int fanCount, int ventCount, string formFactor)
        {
            if (length <= 0 || width <= 0 || height <= 0)
                throw new ArgumentException("Dimensions must be positive");
            if (fanCount < 0 || ventCount < 0)
                throw new ArgumentException("Counts cannot be negative");
            if (string.IsNullOrEmpty(formFactor))
                throw new ArgumentException("Form factor is required");

            Length = length;
            Width = width;
            Height = height;
            FanCount = fanCount;
            VentCount = ventCount;
            FormFactor = formFactor;
        }
    }
}

// Problem2/IComputerBuilder.cs
namespace Problem2
{
    /// <summary>
    /// Interface for building computer systems step by step
    /// </summary>
    public interface IComputerBuilder
    {
        /// <summary>
        /// Sets the hard drive configuration
        /// </summary>
        IComputerBuilder SetHardDrive(int capacity, string type, double readSpeed, double writeSpeed);

        /// <summary>
        /// Sets the motherboard configuration
        /// </summary>
        IComputerBuilder SetMotherboard(int memorySlots, double powerConsumption, int pciSlots,
                                      string formFactor, int hardDriveLimit);

        /// <summary>
        /// Sets the CPU configuration
        /// </summary>
        IComputerBuilder SetCPU(double speed, string manufacturer, string socketType,
                              int cacheSize, int coreCount);

        /// <summary>
        /// Sets the memory configuration
        /// </summary>
        IComputerBuilder SetMemory(double readSpeed, double writeSpeed, string type, int amount);

        /// <summary>
        /// Sets the graphics card configuration
        /// </summary>
        IComputerBuilder SetGraphicsCard(int fanCount, double speed, int videoMemory, int cudaCores);

        /// <summary>
        /// Sets the computer case configuration
        /// </summary>
        IComputerBuilder SetCase(double length, double width, double height,
                                int fanCount, int ventCount, string formFactor);

        /// <summary>
        /// Builds and returns the completed computer
        /// </summary>
        Computer Build();
    }
}

// Problem2/ComputerBuilder.cs
namespace Problem2
{
    /// <summary>
    /// Concrete implementation of a computer builder
    /// </summary>
    public class ComputerBuilder : IComputerBuilder
    {
        private Computer _computer = new Computer();

        /// <summary>
        /// Sets the hard drive configuration
        /// </summary>
        public IComputerBuilder SetHardDrive(int capacity, string type, double readSpeed, double writeSpeed)
        {
            _computer.HardDrive = new HardDrive(capacity, type, readSpeed, writeSpeed);
            return this;
        }

        /// <summary>
        /// Sets the motherboard configuration
        /// </summary>
        public IComputerBuilder SetMotherboard(int memorySlots, double powerConsumption, int pciSlots,
                                             string formFactor, int hardDriveLimit)
        {
            _computer.Motherboard = new Motherboard(memorySlots, powerConsumption, pciSlots,
                                                  formFactor, hardDriveLimit);
            return this;
        }

        /// <summary>
        /// Sets the CPU configuration
        /// </summary>
        public IComputerBuilder SetCPU(double speed, string manufacturer, string socketType,
                                     int cacheSize, int coreCount)
        {
            _computer.CPU = new CPU(speed, manufacturer, socketType, cacheSize, coreCount);
            return this;
        }

        /// <summary>
        /// Sets the memory configuration
        /// </summary>
        public IComputerBuilder SetMemory(double readSpeed, double writeSpeed, string type, int amount)
        {
            _computer.Memory = new Memory(readSpeed, writeSpeed, type, amount);
            return this;
        }

        /// <summary>
        /// Sets the graphics card configuration
        /// </summary>
        public IComputerBuilder SetGraphicsCard(int fanCount, double speed, int videoMemory, int cudaCores)
        {
            _computer.GraphicsCard = new GraphicsCard(fanCount, speed, videoMemory, cudaCores);
            return this;
        }

        /// <summary>
        /// Sets the computer case configuration
        /// </summary>
        public IComputerBuilder SetCase(double length, double width, double height,
                                      int fanCount, int ventCount, string formFactor)
        {
            _computer.Case = new Case(length, width, height, fanCount, ventCount, formFactor);
            return this;
        }

        /// <summary>
        /// Builds and validates the computer configuration
        /// </summary>
        /// <returns>The completed computer</returns>
        /// <exception cref="InvalidOperationException">Thrown when required components are missing</exception>
        public Computer Build()
        {
            // Validate that all required components are set
            if (_computer.HardDrive == null) throw new InvalidOperationException("Hard Drive not set");
            if (_computer.Motherboard == null) throw new InvalidOperationException("Motherboard not set");
            if (_computer.CPU == null) throw new InvalidOperationException("CPU not set");
            if (_computer.Memory == null) throw new InvalidOperationException("Memory not set");
            if (_computer.Case == null) throw new InvalidOperationException("Case not set");

            return _computer;
        }
    }
}

// Problem2/ComputerDirector.cs
namespace Problem2
{
    /// <summary>
    /// Director class that manages the construction of predefined computer configurations
    /// </summary>
    public class ComputerDirector
    {
        private IComputerBuilder _builder;

        /// <summary>
        /// Creates a new computer director with the specified builder
        /// </summary>
        /// <param name="builder">The builder to use for construction</param>
        public ComputerDirector(IComputerBuilder builder)
        {
            _builder = builder;
        }

        /// <summary>
        /// Builds a high-performance gaming PC configuration
        /// </summary>
        /// <returns>The completed gaming PC</returns>
        public Computer BuildGamingPC()
        {
            return _builder
                .SetHardDrive(1000, "SSD", 550, 520)
                .SetMotherboard(4, 150, 3, "ATX", 4)
                .SetCPU(4.7, "Intel", "LGA1200", 16, 8)
                .SetMemory(3200, 3200, "DDR4", 32)
                .SetGraphicsCard(3, 1800, 8, 2048)
                .SetCase(50, 25, 45, 4, 6, "ATX")
                .Build();
        }

        /// <summary>
        /// Builds a standard office PC configuration
        /// </summary>
        /// <returns>The completed office PC</returns>
        public Computer BuildOfficePC()
        {
            return _builder
                .SetHardDrive(500, "SSD", 500, 450)
                .SetMotherboard(2, 65, 1, "MicroATX", 2)
                .SetCPU(3.2, "AMD", "AM4", 8, 4)
                .SetMemory(2400, 2400, "DDR4", 8)
                .SetCase(40, 20, 35, 1, 2, "MicroATX")
                .Build();
        }
    }
}