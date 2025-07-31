# URLz - The URL shortener

A simple ASP.NET Core webapi with two endpoints:
1. POST /shorten - This will shorten a URL (expects a JSON payload with a "URL" property)
2. GET /{short-url} - This will return the full URL (if it has been shortened) and redirect the page