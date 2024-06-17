namespace RequestProcessingPipeline
{
    public class FromHundredToThousandMiddleware
    {
        private readonly RequestDelegate _next;

        public FromHundredToThousandMiddleware(RequestDelegate next)
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
                if (number >= 100 && number <= 999)
                {
                    string[] Hundreds = { "one hundred", "two hundred", "three hundred", "four hundred", "five hundred", "six hundred", "seven hundred", "eight hundred", "nine hundred" };
                    if (number % 100 == 0)
                    {
                        await context.Response.WriteAsync("Your number is " + Hundreds[number / 100 - 1]);
                    }
                    else
                    {
                        context.Session.SetString("number", (number % 100).ToString());
                        context.Session.SetString("hundreds", Hundreds[number / 100 - 1]);
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
