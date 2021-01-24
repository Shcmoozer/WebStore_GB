using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace WebStore.Infrastructure.Middleware
{
    public class TestMiddleware
    {
        private readonly RequestDelegate _Next;

        public TestMiddleware(RequestDelegate Next) => _Next = Next;

        public async Task InvokeAsync(HttpContext context)
        {
            //Действие до следующего узла в конвейере

            

            var next = _Next(context);

            //действие во время того, как оставшаяся часть конвейера что-то еще делает с контекстом

            await next; //точка синхронизации

            //Действие по завершении работы оставшейся части конвейера
        }
    }
}
