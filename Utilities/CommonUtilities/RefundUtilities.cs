using NUnit.Framework;
using Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentPlanResponses;

namespace Splitit.Automation.NG.Utilities.CommonUtilities
{
    public  class RefundUtilities
    {

        public ResponseFullPlanInfoIpn _responseFullPlanInfoIpn;

        public RefundUtilities()
        {
            _responseFullPlanInfoIpn = new ResponseFullPlanInfoIpn(); 
        }

        public void AfterRefundPlanValidation(ResponseFullPlanInfoIpn.Root jResponseFullPlanInfo, RefundAssertionsObject _refundAssertionsObject)
        {
            var _installmentPlan = jResponseFullPlanInfo.InstallmentPlan;

            Assert.That(_installmentPlan.InstallmentPlanStatus.Code.Equals(_refundAssertionsObject.Status));
            Assert.That(_installmentPlan.Amount.Value.Equals(_refundAssertionsObject.CurentAmount));
            Assert.That(_installmentPlan.RefundAmount.Value.Equals(_refundAssertionsObject.RefundAmount));
            Assert.That(_installmentPlan.OriginalAmount.Value.Equals(_refundAssertionsObject.OriginalAmount));
            Assert.That(_installmentPlan.OutstandingAmount.Value.Equals(_refundAssertionsObject.OutstandingAmount));
        }

        public void AfterRefundInstallmentValidation(ResponseFullPlanInfoIpn.Root jResponseFullPlanInfo, RefundAssertionsObject _refundAssertionsObject)
        {
            var _installment = jResponseFullPlanInfo.InstallmentPlan.Installments[_refundAssertionsObject.InstallmentNumber];

            Assert.That(_installment.Status.Code.Equals(_refundAssertionsObject.Status));
            Assert.That(_installment.IsRefund.Equals(_refundAssertionsObject.IsRefund));
            Assert.That(_installment.Amount.Value.Equals(_refundAssertionsObject.CurentAmount));
            Assert.That(_installment.RefundAmount.Value.Equals(_refundAssertionsObject.RefundAmount));
            Assert.That(_installment.OriginalAmount.Value.Equals(_refundAssertionsObject.OriginalAmount));
        }

    }

    public class RefundAssertionsObject
    {
        public RefundAssertionsObject(string? installmentPlanNumber, string? status, bool isRefund, double curentAmount,
            double refundAmount, double originalAmount, double outstandingAmount, int installmentNumber)
        {
            InstallmentPlanNumber = installmentPlanNumber;
            Status = status;
            IsRefund = isRefund;
            CurentAmount = curentAmount;
            RefundAmount = refundAmount;
            OriginalAmount = originalAmount;
            OutstandingAmount = outstandingAmount;
            InstallmentNumber = installmentNumber;
        }

        public string? InstallmentPlanNumber { get; set; }
        public string? Status { get; set; }
        public bool IsRefund { get; set; }
        public double CurentAmount { get; set; }
        public double RefundAmount { get; set; }
        public double OriginalAmount { get; set; }
        public double OutstandingAmount { get; set; }
        public int InstallmentNumber { get; set; }
    }
}

