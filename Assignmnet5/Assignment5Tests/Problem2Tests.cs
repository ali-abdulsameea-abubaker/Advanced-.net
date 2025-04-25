using Problem2;
using System;
using Xunit;

namespace Assignment5Tests
{
    /// <summary>
    /// Unit tests for the Builder pattern implementation in Problem 2.
    /// </summary>
    public class Problem2Tests
    {
        /// <summary>
        /// Verifies that the builder constructs a complete computer with all components.
        /// </summary>
        [Fact]
        public void ComputerBuilder_BuildsCompleteComputer()
        {
            var builder = new ComputerBuilder();
            var computer = builder
                .SetHardDrive(1000, "SSD", 550, 520)
                .SetMotherboard(4, 150, 3, "ATX", 4)
                .SetCPU(4.7, "Intel", "LGA1200", 16, 8)
                .SetMemory(3200, 3200, "DDR4", 32)
                .SetGraphicsCard(3, 1800, 8, 2048)
                .SetCase(50, 25, 45, 4, 6, "ATX")
                .Build();

            Assert.NotNull(computer);
            Assert.Equal("Intel", computer.CPU.Manufacturer);
            Assert.Equal(32, computer.Memory.Amount);
            Assert.Equal(1000, computer.HardDrive.Capacity);
        }

        /// <summary>
        /// Verifies that the director constructs a gaming PC with expected specs.
        /// </summary>
        [Fact]
        public void ComputerDirector_BuildsGamingPC()
        {
            var builder = new ComputerBuilder();
            var director = new ComputerDirector(builder);
            var gamingPC = director.BuildGamingPC();

            Assert.NotNull(gamingPC);
            Assert.Equal(8, gamingPC.CPU.CoreCount);
            Assert.Equal("DDR4", gamingPC.Memory.Type);
            Assert.Equal(3, gamingPC.GraphicsCard.FanCount);
        }

        /// <summary>
        /// Ensures that creating a CPU with invalid speed throws an exception.
        /// </summary>
        [Fact]
        public void CPU_ThrowsException_WhenInvalidSpeed()
        {
            Assert.Throws<ArgumentException>(() => new CPU(0, "Intel", "LGA1200", 16, 8));
        }

        /// <summary>
        /// Ensures that creating Memory with an unsupported type throws an exception.
        /// </summary>
        [Fact]
        public void Memory_ThrowsException_WhenInvalidType()
        {
            Assert.Throws<ArgumentException>(() => new Memory(3200, 3200, "DDR5", 32));
        }
    }
}
