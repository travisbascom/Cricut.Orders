namespace Cricut.Orders.Domain.Models
{
    public class Order
    {
        public int? Id { get; set; }
        public Customer Customer { get; set; } = new Customer();
        public OrderItem[] OrderItems { get; set; } = Array.Empty<OrderItem>();

        private const double DiscountThreshold = 25.0;  //TODO: remove magic number, replace with database value.

        public double Total
        {
            get
            {
                var orderItemTotal = OrderItems.Sum(x => x.Total);
                if (orderItemTotal >= DiscountThreshold)
                {
                    orderItemTotal = Apply10PercentDiscount(orderItemTotal);
                }

                return orderItemTotal;
            }
        }

        private double Apply10PercentDiscount(double totalToDiscount) => totalToDiscount - (totalToDiscount * .10);
        //TODO: replace 10% magic number with database number
    }
}
