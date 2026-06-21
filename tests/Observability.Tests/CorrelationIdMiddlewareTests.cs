using System.Threading.Tasks;
using HealthcareCareCoordination.Observability;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Observability.Tests;

public class CorrelationIdMiddlewareTests
{
    [Fact]
    public async Task InvokeAsync_WhenHeaderIsMissing_GeneratesNewCorrelationId()
    {
        // Arrange
        var nextMock = new Mock<RequestDelegate>();
        var loggerMock = new Mock<ILogger<CorrelationIdMiddleware>>();
        var middleware = new CorrelationIdMiddleware(nextMock.Object, loggerMock.Object);
        var context = new DefaultHttpContext();

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.True(context.Items.ContainsKey(CorrelationIdMiddleware.HeaderName));
        Assert.NotNull(context.Items[CorrelationIdMiddleware.HeaderName]);
        
        // Ensure OnStarting is simulated
        context.Response.OnStarting(async () => await Task.CompletedTask);
        // Note: DefaultHttpContext doesn't naturally fire OnStarting without a server, 
        // but the item dictionary correctly verifies the middleware logic ran.
    }

    [Fact]
    public async Task InvokeAsync_WhenHeaderIsPresent_PreservesCorrelationId()
    {
        // Arrange
        var nextMock = new Mock<RequestDelegate>();
        var loggerMock = new Mock<ILogger<CorrelationIdMiddleware>>();
        var middleware = new CorrelationIdMiddleware(nextMock.Object, loggerMock.Object);
        var context = new DefaultHttpContext();
        var expectedId = "trace-12345";
        context.Request.Headers[CorrelationIdMiddleware.HeaderName] = expectedId;

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.Equal(expectedId, context.Items[CorrelationIdMiddleware.HeaderName]);
    }
}
