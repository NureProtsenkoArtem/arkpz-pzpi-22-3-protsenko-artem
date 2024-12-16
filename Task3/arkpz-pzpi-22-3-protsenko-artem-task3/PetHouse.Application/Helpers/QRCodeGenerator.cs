using QRCoder;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Png;
using System.IO;
using PetHouse.Core.Models;

public static class QrCodeGenerator
{
   public static Stream GenerateAndSaveQrCode(Pet pet)
   {
      var qrData = $"PetId: {pet.PetId}\n" +
                   $"UserId: {pet.UserId}\n" +
                   $"PetName: {pet.PetName}\n" +
                   $"PetType: {pet.PetType}\n" +
                   $"PetWeight: {pet.PetWeight}\n" +
                   $"CaloriesPerDay: {pet.CaloriesPerDay}\n" +
                   $"ActivityLevel: {pet.ActivityLevel}\n";


      using (var qrGenerator = new QRCodeGenerator())
      {
         QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrData, QRCodeGenerator.ECCLevel.Q);
         var qrCode = new PngByteQRCode(qrCodeData);
         byte[] qrCodeBytes = qrCode.GetGraphic(20);
         
         var memoryStream = new MemoryStream(qrCodeBytes);
         
         memoryStream.Seek(0, SeekOrigin.Begin);

         return memoryStream;
      }
   }
}