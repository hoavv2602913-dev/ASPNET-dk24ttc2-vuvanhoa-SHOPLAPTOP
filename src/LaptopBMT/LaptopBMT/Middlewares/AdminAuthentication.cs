using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace LaptopBMT.Middlewares
{
    public class AdminAuthentication
    {
        private readonly RequestDelegate _next;

        public AdminAuthentication(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value?.ToLower() ?? "";

            // ✅ Cho phép vào Login & Logout mà không kiểm tra Session
            if (path.Contains("/account/login") || path.Contains("/account/logout"))
            {
                await _next(context);
                return;
            }

            // ✅ Chỉ kiểm tra Session nếu truy cập vào trang Admin
            if (path.StartsWith("/admin"))
            {
                var role = context.Session.GetString("Role");
                if (role != "Admin") // nhớ đúng chữ "Admin"
                {
                    context.Response.Redirect("/Account/Login");
                    return;
                }
            }

            await _next(context);
        }

    }
}
