using BS.Services.CashManagementService;
using BS.Services.CashManagementService.Models.Request;
using DA;
using DM.DomainModels;
using Microsoft.VisualStudio.CodeCoverage;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Till.Test
{
    internal class CashManagementTest
    {
        private Initializer initializer;
        private CashManagementService cashManagementService;
        private UnitOfWork unitOfWork;

        [SetUp]
        public void Setup()
        {
            initializer = new Initializer();
            unitOfWork = new UnitOfWork(initializer._dbContext);
            cashManagementService = new CashManagementService(unitOfWork);
        }

        [TestCase("PKR", "FiveNote", 3, "anyForNow")]
        public void AddCash_Should_Return_True_When_Cash_Is_Added(string currency, string type, int count, string userId)
        {
            //Arrange
            RequestAddCash requestAddCash = new RequestAddCash()
            {
                Currency = currency,
                Type = type,
                Count = count
            };

            //initializer.mockMapper.Setup(mapper => mapper.Map<RequestAddCash, CashManagement>(It.IsAny<RequestAddCash>())).
            //Returns(new CashManagement
            //{
            //    Currency = currency,
            //    Type = type,
            //    Count = count
            //});

            //Act
            var result = cashManagementService.AddCash(requestAddCash, userId, initializer.token).Result;

            //Assert
            Assert.That(result, Is.EqualTo(true));
        }

        [TestCase("PKR", "FiveNote", 3, "anyForNow")]
        public void ListWithDetails_Should_Return_True_If_Created_Shift_Retrieved(string currency, string type, int count, string userId)
        {
            //Arrange
            RequestAddCash requestAddCash = new RequestAddCash
            {
                Currency = currency,
                Type = type,
                Count = count
            };

            initializer.mockMapper.Setup(mapper => mapper.Map<RequestAddCash, CashManagement>(It.IsAny<RequestAddCash>())).
            Returns(new CashManagement
            {
                Currency = currency,
                Type = type,
                Count = count
            });

            var result = cashManagementService.AddCash(requestAddCash, userId, initializer.token).Result;

            // Act
            var cashs = cashManagementService.ListCashWithDetails(userId, CancellationToken.None).Result;

            // Assert
            var retrievedCash = cashs.FirstOrDefault(s => s.Currency == currency);
            Assert.That(currency, Is.EqualTo(retrievedCash.Currency));
        }

        [TestCase("PKR", "FiveNote", 3, "anyForNow")]
        public void UpdateCash_Should_Return_Response_When_Update_Succeeds(string currency, string type, int count, string userId)
        {
            // Arrange
            RequestAddCash requestAddShift = new RequestAddCash
            {
                Currency = currency,
                Type = type,
                Count = count
            };

            initializer.mockMapper.Setup(mapper => mapper.Map<RequestAddCash, CashManagement>(It.IsAny<RequestAddCash>())).
            Returns(new CashManagement
            {
                Currency = currency,
                Type = type,
                Count = count
            });

            var existingShift = cashManagementService.AddCash(requestAddShift, userId, initializer.token).Result;

            RequestUpdateCash updateRequest = new RequestUpdateCash
            {
                Currency = "updatedCurrency",
                Type = type,
                Count = count
            };

            // Act
            var result = cashManagementService.UpdateCash(updateRequest, userId, initializer.token).Result;

            // Assert
            //Assert.That(result.First().Currency, Is.EqualTo(updateRequest.Currency));
            Assert.That(result, Is.EqualTo(true));
        }


    }
}
