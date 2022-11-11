namespace Ordering.Domain.Events;

public class BuyerAndPaymentMethodVertifiedDomainEvent
    : INotification
{
    public Buyer Buyer { get; private set; }
    public PaymentMethod Payment { get; private set; }
    public int OrderId { get; private set; }

    public BuyerAndPaymentMethodVertifiedDomainEvent(Buyer buyer, PaymentMethod payment, int orderId)
    {
        Buyer = buyer;
        Payment = payment;
        OrderId = orderId;
    }
}