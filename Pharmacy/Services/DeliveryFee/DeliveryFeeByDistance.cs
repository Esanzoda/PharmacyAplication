using Pharmacy.Exception;

namespace Pharmacy.Services.DeliveryFee;

public interface IDeliveryFeeByDistance
{
    decimal CalCulateDeliveryFee(double distanceKm);
}

public class DeliveryFeeByDistance : IDeliveryFeeByDistance
{
    private readonly decimal _baseFee = 10;
    private readonly decimal _ratePerKm = 2;
    private readonly decimal _maxDistanceKm = 100;

    public decimal CalCulateDeliveryFee(double distanceKm)
    {
        if (_maxDistanceKm < (decimal)distanceKm)
        {
            throw new BusinessException("Delivery distance exceeds the maximum limit");
        }

        decimal deliveryFee;
        if (distanceKm < 3)
        {
            deliveryFee = _baseFee;
        }
        else
        {
            deliveryFee = _baseFee + (_ratePerKm * (decimal)distanceKm);
        }

        return Math.Ceiling(deliveryFee);
    }
}