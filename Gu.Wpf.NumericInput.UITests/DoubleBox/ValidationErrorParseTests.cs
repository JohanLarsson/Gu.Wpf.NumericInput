namespace Gu.Wpf.NumericInput.UITests.DoubleBox
{
    using System;
    using System.Text.RegularExpressions;
    using NUnit.Framework;

    public sealed class ValidationErrorParseTests : IDisposable
    {
        private static readonly TestCase[] TestCases =
            {
                new TestCase("abc", "0", "ValidationError.CanParseValidationResult 'Please enter a valid number.'"),
                new TestCase("2,1", "2", "ValidationError.CanParseValidationResult 'Please enter a valid number.'"),
            };

        private static readonly TestCase[] SwedishCases =
            {
                new TestCase("abc", "0", "ValidationError.CanParseValidationResult 'V�nligen ange en giltig siffra.'"),
                new TestCase("2.1", "2", "ValidationError.CanParseValidationResult 'V�nligen ange en giltig siffra.'"),
            };

        private readonly DoubleBoxValidationView view;
        private bool disposed;

        public ValidationErrorParseTests()
        {
            this.view = new DoubleBoxValidationView();
        }

        [SetUp]
        public void SetUp()
        {
            this.view.Reset();
        }

        [TestCaseSource(nameof(TestCases))]
        public void LostFocusValidateOnLostFocus(TestCase data)
        {
            var boxes = this.view.LostFocusValidateOnLostFocusBoxes;
            var doubleBox = boxes.DoubleBox;
            doubleBox.Text = "0";
            this.view.LoseFocusButton.Click(); // needed to reset explicitly here for some reason

            doubleBox.Text = data.Text;
            this.view.Window.WaitWhileBusy();
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(data.Text, doubleBox.Text);
            Assert.AreEqual("0", this.view.ViewModelValueBox.Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());

            this.view.LoseFocusButton.Click();
            Assert.AreEqual(true, doubleBox.HasValidationError());
            Assert.AreEqual(data.ExpectedInfoMessage, doubleBox.ValidationError());
            Assert.AreEqual(data.ErrorMessage, boxes.ErrorBlock.Text);
            Assert.AreEqual(data.Text, doubleBox.Text);
            Assert.AreEqual("0", this.view.ViewModelValueBox.Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
        }

        [TestCaseSource(nameof(TestCases))]
        public void LostFocusValidateOnPropertyChanged(TestCase data)
        {
            var boxes = this.view.LostFocusValidateOnPropertyChangedBoxes;
            var doubleBox = boxes.DoubleBox;
            doubleBox.Text = data.Text;
            Assert.AreEqual(true, doubleBox.HasValidationError());
            Assert.AreEqual(data.ExpectedInfoMessage, doubleBox.ValidationError());
            Assert.AreEqual(data.ErrorMessage, boxes.ErrorBlock.Text);
            Assert.AreEqual(data.Text, doubleBox.Text);
            Assert.AreEqual("0", this.view.ViewModelValueBox.Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());

            this.view.LoseFocusButton.Click();
            Assert.AreEqual(true, doubleBox.HasValidationError());
            Assert.AreEqual(data.ExpectedInfoMessage, doubleBox.ValidationError());
            Assert.AreEqual(data.Text, doubleBox.Text);
            Assert.AreEqual("0", this.view.ViewModelValueBox.Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
        }

        [TestCaseSource(nameof(TestCases))]
        public void PropertyChanged(TestCase data)
        {
            var boxes = this.view.PropertyChangedValidateOnPropertyChangedBoxes;
            var doubleBox = boxes.DoubleBox;
            doubleBox.Text = data.Text;
            this.view.Window.WaitWhileBusy();
            Assert.AreEqual(true, doubleBox.HasValidationError());
            Assert.AreEqual(data.ErrorMessage, boxes.ErrorBlock.Text);
            Assert.AreEqual(data.ExpectedInfoMessage, doubleBox.ValidationError());
            Assert.AreEqual(data.Text, doubleBox.Text);
            Assert.AreEqual(data.Expected, this.view.ViewModelValueBox.Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
        }

        [TestCaseSource(nameof(SwedishCases))]
        public void PropertyChangedSwedish(TestCase data)
        {
            this.view.CultureBox.Select("sv-SE");
            var boxes = this.view.PropertyChangedValidateOnPropertyChangedBoxes;
            var doubleBox = boxes.DoubleBox;
            doubleBox.Text = data.Text;
            Assert.AreEqual(true, doubleBox.HasValidationError());
            Assert.AreEqual(data.ErrorMessage, boxes.ErrorBlock.Text);
            Assert.AreEqual(data.ExpectedInfoMessage, doubleBox.ValidationError());
            Assert.AreEqual(data.Text, doubleBox.Text);
            Assert.AreEqual(data.Expected, this.view.ViewModelValueBox.Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
        }

        [TestCaseSource(nameof(TestCases))]
        public void PropertyChangedWhenNotLocalized(TestCase data)
        {
            this.view.CultureBox.Select("ja-JP");
            var boxes = this.view.PropertyChangedValidateOnPropertyChangedBoxes;
            var doubleBox = boxes.DoubleBox;
            doubleBox.Text = data.Text;
            this.view.Window.WaitWhileBusy();
            Assert.AreEqual(true, doubleBox.HasValidationError());
            Assert.AreEqual(data.ErrorMessage, boxes.ErrorBlock.Text);
            Assert.AreEqual(data.ExpectedInfoMessage, doubleBox.ValidationError());
            Assert.AreEqual(data.Text, doubleBox.Text);
            Assert.AreEqual(data.Expected, this.view.ViewModelValueBox.Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
        }

        [TestCase("2.1", "ValidationError.CanParseValidationResult 'Please enter a valid number.'")]
        [TestCase("2", null)]
        public void LostFocusValidateOnPropertyChangedWhenAllowDecimalPointChangesMakingInputInvalid(string text, string infoMessage)
        {
            var boxes = this.view.LostFocusValidateOnPropertyChangedBoxes;
            var doubleBox = boxes.DoubleBox;
            doubleBox.Text = text;
            this.view.LoseFocusButton.Click();
            Assert.AreEqual(false, doubleBox.HasValidationError());

            this.view.AllowDecimalPointBox.Checked = false;
            if (infoMessage != null)
            {
                this.view.Window.WaitWhileBusy();
                Assert.AreEqual(true, doubleBox.HasValidationError());
                Assert.AreEqual(infoMessage, doubleBox.ValidationError());
                Assert.AreEqual(TestCase.GetErrorMessage(infoMessage), boxes.ErrorBlock.Text);
            }
            else
            {
                Assert.AreEqual(false, doubleBox.HasValidationError());
            }
        }

        [TestCase("2.1", "ValidationError.CanParseValidationResult 'Please enter a valid number.'")]
        [TestCase("2", null)]
        public void LostFocusValidateOnPropertyChangedWhenAllowDecimalPointChangesMakingInputValid(string text, string infoMessage)
        {
            this.view.AllowDecimalPointBox.Checked = false;
            var boxes = this.view.LostFocusValidateOnPropertyChangedBoxes;
            var doubleBox = boxes.DoubleBox;
            doubleBox.Text = text;
            this.view.LoseFocusButton.Click();
            if (infoMessage != null)
            {
                this.view.Window.WaitWhileBusy();
                Assert.AreEqual(true, doubleBox.HasValidationError());
                Assert.AreEqual(infoMessage, doubleBox.ValidationError());
                Assert.AreEqual(TestCase.GetErrorMessage(infoMessage), boxes.ErrorBlock.Text);
            }
            else
            {
                Assert.AreEqual(false, doubleBox.HasValidationError());
            }

            this.view.AllowDecimalPointBox.Checked = true;
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(text, doubleBox.Text);
            //// Assert.AreEqual(text, this.view.ViewModelValueBox.Text);
            //// not sure about what to do here.
            //// calling UpdateSource() is easy enough but dunno what
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
        }

        public void Dispose()
        {
            if (this.disposed)
            {
                return;
            }

            this.disposed = true;
            this.view.Dispose();
        }

        public class TestCase
        {
            public TestCase(string text, string expected, string expectedInfoMessage)
            {
                this.Text = text;
                this.Expected = expected;
                this.ExpectedInfoMessage = expectedInfoMessage;
            }

            public string Text { get; }

            public string Expected { get; }

            public string ExpectedInfoMessage { get; }

            public string ErrorMessage => GetErrorMessage(this.ExpectedInfoMessage);

            public static string GetErrorMessage(string infoMessage)
            {
                return Regex.Match(infoMessage, "[^']+'(?<inner>[^']+)'.*").Groups["inner"].Value;
            }

            public override string ToString() => $"Text: {this.Text}, Expected: {this.Expected}, ExpectedMessage: {this.ExpectedInfoMessage}";
        }
    }
}