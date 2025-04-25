using ProjectPRN222_BL3_Project.Helper;

namespace ProjectPRN222_BL3_Project.Services
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(HttpContext context, VnPaymentRequestModel model);
        VnPaymentResponseModel PaymentExecute(IQueryCollection collections);
    }
}
