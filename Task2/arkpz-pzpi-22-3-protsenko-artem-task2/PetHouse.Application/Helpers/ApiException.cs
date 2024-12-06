﻿namespace PetHouse.Application.Helpers;
public class ApiException : Exception
{
   public int StatusCode { get; }

   public ApiException(string message, int statusCode) : base(message)
   {
      StatusCode = statusCode;
   }
}
