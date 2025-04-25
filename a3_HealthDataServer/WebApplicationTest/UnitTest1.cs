using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Controllers;
using WebApplication1.Data;
using WebApplication1.model;
/// <summary>
///  <Authorship>I, Ali Abdulsameea, 000857347 certify that this material is my original work.
/// No other person's work has been used without due acknowledgement.</Authorship>
/// This is unit test for testing out health data context from controller
/// <author>Ali Abubaker - 000857347</author>
/// </summary>
namespace WebApplicationTest
{
    public class UnitTest1
    {
        private readonly HealthDataContext _context;

        public UnitTest1()
        {
            var options = new DbContextOptionsBuilder<HealthDataContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new HealthDataContext(options);
        }
        /// <summary>
        /// Helper method to seed the database with test data
        /// </summary>
        private void SeedDatabase()
        {
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
            var immunizations = new List<Immunization>
            {
                new Immunization
                {
                    Id = Guid.NewGuid(),
                    CreationTime = DateTimeOffset.UtcNow,
                    OfficialName = "Vaccine A",
                    LotNumber = "Lot123",
                    ExpirationDate = DateTimeOffset.UtcNow.AddYears(1)
                },
                new Immunization
                {
                    Id = Guid.NewGuid(),
                    CreationTime = DateTimeOffset.UtcNow,
                    OfficialName = "Vaccine B",
                    LotNumber = "Lot456",
                    ExpirationDate = DateTimeOffset.UtcNow.AddYears(1)
                }
            };
            var patients = new List<Patient>
            {
                new Patient
                {
                    Id = Guid.NewGuid(),
                    CreationTime = DateTimeOffset.UtcNow,
                    FirstName = "John",
                    LastName = "Doe",
                    DateOfBirth = DateTimeOffset.UtcNow.AddYears(-30)
                },
                new Patient
                {
                    Id = Guid.NewGuid(),
                    CreationTime = DateTimeOffset.UtcNow,
                    FirstName = "Jane",
                    LastName = "Doe",
                    DateOfBirth = DateTimeOffset.UtcNow.AddYears(-25)
                }
            };
            var providers = new List<Provider>
            {
                new Provider
                {
                    Id = Guid.NewGuid(),
                    CreationTime = DateTimeOffset.UtcNow,
                    FirstName = "Dr. Alice",
                    LastName = "Smith",
                    LicenseNumber = 12345,
                    Address = "123 Main St"
                },
                new Provider
                {
                    Id = Guid.NewGuid(),
                    CreationTime = DateTimeOffset.UtcNow,
                    FirstName = "Dr. Bob",
                    LastName = "Johnson",
                    LicenseNumber = 67890,
                    Address = "456 Elm St"
                }
            };
            var organizations = new List<Organization>
            {
                new Organization
                {
                    Id = Guid.NewGuid(),
                    UpdatedTime = DateTimeOffset.UtcNow,
                    Name = "General Hospital",
                    Type = "Hospital",
                    Address = "789 Oak St"
                },
                new Organization
                {
                    Id = Guid.NewGuid(),
                    UpdatedTime = DateTimeOffset.UtcNow,
                    Name = "City Clinic",
                    Type = "Clinic",
                    Address = "101 Pine St"
                }
            };
            _context.Immunization.AddRange(immunizations);
            _context.Patient.AddRange(patients);
            _context.Provider.AddRange(providers);
            _context.Organization.AddRange(organizations);
            _context.SaveChanges();
        }
        /// <summary>
        /// ImmunizationsController Tests
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetImmunizations_ReturnsAllImmunizations()
        {
            // Arrange
            SeedDatabase();
            var controller = new ImmunizationsController(_context);

            // Act
            var result = await controller.GetImmunizations();

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnImmunizations = Assert.IsType<ImmunizationList>(actionResult.Value);
            Assert.Equal(2, returnImmunizations.Immunizations.Count);
        }
        /// <summary>
        /// Returns Immunization By Id testing
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetImmunization_ReturnsImmunizationById()
        {
            // Arrange
            SeedDatabase(); 
            var controller = new ImmunizationsController(_context);
            var immunization = _context.Immunization.First(); 
            // Act
            var result = await controller.GetImmunization(immunization.Id);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result.Result); 
            var returnImmunization = Assert.IsType<Immunization>(actionResult.Value);
            Assert.Equal(immunization.Id, returnImmunization.Id); 
        }
        /// <summary>
        /// Add New Immunization Testing
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task PostImmunization_AddsNewImmunization()
        {
            // Arrange
            var controller = new ImmunizationsController(_context);
            var newImmunization = new Immunization
            {
                OfficialName = "Vaccine C",
                LotNumber = "Lot789",
                ExpirationDate = DateTimeOffset.UtcNow.AddYears(1)
            };
            // Act
            var result = await controller.PostImmunization(newImmunization);
            // Assert
            var actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnImmunization = Assert.IsType<Immunization>(actionResult.Value);
            Assert.Equal(newImmunization.OfficialName, returnImmunization.OfficialName);
        }
        /// <summary>
        /// Deletes Immunization testing
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task DeleteImmunization_DeletesImmunization()
        {
            // Arrange
            SeedDatabase();
            var controller = new ImmunizationsController(_context);
            var immunization = _context.Immunization.First();
            // Act
            var result = await controller.DeleteImmunization(immunization.Id);
            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.Null(_context.Immunization.Find(immunization.Id));
        }
        /// <summary>
        /// PatientsController Tests
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetPatients_ReturnsAllPatients()
        {
            // Arrange
            SeedDatabase(); 
            var controller = new PatientsController(_context);
            // Act
            var result = await controller.GetPatients();
            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnPatients = Assert.IsType<PatientList>(actionResult.Value); 
            Assert.Equal(2, returnPatients.Patients.Count); 
        }
        /// <summary>
        /// test GetPatient_ReturnsPatientById
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetPatient_ReturnsPatientById()
        {
            // Arrange
            SeedDatabase();
            var controller = new PatientsController(_context);
            var patient = _context.Patient.First(); 
            // Act
            var result = await controller.GetPatient(patient.Id);
            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result.Result); 
            var returnPatient = Assert.IsType<Patient>(actionResult.Value); 
            Assert.Equal(patient.Id, returnPatient.Id); 
        }
        /// <summary>
        /// testing PostPatient_AddsNewPatient
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task PostPatient_AddsNewPatient()
        {
            // Arrange
            var controller = new PatientsController(_context);
            var newPatient = new Patient
            {
                FirstName = "Alice",
                LastName = "Johnson",
                DateOfBirth = DateTimeOffset.UtcNow.AddYears(-20)
            };
            // Act
            var result = await controller.PostPatient(newPatient);
            // Assert
            var actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnPatient = Assert.IsType<Patient>(actionResult.Value);
            Assert.Equal(newPatient.FirstName, returnPatient.FirstName);
        }
        /// <summary>
        /// Delete Patient 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task DeletePatient_DeletesPatient()
        {
            // Arrange
            SeedDatabase();
            var controller = new PatientsController(_context);
            var patient = _context.Patient.First();
            // Act
            var result = await controller.DeletePatient(patient.Id);
            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.Null(_context.Patient.Find(patient.Id));
        }
        /// <summary>
        /// GetProviders_ReturnsAllProviders
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetProviders_ReturnsAllProviders()
        {
            // Arrange
            SeedDatabase(); 
            var controller = new ProvidersController(_context);
            // Act
            var result = await controller.GetProviders();
            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnProviders = Assert.IsType<ProviderList>(actionResult.Value); 
            Assert.Equal(2, returnProviders.Providers.Count);
        }
        /// <summary>
        /// GetProvider_ReturnsProviderById
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetProvider_ReturnsProviderById()
        {
            // Arrange
            SeedDatabase();
            var controller = new ProvidersController(_context);
            var provider = _context.Provider.First();
            // Act
            var result = await controller.GetProvider(provider.Id);
            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnProvider = Assert.IsType<Provider>(actionResult.Value);
            Assert.Equal(provider.Id, returnProvider.Id); 
        }
        /// <summary>
        /// PostProvider_AddsNewProvider
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task PostProvider_AddsNewProvider()
        {
            // Arrange
            var controller = new ProvidersController(_context);
            var newProvider = new Provider
            {
                FirstName = "Dr. Carol",
                LastName = "Williams",
                LicenseNumber = 54321,
                Address = "789 Maple St"
            };
            // Act
            var result = await controller.PostProvider(newProvider);
            // Assert
            var actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnProvider = Assert.IsType<Provider>(actionResult.Value);
            Assert.Equal(newProvider.FirstName, returnProvider.FirstName);
        }
        /// <summary>
        /// DeleteProvider_DeletesProvider
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task DeleteProvider_DeletesProvider()
        {
            // Arrange
            SeedDatabase();
            var controller = new ProvidersController(_context);
            var provider = _context.Provider.First();
            // Act
            var result = await controller.DeleteProvider(provider.Id);
            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.Null(_context.Provider.Find(provider.Id));
        }

        /// <summary>
        /// OrganizationsController Tests
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetOrganizations_ReturnsAllOrganizations()
        {
            // Arrange
            SeedDatabase(); 
            var controller = new OrganizationsController(_context);

            // Act
            var result = await controller.GetOrganizations();

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnOrganizations = Assert.IsType<OrganizationList>(actionResult.Value); 
            Assert.Equal(2, returnOrganizations.Organizations.Count); 
        }
        /// <summary>
        /// GetOrganization_ReturnsOrganizationById
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetOrganization_ReturnsOrganizationById()
        {
            // Arrange
            SeedDatabase(); 
            var controller = new OrganizationsController(_context);
            var organization = _context.Organization.First(); 
            // Act
            var result = await controller.GetOrganization(organization.Id);
            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result.Result); 
            var returnOrganization = Assert.IsType<Organization>(actionResult.Value); 
            Assert.Equal(organization.Id, returnOrganization.Id); 
        }
        /// <summary>
        /// PostOrganization_AddsNewOrganization
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task PostOrganization_AddsNewOrganization()
        {
            // Arrange
            var controller = new OrganizationsController(_context);
            var newOrganization = new Organization
            {
                Name = "New Clinic",
                Type = "Clinic",
                Address = "123 New St"
            };
            // Act
            var result = await controller.PostOrganization(newOrganization);
            // Assert
            var actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnOrganization = Assert.IsType<Organization>(actionResult.Value);
            Assert.Equal(newOrganization.Name, returnOrganization.Name);
        }
        /// <summary>
        /// DeleteOrganization_DeletesOrganization
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task DeleteOrganization_DeletesOrganization()
        {
            // Arrange
            SeedDatabase();
            var controller = new OrganizationsController(_context);
            var organization = _context.Organization.First();
            // Act
            var result = await controller.DeleteOrganization(organization.Id);
            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.Null(_context.Organization.Find(organization.Id));
        }
    }
}