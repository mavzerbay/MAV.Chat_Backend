using System;

namespace MAV.Chat.Common.Errors
{
    public class ApiResponse
    {
        public ApiResponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

        private string GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "Hata ,işlem tamamlanamadı.",
                401 => "Yetkili değilsiniz.",
                404 => "Aradığınız kaynak bulunamadı.",
                500 => "Sunucu hatası, bir şeyler yanlış gitti hemen ilgileniyoruz.",
                _ => null
            };
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}