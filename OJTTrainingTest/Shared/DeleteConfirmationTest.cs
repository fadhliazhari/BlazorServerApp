using Bunit;
using OJTTraining.Shared;
using Xunit;

namespace OJTTrainingTest
{
    public class DeleteConfirmationTest : TestContext
    {
        [Fact]
        public void WithParamMarkupTest()
        {
            string dialogMessage = "Test Message";

            // Act
            IRenderedComponent<DeleteConfirmation> dialog = RenderComponent<DeleteConfirmation>(parameters =>
            {
                parameters
                .Add(parameter => parameter.Message, dialogMessage);
            });

            // Assert
            dialog.Find("p").MarkupMatches($"<p>{dialogMessage}</p>");
        }

        [Fact]
        public void ButtonTest()
        {
            IRenderedComponent<DeleteConfirmation> dialog = RenderComponent<DeleteConfirmation>();

            // Check if all button can function
            dialog.FindAll("button")[0].Click();
            dialog.FindAll("button")[1].Click();
        }
    }
}
