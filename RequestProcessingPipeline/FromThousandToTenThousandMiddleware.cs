namespace RequestProcessingPipeline
{
    public class FromThousandToTenThousandMiddleware
    {
        private readonly RequestDelegate _next;

        public FromThousandToTenThousandMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            string? token = context.Request.Query["number"];
            try
            {
                int number = Convert.ToInt32(token);
                number = Math.Abs(number);
                if (number >= 1000 && number <= 9999)
                {
                    string thousands = number / 1000 == 1 ? "one thousand" : $"{number / 1000} thousand";
                    if (number % 1000 == 0)
                    {
                        await context.Response.WriteAsync("Your number is " + thousands);
                    }
                    else
                    {
                        context.Session.SetString("number", (number % 1000).ToString());
                        context.Session.SetString("thousands", thousands);
                        await _next(context);
                    }
                }
                else
                {
                    await _next(context);
                }
            }
            catch (Exception)
            {
                await context.Response.WriteAsync("Incorrect parameter");
            }
        }
    }
}
