using Core.Configuration;
using Core.EventModels;

namespace Core.Models
{
    public class PaymentInfo : PropertyObject
    {
        private string cardNumber = string.Empty, phoneNumber = string.Empty;
        private int amount;

        public string CardNumber { get => cardNumber; set { if (cardNumber != value) { cardNumber = value; raisePropertyChanged(nameof(CardNumber)); } } }
        public string PhoneNumber { get => phoneNumber; set { if (phoneNumber != value && (value != null && value.Length < 13)) { phoneNumber = value; raisePropertyChanged(nameof(PhoneNumber)); } } }
        public int Amount { get => amount; set { if (amount != value) { amount = value; raisePropertyChanged(nameof(Amount)); } } }

        public PaymentInfo()
        {
        }

        public void Clear()
        {
            PhoneNumber = string.Empty;
            CardNumber = string.Empty;
            Amount = 0;
        }
    }
}
