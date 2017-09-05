﻿namespace Gu.Wpf.NumericInput.UITests.DoubleBox
{
    using System;
    using Gu.Wpf.UiAutomation;
    using NUnit.Framework;

    public sealed class CultureWindowTests : IDisposable
    {
        private readonly CultureView view;
        private bool disposed;

        public CultureWindowTests()
        {
            this.view = new CultureView();
        }

        [Test]
        public void TestCultures()
        {
            this.view.ValueTextBox.Enter("1.234");
            Keyboard.Type(Key.TAB);
            Assert.AreEqual("1,234", this.view.SpinnerDoubleBox.Text);
            Assert.AreEqual("1,234", this.view.InheritingCultureDoubleBox.Text);
            Assert.AreEqual("1,234", this.view.SvSeDoubleBox.Text);
            Assert.AreEqual("1.234", this.view.EnUsDoubleBox.Text);
            Assert.AreEqual("1,234", this.view.BoundCultureDoubleBox.Text);

            this.view.CultureTextBox.Enter("en-us");
            Keyboard.Type(Key.TAB);
            Assert.AreEqual("1,234", this.view.SpinnerDoubleBox.Text);
            Assert.AreEqual("1,234", this.view.InheritingCultureDoubleBox.Text);
            Assert.AreEqual("1,234", this.view.SvSeDoubleBox.Text);
            Assert.AreEqual("1.234", this.view.EnUsDoubleBox.Text);
            Assert.AreEqual("1.234", this.view.BoundCultureDoubleBox.Text);
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

        public sealed class CultureView : IDisposable
        {
            private readonly Application application;

            private bool disposed;

            public CultureView()
            {
                this.application = Application.Launch(Info.CreateStartInfo("CultureWindow"));
                this.Window = this.application.MainWindow;
                this.ValueTextBox = this.Window.FindTextBox(nameof(this.ValueTextBox));
                this.SpinnerDoubleBox = this.Window.FindTextBox(nameof(this.SpinnerDoubleBox));
                this.InheritingCultureDoubleBox = this.Window.FindTextBox(nameof(this.InheritingCultureDoubleBox));
                this.SvSeDoubleBox = this.Window.FindTextBox(nameof(this.SvSeDoubleBox));
                this.EnUsDoubleBox = this.Window.FindTextBox(nameof(this.EnUsDoubleBox));
                this.BoundCultureDoubleBox = this.Window.FindTextBox(nameof(this.BoundCultureDoubleBox));
                this.CultureTextBox = this.Window.FindTextBox(nameof(this.CultureTextBox));
            }

            public Window Window { get; }

            public TextBox ValueTextBox { get; }

            public TextBox SpinnerDoubleBox { get; }

            public TextBox InheritingCultureDoubleBox { get; }

            public TextBox SvSeDoubleBox { get; }

            public TextBox EnUsDoubleBox { get; }

            public TextBox BoundCultureDoubleBox { get; }

            public TextBox CultureTextBox { get; }

            public void Dispose()
            {
                if (this.disposed)
                {
                    return;
                }

                this.disposed = true;
                this.application?.Dispose();
            }
        }
    }
}