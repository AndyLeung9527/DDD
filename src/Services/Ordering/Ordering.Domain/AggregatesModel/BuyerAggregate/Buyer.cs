namespace Ordering.Domain.AggregatesModel.BuyerAggregate;

public class Buyer
    :Entity, IAggregateRoot
{
    public string IdentityGuid { get; private set; }
    
    public string Name { get; private set; }

    private List<PaymentMethod> _paymentMethods;

    public IReadOnlyCollection<PaymentMethod> PaymentMethods => _paymentMethods.AsReadOnly();

    protected Buyer()
    {
        _paymentMethods = new List<PaymentMethod>();
    }

    public Buyer(string identity, string name) : this()
    {
        IdentityGuid = !string.IsNullOrWhiteSpace(identity) ? identity : throw new ArgumentNullException(nameof(identity));
        Name = !string.IsNullOrWhiteSpace(name) ? name : throw new ArgumentNullException(nameof(name));
    }

    public PaymentMethod VertifyOrAddPaymentMethod(
        int cardTypeId,string alias,string cardNumber,
        string securityNumber, string cardHolerName,DateTime expiration,int orderId)
    {
        var existingPayment = _paymentMethods
            .SingleOrDefault(p => p.IsEqualTo(cardTypeId, cardNumber, expiration));

        if(existingPayment != null)
        {
            AddDomainEvent(new BuyerAndPaymentMethodVertifiedDomainEvent(this, existingPayment, orderId));

            return existingPayment;
        }

        var payment = new PaymentMethod(cardTypeId,alias,cardNumber,securityNumber,cardHolerName,expiration);

        _paymentMethods.Add(payment);

        AddDomainEvent(new BuyerAndPaymentMethodVertifiedDomainEvent(this, payment, orderId));

        return payment;
    }
}
