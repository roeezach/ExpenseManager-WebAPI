﻿using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;

namespace ExpensesManager.Integrations.GetExpensesAPI
{
    internal static class GetExpensesRequestBuilder
    {
        private const string SPLITWISE_BASE_ADDRESS = "https://secure.splitwise.com/api/v3.0/";
        private const string API_NAME_EXPENSES = "get_expenses";
        public static HttpClient apiClient { get; set; }

        internal static void HeaderBuilder()
        {
            apiClient = new HttpClient();
            apiClient.DefaultRequestHeaders.Accept.Clear();
            apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

    internal static void SplitwiseAuthenticationRequestBuilder(IConfiguration configuration)
        {
            string? authorizationString;
            apiClient.BaseAddress = new Uri(SPLITWISE_BASE_ADDRESS + API_NAME_EXPENSES);
            authorizationString = configuration.GetSection("Secrets")["SplitewiseSecret"];

            if (string.IsNullOrEmpty(authorizationString))
            {
                Console.WriteLine();
                Console.WriteLine("Splitwise API key not found in configuration.");
                Console.WriteLine("pleae enter the auturization token of the user");
                authorizationString = Console.ReadLine();
            }

            apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorizationString);
        }

        internal static void SetDateRangeStartEndDateRequest(DateTime dateStart, DateTime dateEnd)
        {
            string dateTimeStart = dateStart.ToString();
            string dateTimeEnd = dateEnd.ToString();
            apiClient.BaseAddress = new Uri(apiClient.BaseAddress, "?dated_after=" + dateTimeStart + "&dated_before=" + dateTimeEnd);
        }
    }
}
