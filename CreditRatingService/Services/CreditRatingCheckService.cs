using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace CreditRatingService
{
    public class CreditRatingCheckService : CreditRatingCheck.CreditRatingCheckBase
    {
        private readonly ILogger<CreditRatingCheckService> _logger;
        private static readonly Dictionary<string, int> customerTrustedCredit = new()
        {
            {"id0201", 10000},
            {"id0417", 5000},
            {"id0306", 15000}
        };

        public static Dictionary<string, int> CustomerTrustedCredit => customerTrustedCredit;

        public CreditRatingCheckService(ILogger<CreditRatingCheckService> logger)
        {
            _logger = logger;
        }

        public override Task<CreditReply> CheckCreditRequest(CreditRequest request, ServerCallContext context)
        {
            return Task.FromResult(new CreditReply
            {
                IsAccepted = IsEligibleForCredit(request.CustomerId, request.Credit)
            });
        }

        private bool IsEligibleForCredit(string customerId, Int32 credit)
        {
            bool isEligible = false;

            if (CustomerTrustedCredit.TryGetValue(customerId, out Int32 maxCredit))
            {
                isEligible = credit <= maxCredit;
            }

            return isEligible;
        }
    }
}