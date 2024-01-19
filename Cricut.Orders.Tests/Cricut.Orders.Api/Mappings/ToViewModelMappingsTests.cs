using AutoBogus;
using Cricut.Orders.Api.Mappings;
using Cricut.Orders.Domain.Models;
using Cricut.Orders.Infrastructure;
using FluentAssertions;

namespace Cricut.Orders.Tests.Cricut.Orders.Api.Mappings
{
    [TestClass]
    public class ToViewModelMappingsTests
    {
        [TestMethod]
        public void Order_ToViewModel_MapsCorrectly()
        {
            var domainModel = new AutoFaker<Order>()
                .Generate();

            var viewModel = domainModel.ToViewModel();
            viewModel.Should().BeEquivalentTo(domainModel, opts =>
                opts.Excluding(x => x.OrderItems));

            viewModel.OrderItems.Length.Should().Be(domainModel.OrderItems.Length);
            for (var i = 0; i < viewModel.OrderItems.Length; i++)
            {
                viewModel.OrderItems[i].Should().BeEquivalentTo(domainModel.OrderItems[i], opts =>
                    opts.Excluding(x => x.Total));
            }
        }

        [TestMethod]
        public void Orders_ToViewModel_MapsCorrectly()
        {
            var orderStore = GetConfiguredOrderStore();
            var orderArray = orderStore.GetAllOrdersForCustomerAsync(12345);
            Order[] orders = orderArray.Result;

            // Act
            var viewModels = orders.ToViewModel();

            // Assert
            viewModels.Should().NotBeNull().And.HaveCount(orders.Length);

            for (var i = 0; i < viewModels.Length; i++)
            {
                viewModels[i].Id.Should().Be(orders[i].Id ?? 0);
                viewModels[i].Total.Should().Be(orders[i].Total);
            }
        }

        private OrderStore GetConfiguredOrderStore() =>
          new OrderStore();
    }
}
